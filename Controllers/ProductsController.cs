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
using Microsoft.CodeAnalysis;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace AuctionProject.Controllers
{
    public class ProductsController : Controller
    {
        private IWebHostEnvironment Environment;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProductsController(ApplicationDbContext context, IWebHostEnvironment environment, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            Environment = environment;
            _userManager = userManager;

        }
        [Authorize(Roles = "Admin")]
        // GET: Products
        public async Task<IActionResult> Index()
        {
            var Products = (from e in _context.Product
                            join a in _context.User on e.User.Id equals a.Id
                            select new Product
                            {
                                Id = e.Id,
                                Title = e.Title,
                                Description = e.Description,
                                DateEnd = e.DateEnd,
                                DateStart = e.DateStart,
                                Image = e.Image,
                                PriceStart = e.PriceStart,
                                PriceEnd = e.PriceEnd,
                                PriceFinish = e.PriceFinish,
                                PriceJump = e.PriceJump,
                                User = e.User,
                                UserGet = e.UserGet,
                                IsPublic = e.IsPublic,
                                isBuyNow = e.isBuyNow,

                            });
            return View(Products);
        }
        public async Task<IActionResult> IndexUser()
        {
            ApplicationUser author = await _userManager.GetUserAsync(HttpContext.User) ?? new ApplicationUser();
            var Products = (from e in _context.Product
                            join a in _context.User on e.User.Id equals a.Id
                            where e.User == author
                            select new Product
                            {
                                Id = e.Id,
                                Title = e.Title,
                                Description = e.Description,
                                DateEnd = e.DateEnd,
                                DateStart = e.DateStart,
                                Image = e.Image,
                                PriceStart = e.PriceStart,
                                PriceEnd = e.PriceEnd,
                                PriceFinish = e.PriceFinish,
                                PriceJump = e.PriceJump,
                                User = e.User,
                                UserGet = e.UserGet,
                                IsPublic = e.IsPublic,
                                isBuyNow = e.isBuyNow,

                            }) ;
            return View(Products);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Censorship()
        {
          
            var Products = (from e in _context.Product
                            join a in _context.User on e.User.Id equals a.Id
                           
                            select new Product
                            {
                                Id = e.Id,
                                Title = e.Title,
                                Description = e.Description,
                                DateEnd = e.DateEnd,
                                DateStart = e.DateStart,
                                Image = e.Image,
                                PriceStart = e.PriceStart,
                                PriceEnd = e.PriceEnd,
                                PriceFinish = e.PriceFinish,
                                PriceJump = e.PriceJump,
                                User = e.User,
                                UserGet = e.UserGet,
                                IsPublic = e.IsPublic,
                                isBuyNow = e.isBuyNow,

                            });
            return View(Products);
        }
       
        public async Task<IActionResult> CensorshipAcceptOrCancel(int? id)
        {
           var tempProduct = _context.Product.Where( a => a.Id == id ).FirstOrDefault();
            if (tempProduct.IsPublic == false)
            {
                tempProduct.IsPublic = true;
                await _context.SaveChangesAsync();
                return Json(tempProduct.IsPublic);
            }
            else
            {
                tempProduct.IsPublic = false;
                await _context.SaveChangesAsync();
                return Json(tempProduct.IsPublic);
            }
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }
            //query to find the role of the user
            var colorMembers = (from user in _context.User
                                join role in _context.UserRoles on user.Id equals role.UserId
                                join userrole in _context.Roles on role.RoleId equals userrole.Id
                                select new RoleColor
                                {
                                    Email = user.Email,
                                    Name = userrole.Name
                                }).ToList(); // Make sure to materialize the query into a list

            ViewBag.Color = colorMembers;
            /*           var product = await _context.Product
                           .FirstOrDefaultAsync(m => m.Id == id);*/
            var product = await _context.Product
                   .Include(e => e.User) // Include related User entity
                   .Include(e => e.UserGet) // Include related UserGet entity
                   .FirstOrDefaultAsync(e => e.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        /*public async Task<IActionResult> Create([Bind("Id,Title,Description,IsPublic,DateStart,DateEnd,Image,PriceStart,PriceEnd,PriceJump,PriceFinish")] Product @product, List<IFormFile> files)
        {
            if (ModelState.IsValid)
            {
                string wwwPath = this.Environment.WebRootPath;
                string contentPath = this.Environment.ContentRootPath;
                // kiem tra size
                long size = files.Sum(f => f.Length);
                // duong dan den thu muc + file -> wwwPath se dan den file wwwroot
                var folderPath = Path.Combine(wwwPath, "Upload");

                foreach (var formFile in files)
                {
                    if (formFile.Length > 0)
                    {
                        // add time vao file anh de khong bi trung
                        var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                        // nhan ten minh dat ko bao gom duoi
                        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(formFile.FileName);
                        // lay lai cai duoi file
                        var fileExtension = Path.GetExtension(formFile.FileName);
                        // ket hop lai
                        var newFileName = $"{fileNameWithoutExtension}_{timestamp}{fileExtension}";
                        // file name + cai ten moi
                        var filePath = Path.Combine(folderPath, newFileName);
                        product.Image = newFileName;
                        using (var stream = System.IO.File.Create(filePath))
                        {
                            await formFile.CopyToAsync(stream);
                        }
                    }
                }

                *//* product.User = User.Identity.IsAuthenticated;*//*
                ApplicationUser author = await _userManager.GetUserAsync(HttpContext.User) ?? new ApplicationUser();
                @product.User = author;

                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }*/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,DateStart,DateEnd,Image,PriceStart,PriceEnd,PriceJump,PriceFinish")] Product @product, List<IFormFile> files)
        {
            if (ModelState.IsValid)
            {
                //client
                string wwwPath = this.Environment.WebRootPath;
                //sever
                string contentPath = this.Environment.ContentRootPath;
                // kiem tra size
                long size = files.Sum(f => f.Length);
                // duong dan den thu muc + file -> wwwPath se dan den file wwwroot
                var folderPath = Path.Combine(wwwPath, "Upload");

                foreach (var formFile in files)
                {
                    if (formFile.Length > 0)
                    {
                        // add time vao file anh de khong bi trung
                        var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                        // nhan ten minh dat ko bao gom duoi
                        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(formFile.FileName);
                        // lay lai cai duoi file
                        var fileExtension = Path.GetExtension(formFile.FileName);
                        // ket hop lai
                        var newFileName = $"{fileNameWithoutExtension}_{timestamp}{fileExtension}";
                        // file name + cai ten moi
                        var filePath = Path.Combine(folderPath, newFileName);
                        product.Image = newFileName;
                        using (var stream = System.IO.File.Create(filePath))
                        {
                            await formFile.CopyToAsync(stream);
                        }
                    }
                }

                /* product.User = User.Identity.IsAuthenticated;*/
                ApplicationUser author = await _userManager.GetUserAsync(HttpContext.User) ?? new ApplicationUser();
                @product.User = author;
                product.UserGet = new ApplicationUser
                {
                    // Set other properties if needed
                    PhoneNumber = "0932978748",
                    Email = "No Bidding",
                };
                product.SaveMoney = 0;

                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }


        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,DateStart,DateEnd,Image,PriceStart,PriceEnd,PriceJump,PriceFinish")] Product product, bool IsPublic, bool isBuyNow)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    product.IsPublic = IsPublic;
                    product.isBuyNow = isBuyNow;
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        public async Task<IActionResult> Offer(int? id)
        {
            var product1 = await _context.Product.FirstOrDefaultAsync(m => m.Id == id);
            ApplicationUser author = await _userManager.GetUserAsync(HttpContext.User) ?? new ApplicationUser();
            product1.UserGet = author;
            product1.PriceFinish = product1.PriceStart + product1.PriceJump;
            _context.Update(product1);
            await _context.SaveChangesAsync();
            var Products = (from e in _context.Product
                            join a in _context.User on e.User.Id equals a.Id
                            select new Product
                            {
                                Id = e.Id,
                                Title = e.Title,
                                Description = e.Description,
                                DateEnd = e.DateEnd,
                                DateStart = e.DateStart,
                                Image = e.Image,
                                PriceStart = e.PriceStart,
                                PriceEnd = e.PriceEnd,
                                PriceFinish = e.PriceFinish,
                                PriceJump = e.PriceJump,
                                User = e.User,
                                UserGet = e.UserGet,

                            });
            /*return View("~/Products");*/
                return RedirectToAction(nameof(Index));
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Product == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Product'  is null.");
            }
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                _context.Product.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return (_context.Product?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        //checkList and split
        public void checkListJoin(Product? product, ApplicationUser? author)
        {
            bool checkList = false;
            string listUserJoin = product.ListUserJoin;
            string[] elements;
            if (listUserJoin != null)
            {
                elements = listUserJoin.Split(',');
            }
            else
            {
                elements = new string[0];
            }
            foreach(string element in elements)
            {
                if(element == author.Email)
                {
                    checkList = true;
                }
            }
            if (checkList == false)
            {
                product.ListUserJoin = listUserJoin + "," + author.Email;
            }
        }


        [HttpPost]
        public async Task<IActionResult> Offer(int? productId, float? priceFinish)
        {
            try
            {
                var product = await _context.Product.FirstOrDefaultAsync(m => m.Id == productId);
                List<ApplicationUser> userList = await _context.Users.ToListAsync();
                var userBackMoney = new ApplicationUser();
                foreach (var user in userList)
                {
                    if (user == product.UserGet)
                    {
                        userBackMoney = await _context.User.FirstOrDefaultAsync(m => m.Id == user.Id);
                    }
                }
                ApplicationUser author = await _userManager.GetUserAsync(HttpContext.User) ?? new ApplicationUser();
                if (author.Email == null || string.IsNullOrEmpty(author.Id))
                {
                    return Json(new { success = false, message = "Please Login to Continue" });
                }
                else if (product.isBuyNow == true)
                {
                   
                    return Json(new { success = false, message = "Auction has been ended." });
                }
                else if (product.PriceFinish + product.PriceJump >= product.PriceEnd)
                {
                    return Json(new { success = false, message = "You need to buy this product with highest price" });
                }
                else if (product.DateStart > DateTime.Now)
                {
                    return Json(new { success = false, message = "Aucition comming soon" });
                }
                // check if the auction end
                else if (product.DateEnd <= DateTime.Now)
                {
                    return Json(new { success = false, message = "The product has ended" });
                }

                // Check if the user has enough money to offer
                else if (author.Money < priceFinish)
                {
                    return Json(new { success = false, message = "You don't have enough money to offer." });
                }

                // Check if the user is already the highest bidder
                else if (product.UserGet == author)
                {
                    return Json(new { success = false, message = "You are the highest bidder." });
                }
                else if(product.User == author)
                {
                    return Json(new { success = false, message = "You can not bid because you are the master of this product." });
                }
                else
                {
                    product.UserGet = author;
                    //add author to list

                    if (product.PriceFinish != priceFinish)
                    {
                        return Json(new { success = false, message = "Someone has offered a higher price than the current one." });
                    }
                    else
                    {
                        var tempPriceFinish = product.PriceFinish;
                        var tempSaveMoney = product.SaveMoney;
                        if (tempPriceFinish != null && tempPriceFinish >= 0)
                        {
                            product.PriceFinish = product.PriceJump + tempPriceFinish;
                            product.SaveMoney = product.PriceFinish;
                            author.Money = author.Money - ((int?)product.SaveMoney);
                            userBackMoney.Money = userBackMoney.Money + ((int?)tempSaveMoney);
                        }
                        else
                        {
                            product.PriceFinish = product.PriceStart + product.PriceJump + tempPriceFinish;
                            author.Money = author.Money - ((int?)tempPriceFinish);
                            userBackMoney.Money = userBackMoney.Money + ((int?)tempPriceFinish);
                        }
                        checkListJoin(product, author);
                        // display number notication of old bid
                        if(userBackMoney.NotificationCount == null || userBackMoney.NotificationCount == 0)
                        {
                            userBackMoney.NotificationCount = 1;
                        }
                        else
                        {
                            userBackMoney.NotificationCount += 1;
                        }
                        // creat emessage
                        var notification = new Notification
                        {
                            UserId = userBackMoney.Id, // Set the user ID of the previous highest bidder
                            Message = $"Your bid on the product <a href='/Products/Details/{product.Id}'>{product.Title}</a> has been surpassed.",
                            IsRead = false,
                            Timestamp = DateTime.Now
                        };
                        _context.Notifications.Add(notification);
                        _context.Update(userBackMoney);
                        _context.Update(author);
                        _context.Update(product);
                        await _context.SaveChangesAsync();
                        /*return View("~/Products");*/
                        _ = SendGmailAsync(author.Email, userBackMoney.Email);
                        return Json(new { success = true, message = "Offer placed successfully" });
                    }
                }
            }
            catch
            {
                return Json(new { success = true, message = "Something went wrong" });
            }
        }
        [HttpPost]
        public async Task<IActionResult> OfferWithPrice(int? productId, float? priceFinish, float? price)
        {
            try
            {
                var product = await _context.Product.FirstOrDefaultAsync(m => m.Id == productId);
                List<ApplicationUser> userList = await _context.Users.ToListAsync();
                var userBackMoney = new ApplicationUser();
                foreach (var user in userList)
                {
                    if (user == product.UserGet)
                    {
                        userBackMoney = await _context.User.FirstOrDefaultAsync(m => m.Id == user.Id);
                    }
                }
                ApplicationUser author = await _userManager.GetUserAsync(HttpContext.User) ?? new ApplicationUser();
                if (author.Email == null || string.IsNullOrEmpty(author.Id))
                {
                    return Json(new { success = false, message = "Please Login to Continue" });
                }
                else if (product.isBuyNow == true)
                {
                    return Json(new { success = false, message = "Auction has been ended." });
                }
                else if (product.PriceFinish + product.PriceJump >= product.PriceEnd)
                {
                    return Json(new { success = false, message = "You need to buy this product with highest price" });
                }
                else if (product.DateStart > DateTime.Now)
                {
                    return Json(new { success = false, message = "Aucition comming soon" });
                }
                // check if the auction end
                else if (product.DateEnd <= DateTime.Now)
                {
                    return Json(new { success = false, message = "The product has ended" });
                }

                // Check if the user has enough money to offer
                else if (author.Money < priceFinish)
                {
                    return Json(new { success = false, message = "You don't have enough money to offer." });
                }

                // Check if the user is already the highest bidder
                else if (product.UserGet == author)
                {
                    return Json(new { success = false, message = "You are the highest bidder." });
                }
                else if(product.User == author)
                {
                    return Json(new { success = false, message = "You can not bid because you are the master of this product." });
                }
                else
                {
                    product.UserGet = author;
                    //add author to list

                    if (product.PriceFinish != priceFinish)
                    {
                        return Json(new { success = false, message = "Someone has offered a higher price than the current one." });
                    }
                    else
                    {
                        var tempPriceFinish = product.PriceFinish;
                        var tempSaveMoney = product.SaveMoney;
                        if (tempPriceFinish != null && tempPriceFinish >= 0)
                        {
                            product.PriceFinish = price;
                            product.SaveMoney = product.PriceFinish;
                            author.Money = author.Money - ((int?)product.SaveMoney);
                            userBackMoney.Money = userBackMoney.Money + ((int?)tempSaveMoney);
                        }
                        else
                        {
                            product.PriceFinish = price;
                            author.Money = author.Money - ((int?)tempPriceFinish);
                            userBackMoney.Money = userBackMoney.Money + ((int?)tempPriceFinish);
                        }
                        checkListJoin(product, author);
                        _context.Update(userBackMoney);
                        _context.Update(author);
                        _context.Update(product);
                        await _context.SaveChangesAsync();
                        /*return View("~/Products");*/
                        _ = SendGmailAsync(author.Email, userBackMoney.Email);
                        return Json(new { success = true, message = "Offer placed successfully" });
                    }
                }
            }
            catch
            {
                return Json(new { success = true, message = "Something went wrong" });
            }
        }
        [HttpPost]
        public async Task<IActionResult> BuyNow(int? productId)
        {
            try
            {
                var product = await _context.Product.FirstOrDefaultAsync(m => m.Id == productId);
                List<ApplicationUser> userList = await _context.Users.ToListAsync();
                var userBackMoney = new ApplicationUser();
                foreach (var user in userList)
                {
                    if (user == product.UserGet)
                    {
                        userBackMoney = await _context.User.FirstOrDefaultAsync(m => m.Id == user.Id);
                    }
                }
                ApplicationUser author = await _userManager.GetUserAsync(HttpContext.User) ?? new ApplicationUser();
                if (productId == null)
                {
                    return Json(new { success = false, message = "Invalid product ID" });
                }

                if (product == null)
                {
                    return Json(new { success = false, message = "Product not found" });
                }
                author.Money = (int?)(author.Money - product.PriceEnd);
                userBackMoney.Money = (int?)product.SaveMoney;
                product.UserGet = author;
                product.isBuyNow = true;

                _context.Update(product);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "You made the highest bid and won!" });
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                // You can use a logging framework like Serilog or log to a file/database
                Console.Error.WriteLine($"Error in BuyNow action: {ex.Message}");

                return Json(new { success = false, message = "An error occurred while processing your request" });
            }
        }

        //send mail
        public async Task SendGmailAsync(string? user, string? userbackMoney)
        {
            if (user == null || userbackMoney == null)
            { return; }
            try
            {
                // Create a new MailMessage
                using (var message = new MailMessage())
                {
                    message.From = new MailAddress("duongchilocmail@gmail.com");
                    message.To.Add(userbackMoney);
                    message.Subject = "Notice of auction information";
                    message.Body = $"The user '{user}' offered higher than you '{userbackMoney}'";

                    // Create a new SMTP client
                    using (var smtpClient = new SmtpClient())
                    {
                        smtpClient.Host = "smtp.gmail.com";
                        smtpClient.Port = 587;
                        smtpClient.EnableSsl = true;
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = new NetworkCredential("duongchilocmail@gmail.com", "sallqsnernrfmxkw");

                        // Send the email
                        await smtpClient.SendMailAsync(message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
        /*  [HttpPost]
          public async Task<IActionResult> Offer(int? productId, float? priceFinish)
          {
              var product = await _context.Product.FirstOrDefaultAsync(m => m.Id == productId);
              ApplicationUser author = await _userManager.GetUserAsync(HttpContext.User) ?? new ApplicationUser();
              if (author.Email == null || string.IsNullOrEmpty(author.Id))
              {
                  TempData["ErrorMessage"] = "Please Login to Continue";
                  return RedirectToAction(nameof(Index));
              }
              else
              {
                  product.UserGet = author;
                  checkListJoin(product,author);
                  *//*var tempListUser = product.ListUserJoin;
                  product.ListUserJoin = tempListUser + "," + author.Email;*//*
                  //add author to list

                  if (product.PriceFinish != priceFinish)
                  {
                      TempData["ErrorMessage"] = "Someone has offered a higher price than the current one.";
                  }
                  else
                  {
                      var tempPriceFinish = product.PriceFinish;
                      if (tempPriceFinish != null && tempPriceFinish >= 0)
                      {
                          product.PriceFinish = product.PriceJump + tempPriceFinish;
                      }
                      else
                      {
                          product.PriceFinish = product.PriceStart + product.PriceJump + tempPriceFinish;
                      }
                      _context.Update(product);
                      await _context.SaveChangesAsync();
                      *//*return View("~/Products");*//*
                  }
                  return RedirectToAction("Details", new {id=product.Id});
              }
          }*/

        /* [HttpGet("GetChatMessages")]
         public async Task<ActionResult<IEnumerable<ChatMessage>>> GetChatMessages(int productId)
         {
             var messages = await _context.ChatMessages
                 .Where(c => c.Idproduct == productId)
                 .ToListAsync();

             return Json(messages);
         }*/
        [HttpGet("GetChatMessages")]
        public async Task<ActionResult<IEnumerable<ChatMessage>>> GetChatMessages(int productId, string channel)
        {
            var messages = await _context.ChatMessages
                .Where(c => c.Idproduct == productId && c.Channel == channel)
                .ToListAsync();

            return Json(messages);
        }


        [HttpGet]
        public async Task<IActionResult> GetTimeEnd(int id)
        {
            Product currentProduct = await _context.Product.FirstOrDefaultAsync(a => a.Id == id) ?? new Product();
            return Json(currentProduct.DateEnd);
        }
        [HttpGet]
        public async Task<IActionResult> updatePriceFinish(int id)
        {
            Product currentProduct = await _context.Product.FirstOrDefaultAsync(a => a.Id == id) ?? new Product();
            return Json(currentProduct.PriceFinish);
        }
        [HttpGet]
        public async Task<IActionResult> UpdateUserGet(int id)
        {
            Product currentProduct = await _context.Product
                .Include(p => p.UserGet)
                .FirstOrDefaultAsync(a => a.Id == id) ?? new Product();
            return Json(currentProduct.UserGet.Email);
        }
        [HttpGet]
        public async Task<IActionResult> CloseDateEnd(int id)
        {
            Product currentProduct = await _context.Product.FirstOrDefaultAsync(a => a.Id == id) ?? new Product();
            return Json(currentProduct.DateEnd);
        }
        [HttpGet]
        public async Task<IActionResult> isProductEnd(int id)
        {
            Product currentProduct = await _context.Product.FirstOrDefaultAsync(a => a.Id == id) ?? new Product();
            if (currentProduct.DateEnd == DateTime.Now)
            {
                return Json(true);
            }else
            {
                return Json(false);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetNotifications()
        {
            ApplicationUser author = await _userManager.GetUserAsync(HttpContext.User) ?? new ApplicationUser();
            var notifications = _context.Notifications
                .Where(n => n.UserId == author.Id)
                .OrderByDescending(n => n.Timestamp)
                .ToList();

            return Json(notifications);
        }
    }
}
