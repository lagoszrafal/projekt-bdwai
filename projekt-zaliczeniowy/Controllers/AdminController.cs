using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projekt_zaliczeniowy.Areas.Identity.Data;
using projekt_zaliczeniowy.Models;

namespace projekt_zaliczeniowy.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
{
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Users()
        {
            var users = await _userManager.Users.ToListAsync();
            // Tworzymy listę krotek: (AppUser, string)
            var userWithRoles = new List<(AppUser User, string Role)>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userWithRoles.Add((user, roles.FirstOrDefault() ?? "Brak roli"));
            }

            return View(userWithRoles);
        }


        // Wyświetlenie formularza
        [HttpGet]
        public async Task<IActionResult> EditUserRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = _roleManager.Roles.Select(r => r.Name).ToList();

            var model = new EditUserRolesViewModel
            {
                UserId = userId,
                UserEmail = user.Email,
                CurrentRoles = userRoles,
                AllRoles = allRoles
            };

            return View(model);
        }

        // Zapisanie zmian
        [HttpPost]
        public async Task<IActionResult> UpdateRoles(string userId, string selectedRole)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var roles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, roles); // Usuwamy stare role
            await _userManager.AddToRoleAsync(user, selectedRole); // Dodajemy nową

            return RedirectToAction("Users");
        }
        public ActionResult Index()
    {
        return View();
    }

    // GET: AdminController/Details/5
    public ActionResult Details(int id)
    {
        return View();
    }

    // GET: AdminController/Create
    public ActionResult Create()
    {
        return View();
    }

    // POST: AdminController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(IFormCollection collection)
    {
        try
        {
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }

    // GET: AdminController/Edit/5
    public ActionResult Edit(int id)
    {
        return View();
    }

    // POST: AdminController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(int id, IFormCollection collection)
    {
        try
        {
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }

    // GET: AdminController/Delete/5
    public ActionResult Delete(int id)
    {
        return View();
    }

    // POST: AdminController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id, IFormCollection collection)
    {
        try
        {
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }
}
}
