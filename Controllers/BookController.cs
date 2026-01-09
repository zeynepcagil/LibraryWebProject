using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Library_Project.Models;

namespace Library_Project.Controllers
{
    [Authorize]
    public class BookController : Controller
    {
        private readonly Context _context;

        public BookController(Context context)
        {
            _context = context;
        }

        // ==========================================
        // 1. LISTELEME (INDEX)
        // ==========================================
        public IActionResult Index()
        {
            var books = _context.Books.ToList();
            return View(books);
        }

        // ==========================================
        // 2. DETAYLAR VE YORUMLAR (DETAILS)
        // ==========================================
        public IActionResult Details(int id)
        {
            var book = _context.Books
                .Include(b => b.Reviews)
                .ThenInclude(r => r.User)
                .FirstOrDefault(b => b.BOOK_ID == id);

            if (book == null) return NotFound();

            return View(book);
        }

        // ==========================================
        // 3. YORUM EKLEME (ADD REVIEW)
        // ==========================================
        [HttpPost]
        public IActionResult AddReview(int bookId, int rating, string comment)
        {
            int? userId = HttpContext.Session.GetInt32("id");
            if (userId == null) return RedirectToAction("Login", "Account");

            var newReview = new Review
            {
                BOOK_ID = bookId,
                USER_ID = (int)userId,
                RATING = rating,
                COMMENT = comment,
                REVIEW_DATE = DateTime.Now
            };

            _context.Reviews.Add(newReview);
            _context.SaveChanges();

            // Puan ortalamasını güncelle
            var book = _context.Books.Include(b => b.Reviews).FirstOrDefault(b => b.BOOK_ID == bookId);
            if (book != null && book.Reviews.Any())
            {
                book.RATING = Math.Round(book.Reviews.Average(r => r.RATING), 1);
                _context.SaveChanges();
            }

            return RedirectToAction("Details", new { id = bookId });
        }

        // ==========================================
        // 4. KİTAP EKLEME (CREATE) - SADECE ADMIN
        // ==========================================
        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public async Task<IActionResult> Create(Book book)
        {
            if (ModelState.IsValid)
            {
                book.STATUS = true;
                book.RATING = 0;
                _context.Books.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // ==========================================
        // 5. İADE ALMA (RETURN) - TEK VE DÜZELTİLMİŞ HALİ
        // ==========================================
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Return(int loanId)
        {
            // Index sayfasından 'loanId' parametresiyle aslında 'BookID' gönderiyoruz.
            // Bu yüzden önce bu kitabın aktif ödünç kaydını bulmalıyız.

            // Kitabı bul
            var book = await _context.Books.FindAsync(loanId);

            // Aktif ödünç kaydını bul (Status: true veya Active olan)
            var loan = await _context.Loans
                .FirstOrDefaultAsync(l => l.BOOK_ID == loanId && l.STATUS == true); // STATUS bool ise

            if (loan == null)
            {
                TempData["Message"] = "⚠️ Bu kitap şu an ödünçte görünmüyor!";
                return RedirectToAction(nameof(Index));
            }

            // --- CEZA HESAPLAMA MANTIĞI (Grace Period: 24 Saat) ---
         // PDF Referansı: REQ-SYS-012 
            double dailyRate = 10.0;
            double fineAmount = 0;

            if (DateTime.Now > loan.DUE_DATE.AddDays(1))
            {
                TimeSpan delay = DateTime.Now - loan.DUE_DATE;
                int lateDays = delay.Days;
                fineAmount = lateDays * dailyRate;
                TempData["Message"] = $"⚠️ Kitap {lateDays} gün gecikti! Ceza: {fineAmount} TL";
            }
            else
            {
                TempData["Message"] = "✅ Kitap zamanında iade alındı.";
            }
            // ----------------------------------------------------

            // İşlemi kapat
            loan.STATUS = false; // Ödüncü kapat
            loan.RETURN_DATE = DateTime.Now;

            if (book != null)
            {
                book.STOCK += 1; // Stoğu artır
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // ==========================================
        // 6. ÖDÜNÇ VERME (LOAN)
        // ==========================================
        [Authorize(Policy = "AdminOnly")]
        public IActionResult Loan(int bookId)
        {
            // Index sayfasından tıklandığında Admin Loan sayfasına yönlendirsin
            return RedirectToAction("Loan", "Admin", new { bookId = bookId });
        }
    }
}