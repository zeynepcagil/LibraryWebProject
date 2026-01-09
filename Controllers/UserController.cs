using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Library_Project.Models;

namespace Library_Project.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly Context _context;

        public UserController(Context context)
        {
            _context = context;
        }

        public IActionResult MyBooks()
        {
            // 1. Kullanıcı Kimliği Kontrolü (Session'dan veya Claim'den)
            int? userId = HttpContext.Session.GetInt32("id");

            // Eğer Session düşmüşse ama Cookie duruyorsa Claim'den almayı dene (Yedek plan)
            if (userId == null)
            {
                var claimId = User.FindFirst("UserId")?.Value;
                if (claimId != null)
                {
                    userId = int.Parse(claimId);
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }

            // 2. Verileri Çekme (Include ile tek sorguda kitap bilgilerini de alıyoruz)
            var loans = _context.Loans
                .Include(l => l.Book) // JOIN işlemi yapar, performansı artırır.
                .Where(l => l.USER_ID == userId && l.STATUS == true) // Sadece aktif (iade edilmemiş) olanlar
                .ToList();

            // 3. ViewModel'e Dönüştürme ve Ceza Hesaplama
            var model = new List<MyBookViewModel>();

            foreach (var loan in loans)
            {
                // Kalan gün hesabı (+ ise kalan gün, - ise geciken gün)
                var remaining = (loan.DUE_DATE - DateTime.Now).Days;

                // --- CEZA HESAPLAMA (Grace Period: 24 Saat) ---
                // PDF Referansı: Grace Period Logic [cite: 42, 268]
                double fine = 0;

                // Eğer şu anki zaman, teslim tarihinden 1 gün (24 saat) sonrasını geçmişse
                if (DateTime.Now > loan.DUE_DATE.AddDays(1))
                {
                    // Geciken gün sayısı (Negatif çıkacağı için tersten çıkartıyoruz veya Abs kullanıyoruz)
                    var delay = DateTime.Now - loan.DUE_DATE;
                    int lateDays = delay.Days;

                    // Günlük 10 TL Ceza
                    fine = lateDays * 10.0;
                }
                // ------------------------------------------------

                model.Add(new MyBookViewModel
                {
                    BookId = loan.BOOK_ID,
                    Title = loan.Book != null ? loan.Book.TITLE : "Unknown Book",
                    DueDate = loan.DUE_DATE,
                    RemainingDays = remaining,
                    FineAmount = fine // View tarafında kırmızı göstermek için
                });
            }

            return View(model);
        }
    }
}