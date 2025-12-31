using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using projekt_zaliczeniowy.Areas.Identity.Data;
using projekt_zaliczeniowy.Models;

namespace projekt_zaliczeniowy.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BooksController : Controller
    {
        private readonly AppDbContext _context;
        public BooksController(AppDbContext context)
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

        // GET: BooksController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: BooksController/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(_context.Category, "Id", "Nazwa");
            return View();
        }

        // POST: BooksController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nazwa,Autor,Regal,CategoryId")] Books book)
        {
            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: BooksController/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null) return NotFound();

            var book = await _context.Book.FindAsync(id);
            if (book == null) return NotFound();

            ViewBag.CategoryId = new SelectList(_context.Category, "Id", "Nazwa", book.CategoryId);

            ViewBag.UserId = new SelectList(_context.Users, "Id", "Email", book.UserId);

            return View(book);
        }

        // POST: BooksController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nazwa,Autor,Regal,DataWypozyczenia,UserId,CategoryId")] Books book)
        {
            if (id != book.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Category.Any(e => e.Id == book.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: BooksController/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null) return NotFound();

            // Używamy Include, aby załadować powiązane obiekty
            var book = await _context.Book
                .Include(b => b.Category) // Ładuje kategorię
                .Include(b => b.User)     // Ładuje użytkownika
                .FirstOrDefaultAsync(m => m.Id == id);

            if (book == null) return NotFound();

            return View(book);
        }

        // POST: BooksController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(long id)
        {
            var book = await _context.Book.FindAsync(id);

            if (book != null)
            {
                // Opcjonalnie: sprawdź czy kategoria nie ma przypisanych produktów
                _context.Book.Remove(book);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
