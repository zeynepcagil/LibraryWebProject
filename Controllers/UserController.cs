using Library_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            // Giriş yapan user id
            var userId = int.Parse(User.FindFirst("UserId")!.Value);

            // Kullanıcının aktif loan'ları
            var loans = _context.Loans
                .Where(l => l.USER_ID == userId && l.RETURN_DATE == null)
                .ToList();

            // Loan → ViewModel
            var result = loans.Select(l => new MyBookViewModel
            {
                Title = _context.Books
                    .Where(b => b.BOOK_ID == l.BOOK_ID)
                    .Select(b => b.TITLE)
                    .FirstOrDefault() ?? "Unknown",

                DueDate = l.DUE_DATE,
                RemainingDays = (l.DUE_DATE - DateTime.Now).Days
            }).ToList();

            return View(result);
        }
    }
}
