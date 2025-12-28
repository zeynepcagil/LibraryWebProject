using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Library_Project.Models;

namespace Library_Project.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly Context _context;

    public HomeController(ILogger<HomeController> logger, Context context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index(
        string search,
        string category,
        double? minRating)
    {
        var books = _context.Books.AsQueryable();

        //Title + Author search
        if (!string.IsNullOrEmpty(search))
        {
            books = books.Where(b =>
                b.TITLE.Contains(search) ||
                b.AUTHOR.Contains(search));
        }

        //Category filter
        if (!string.IsNullOrEmpty(category))
        {
            books = books.Where(b => b.CATEGORY == category);
        }

        //Rating filter
        if (minRating.HasValue)
        {
            books = books.Where(b => b.RATING >= minRating.Value);
        }

        //Dropdown için kategoriler
        ViewBag.Categories = _context.Books
            .Select(b => b.CATEGORY)
            .Distinct()
            .ToList();

        return View(books.ToList());
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }
}
