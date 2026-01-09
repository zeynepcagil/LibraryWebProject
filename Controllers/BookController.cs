using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Library_Project.Models;

namespace Library_Project.Controllers;

[Authorize]
public class BookController : Controller
{
    private readonly Context _context;

    public BookController(Context context)
    {
        _context = context;
    }

    // All Books (Ana Sayfa)
    public IActionResult Index()
    {
        var books = _context.Books.ToList();
        return View(books);
    }

    // ==========================================
    // YENİ EKLENEN KISIM: DETAYLAR VE YORUMLAR
    // ==========================================

    // Kitap Detayı ve Yorumları Göster
    public IActionResult Details(int id)
    {
        // Kitabı bulurken Yorumları (Reviews) ve Yorumu Yapan Kullanıcıyı (User) dahil et
        var book = _context.Books
            .Include(b => b.Reviews)
            .ThenInclude(r => r.User)
            .FirstOrDefault(b => b.BOOK_ID == id);

        if (book == null) return NotFound();

        return View(book);
    }

    // Yorum Ekleme İşlemi
    [HttpPost]
    public IActionResult AddReview(int bookId, int rating, string comment)
    {
        // Oturum açan kullanıcının ID'sini al
        int? userId = HttpContext.Session.GetInt32("id");

        // Eğer oturum düşmüşse Login'e at
        if (userId == null) return RedirectToAction("Login", "Account");

        // Yeni yorum objesi oluştur
        var newReview = new Review
        {
            BOOK_ID = bookId,
            USER_ID = (int)userId,
            RATING = rating,
            COMMENT = comment,
            REVIEW_DATE = DateTime.Now
        };

        // Veritabanına ekle ve kaydet
        _context.Reviews.Add(newReview);
        _context.SaveChanges();

        // Kitabın detay sayfasına geri dön (Yorumu görsün)
        return RedirectToAction("Details", new { id = bookId });
    }

    // ==========================================
    // ADMIN İŞLEMLERİ (MEVCUT KODLARIN)
    // ==========================================

    // Add Book (Only Admin)
    [Authorize(Policy = "AdminOnly")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Create(Book book)
    {
        if (!ModelState.IsValid)
            return View(book);

        _context.Books.Add(book);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // Lend (Only Admin)
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Loan(int bookId, int userId)
    {
        var book = await _context.Books.FindAsync(bookId);
        var user = await _context.Users.FindAsync(userId);

        if (book == null || user == null)
            return NotFound();

        if (!book.CanBeLoaned())
            return BadRequest("Kitap ödünç verilemez.");

        var loan = new Loan
        {
            BOOK_ID = book.BOOK_ID,
            USER_ID = user.USER_ID,
            LOAN_DATE = DateTime.Now,
            DUE_DATE = DateTime.Now.AddDays(14),
            STATUS = true
        };

        book.DecreaseStock();

        _context.Loans.Add(loan);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // Take Return (Only Admin)
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Return(int loanId)
    {
        var loan = await _context.Loans
            .FirstOrDefaultAsync(l => l.LOAN_ID == loanId);

        if (loan == null)
            return NotFound();

        var book = await _context.Books
            .FirstOrDefaultAsync(b => b.BOOK_ID == loan.BOOK_ID);

        if (book == null)
            return NotFound();

        loan.CloseLoan();
        book.IncreaseStock();

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}