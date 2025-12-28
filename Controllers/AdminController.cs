using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Library_Project.Models;

[Authorize(Policy = "AdminOnly")] // Sadece adminler girebilsin
public class AdminController : Controller
{
    private readonly Context _context;

    public AdminController(Context context)
    {
        _context = context;
    }

    // GET: Ödünç verme sayfasını açar
    [HttpGet]
    public IActionResult Loan()
    {
        // Stokta olan ve aktif kitapları getir
        ViewBag.Books = _context.Books
            .Where(b => b.STOCK > 0 && b.STATUS == true)
            .ToList();

        // Tüm kullanıcıları getir (Admin hariç tutulabilir istersen)
        ViewBag.Users = _context.Users
            .OrderBy(u => u.NAME)
            .ToList();

        return View();
    }

    // POST: Form gönderildiğinde çalışır
    [HttpPost]
    public IActionResult Loan(int bookId, int userId)
    {
        var book = _context.Books.Find(bookId);
        var user = _context.Users.Find(userId);

        if (book == null || user == null)
            return BadRequest("Kitap veya Kullanıcı bulunamadı.");

        if (book.STOCK <= 0)
            return BadRequest("Bu kitabın stoğu tükenmiş.");

        // Ödünç kaydı oluştur
        var loan = new Loan
        {
            BOOK_ID = book.BOOK_ID,
            USER_ID = user.USER_ID,
            LOAN_DATE = DateTime.Now,
            DUE_DATE = DateTime.Now.AddDays(14), // 14 gün süre
            STATUS = true // Aktif ödünç
        };

        // Stok düş
        book.DecreaseStock();

        _context.Loans.Add(loan);
        _context.SaveChanges();

        // İşlem bitince tekrar sayfaya dön veya mesaj ver
        TempData["Success"] = "Kitap başarıyla ödünç verildi!";
        return RedirectToAction("Loan");
    }
}