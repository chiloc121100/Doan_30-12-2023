using AuctionProject.Data;
using AuctionProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionProject.Controllers
{
    public class ApplicationUsersController : Controller
    {
        private IWebHostEnvironment Environment;
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public ApplicationUsersController(ILogger<HomeController> logger, ApplicationDbContext context, IWebHostEnvironment _environment, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            Environment = _environment;
            _userManager = userManager;
            _context = context;
        }
        [Authorize(Roles = "Admin,Mod")]
        public IActionResult Index()
        {
            var usersData = _context.Users;
            return View(usersData);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string? email)
        {
            if (email == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(m => m.Email == email);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string email, int money)
        {
            if (email == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(m => m.Email == email);

            if (user == null)
            {
                return NotFound();
            }

            // Update the user's money
            user.Money = money;

            try
            {
                // Save the changes to the database
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); // Redirect to the user list after successful edit
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(email))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        private bool UserExists(string email)
        {
            throw new NotImplementedException();
        }
    }
}
