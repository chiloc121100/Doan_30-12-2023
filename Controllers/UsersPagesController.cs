using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AuctionProject.Data;
using AuctionProject.Models;
using Microsoft.AspNetCore.Identity;
using AuctionProject.Data.Migrations;

namespace AuctionProject.Controllers
{
    public class UsersPagesController : Controller
    {
        private IWebHostEnvironment Environment;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersPagesController(ApplicationDbContext context, IWebHostEnvironment environment, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            Environment = environment;
            _userManager = userManager;
        }

        // GET: UsersPages
        public async Task<IActionResult> Index(string? id)
        {
            var userPage = await _context.User
                .FirstOrDefaultAsync(m => m.Email == id);
            ViewData["UsersPage"] = userPage;
            // display list follow
            if (userPage != null)
            {
                // Display list of products followed by the user
                var listFollow = _context.ProductFollow
                    .Where(m => m.UserId == userPage.Id).ToList();
                ViewData["ListProductFollow"] = listFollow;
                List<Product> ListProduct = new List<Product>();
                foreach (var templf in listFollow)
                {
                    var tempProduct = _context.Product.SingleOrDefault(m => m.Id == templf.ProductId);
                    if (tempProduct != null)
                    {
                        ListProduct.Add(tempProduct);
                    }
                }
                ViewData["UsersFollow"] = ListProduct;

                List<ProductFollow> ListProductFollow = new List<ProductFollow>();
                

            }
            return View();
        }

        // GET: UsersPages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.UsersPage == null)
            {
                return NotFound();
            }

            var usersPage = await _context.UsersPage
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usersPage == null)
            {
                return NotFound();
            }

            return View(usersPage);
        }

        // GET: UsersPages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UsersPages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserName,PhoneNumber")] UsersPage usersPage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usersPage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(usersPage);
        }

        // GET: UsersPages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.UsersPage == null)
            {
                return NotFound();
            }

            var usersPage = await _context.UsersPage.FindAsync(id);
            if (usersPage == null)
            {
                return NotFound();
            }
            return View(usersPage);
        }

        // POST: UsersPages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserName,PhoneNumber")] UsersPage usersPage)
        {
            if (id != usersPage.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usersPage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsersPageExists(usersPage.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(usersPage);
        }

        // GET: UsersPages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.UsersPage == null)
            {
                return NotFound();
            }

            var usersPage = await _context.UsersPage
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usersPage == null)
            {
                return NotFound();
            }

            return View(usersPage);
        }

        // POST: UsersPages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.UsersPage == null)
            {
                return Problem("Entity set 'ApplicationDbContext.UsersPage'  is null.");
            }
            var usersPage = await _context.UsersPage.FindAsync(id);
            if (usersPage != null)
            {
                _context.UsersPage.Remove(usersPage);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsersPageExists(int id)
        {
          return (_context.UsersPage?.Any(e => e.Id == id)).GetValueOrDefault();
        }

    }
}
