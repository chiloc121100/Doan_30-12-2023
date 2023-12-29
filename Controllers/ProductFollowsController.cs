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
using Microsoft.Identity.Client;

namespace AuctionProject.Controllers
{
    public class ProductFollowsController : Controller
    {
        private IWebHostEnvironment Environment;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProductFollowsController(ApplicationDbContext context, IWebHostEnvironment environment, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            Environment = environment;
            _userManager = userManager;
        }

        // GET: ProductFollows
        public async Task<IActionResult> Index()
        {
              return _context.ProductFollow != null ? 
                          View(await _context.ProductFollow.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.ProductFollow'  is null.");
        }

        // GET: ProductFollows/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ProductFollow == null)
            {
                return NotFound();
            }

            var productFollow = await _context.ProductFollow
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productFollow == null)
            {
                return NotFound();
            }

            return View(productFollow);
        }

        // GET: ProductFollows/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProductFollows/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductId,UserId")] ProductFollow productFollow)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productFollow);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productFollow);
        }

        // GET: ProductFollows/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ProductFollow == null)
            {
                return NotFound();
            }

            var productFollow = await _context.ProductFollow.FindAsync(id);
            if (productFollow == null)
            {
                return NotFound();
            }
            return View(productFollow);
        }

        // POST: ProductFollows/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductId,UserId")] ProductFollow productFollow)
        {
            if (id != productFollow.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productFollow);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductFollowExists(productFollow.Id))
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
            return View(productFollow);
        }

        // GET: ProductFollows/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ProductFollow == null)
            {
                return NotFound();
            }

            var productFollow = await _context.ProductFollow
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productFollow == null)
            {
                return NotFound();
            }

            return View(productFollow);
        }

        // POST: ProductFollows/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ProductFollow == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ProductFollow'  is null.");
            }
            var productFollow = await _context.ProductFollow.FindAsync(id);
            if (productFollow != null)
            {
                _context.ProductFollow.Remove(productFollow);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductFollowExists(int id)
        {
          return (_context.ProductFollow?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> Follow([Bind("Id")] ProductFollow productFollow, int? productId)
        {
            ApplicationUser author = await _userManager.GetUserAsync(HttpContext.User) ?? new ApplicationUser();

            var productFollowList = _context.ProductFollow
                .Where(pf => pf.ProductId == productId)
                .ToList();

            if (ModelState.IsValid)
            {
                // Check if the user has already followed the product
                var existingProductFollow = productFollowList.FirstOrDefault(pf => pf.UserId == author.Id && pf.ProductId == productId);

                if (existingProductFollow != null)
                {
                    // If the user is already following the product, remove the follow
                    _context.Remove(existingProductFollow);
                    productFollow.IsFollow = false;
                }
                else
                {
                    // Create a new ProductFollow and set its properties
                    var newProductFollow = new ProductFollow
                    {
                        UserId = author.Id,
                        ProductId = productId.Value, // Ensure the value is not null
                        IsFollow = true
                    };

                    // Add the new ProductFollow to the context
                    _context.Add(newProductFollow);
                    productFollow.IsFollow = true;
                }

                // Save changes to the database
                await _context.SaveChangesAsync();

                // Redirect to the Index action after successful follow/unfollow
                return Json(productFollow.IsFollow);
            }

            // If ModelState is not valid, return the view with validation errors
            return Json(productFollow.IsFollow);

        }

        public async Task<IActionResult> CheckFollow([Bind("Id")] ProductFollow productFollow, int? productId)
        {
            ApplicationUser author = await _userManager.GetUserAsync(HttpContext.User) ?? new ApplicationUser();
            var productFollowList = _context.ProductFollow
                .Where(pf => pf.ProductId == productId)
                .ToList();

            if (ModelState.IsValid)
            {
                // Check if the user has already followed the product
                var existingProductFollow = productFollowList.FirstOrDefault(pf => pf.UserId == author.Id && pf.ProductId == productId);

                if (existingProductFollow != null)
                {                 
                    return Json(true);
                }
                else
                {
                    return Json(false);
                }
            }
            // If ModelState is not valid, return the view with validation errors
            return Json("ok");

        }


        public async Task<IActionResult> Unfollow(int productId)
        {
            ApplicationUser author = await _userManager.GetUserAsync(HttpContext.User) ?? new ApplicationUser();

            var btnFollow = _context.ProductFollow
                .FirstOrDefault(pf => pf.UserId == author.Id && pf.ProductId == productId);

            if (btnFollow != null)
            {
                if(btnFollow.IsFollow == true)
                {
                    btnFollow.IsFollow = false;
                }
                else
                {
                    btnFollow.IsFollow = true;
                }
                // Toggle IsFollow
                _context.Update(btnFollow);
                await _context.SaveChangesAsync();
                return Json(btnFollow.IsFollow); // Return the updated IsFollow value
            }

            return Json(false); // Indicate that the user was not following the product
        }



    }
}
