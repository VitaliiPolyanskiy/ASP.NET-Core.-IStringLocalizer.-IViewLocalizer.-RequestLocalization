using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using MultilingualSite.Models;

namespace MultilingualSite.Controllers
{
    public class ClubController : Controller
    {
        private readonly ClubContext _context;

        public ClubController(ClubContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Clubs.ToListAsync());
        }

        [HttpGet]
        public IActionResult CreateClub()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateClub(Club club)
        {
            if (ModelState.IsValid)
            {
                _context.Clubs.Add(club);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(club);
        }

        // Сучасний метод зміни мови через стандартні механізми ASP.NET Core
        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl ?? "/");
        }
    }
}