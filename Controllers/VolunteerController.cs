using GiftOfTheGivers.Data;
using GiftOfTheGivers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GiftOfTheGivers.Controllers
{
    public class VolunteerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VolunteerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Volunteer
        public IActionResult Index()
        {
            return View();
        }

        // POST: Volunteer/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Volunteer volunteer)
        {
            if (ModelState.IsValid)
            {
                volunteer.DateApplied = DateTime.UtcNow;
                volunteer.Status = "Pending";

                _context.Volunteers.Add(volunteer);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Thank you for registering! A coordinator will contact you shortly.";
                return RedirectToAction("Index");
            }

            return View("Index", volunteer);
        }

        // GET: Volunteer/Manage (Employee Role Restricted)
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Manage()
        {
            var volunteers = await _context.Volunteers.OrderByDescending(v => v.DateApplied).ToListAsync();
            return View(volunteers);
        }
    }
}
