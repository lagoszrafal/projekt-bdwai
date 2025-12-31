using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projekt_zaliczeniowy.Areas.Identity.Data;
using projekt_zaliczeniowy.Models;


namespace projekt_zaliczeniowy.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
{
        private readonly AppDbContext _context;
        public CategoriesController(AppDbContext context)
        {
            _context = context;
        }
        // GET: CategoriesController
        public async Task<IActionResult> Index()
        {
            var categories = await _context.Category.ToListAsync();
            return View(categories);
        }

    // GET: CategoriesController/Details/5
    public ActionResult Details(int id)
    {
        return View();
    }

    // GET: CategoriesController/Create
    public ActionResult Create()
    {
        return View();
    }

    // POST: CategoriesController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nazwa")] Categories category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: CategoriesController/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var category = await _context.Category.FindAsync(id);
            if (category == null) return NotFound();

            return View(category);
        }

        // POST: CategoriesController/Edit/5
        [HttpPost]
    [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nazwa")] Categories category)
        {
            if (id != category.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Category.Any(e => e.Id == category.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: CategoriesController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var category = await _context.Category.FindAsync(id);
            if (category == null) return NotFound();

            return View(category);
        }

    // POST: CategoriesController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Category.FindAsync(id);

            if (category != null)
            {
                // Opcjonalnie: sprawdź czy kategoria nie ma przypisanych produktów
                _context.Category.Remove(category);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
