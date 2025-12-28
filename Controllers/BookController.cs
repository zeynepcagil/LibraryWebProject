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

    // All Books
    public IActionResult Index()
    {
        var books = _context.Books.ToList();
        return View(books);
    }

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
