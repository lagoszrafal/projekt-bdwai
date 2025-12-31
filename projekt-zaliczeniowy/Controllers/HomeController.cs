using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projekt_zaliczeniowy.Areas.Identity.Data;
using projekt_zaliczeniowy.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace projekt_zaliczeniowy.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        // GET: BooksController
        public async Task<IActionResult> Index()
        {
            var books = await _context.Book
            .Include(b => b.Category)
            .Include(b => b.User)
            .ToListAsync();

            return View(books);
        }
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Wypozyczenia()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var books = await _context.Book
                 .Include(b => b.Category)
                 .Where(b => b.UserId == userId)
                 .ToListAsync();

            ViewBag.UserHistory = await _context.History
                .Where(h => h.UserId == userId)
                .OrderByDescending(h => h.Id)
                .ToListAsync();

            return View(books);
        }

        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Borrow(long? id)
        {
            if (id == null) return NotFound();

            // U¿ywamy Include, aby za³adowaæ powi¹zane obiekty
            var book = await _context.Book
                .Include(b => b.Category) // £aduje kategoriê
                .Include(b => b.User)     // £aduje u¿ytkownika
                .FirstOrDefaultAsync(m => m.Id == id);

            if (book == null) return NotFound();

            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Borrow(long id)
        {
            var book = await _context.Book
                .Include(b => b.Category) // £aduje kategoriê
                .FirstOrDefaultAsync(m => m.Id == id);

            if (book != null)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                book.UserId = userId;
                book.DataWypozyczenia = DateOnly.FromDateTime(DateTime.Now);

                var historyEntry = new History
                {
                    Dzialanie = $"Wypo¿yczono ksi¹¿kê: {book.Nazwa} autorstwa: {book.Autor} z kategorii: {book.Category.Nazwa}",
                    Data = DateOnly.FromDateTime(DateTime.Now),
                    UserId = userId
                };

                _context.Update(book);
                _context.Add(historyEntry);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Wypozyczenia));
        }

        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Zwroc(long? id)
        {
            if (id == null) return NotFound();

            // U¿ywamy Include, aby za³adowaæ powi¹zane obiekty
            var book = await _context.Book
                .Include(b => b.Category) // £aduje kategoriê
                .Include(b => b.User)     // £aduje u¿ytkownika
                .FirstOrDefaultAsync(m => m.Id == id);

            if (book == null) return NotFound();

            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Zwroc(long id)
        {
            var book = await _context.Book
                .Include(b => b.Category) // £aduje kategoriê
                .FirstOrDefaultAsync(m => m.Id == id);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (book != null && book.UserId==userId)
            {
                book.UserId = null;
                book.DataWypozyczenia = null;

                var historyEntry = new History
                {
                    Dzialanie = $"Zwrócono ksi¹¿kê: {book.Nazwa} autorstwa: {book.Autor} z kategorii: {book.Category.Nazwa}",
                    Data = DateOnly.FromDateTime(DateTime.Now),
                    UserId = userId
                };
                _context.Add(historyEntry);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Wypozyczenia));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
