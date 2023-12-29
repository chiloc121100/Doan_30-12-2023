using AuctionProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using System.IO;
using Microsoft.VisualBasic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using AuctionProject.Data;
using Microsoft.EntityFrameworkCore;

using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

// when use stripe to pay i need to change Product = AuctionProject.Models.Product
using Stripe;
using Stripe.Checkout;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AuctionProject.Controllers
{
    public class HomeController : Controller
    {
        private IWebHostEnvironment Environment;
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly StripeSettings _stripeSettings;

        public HomeController(IOptions<StripeSettings> stripeSettings,ILogger<HomeController> logger, ApplicationDbContext context, IWebHostEnvironment _environment, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            Environment = _environment;
            _userManager = userManager;
            _context = context;
            _stripeSettings = stripeSettings.Value;
        }

        public IActionResult Index()
        {
            var Productstemp = (from e in _context.Product
                            join a in _context.User on e.User.Id equals a.Id
                            select new AuctionProject.Models.Product
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

                            });
            var Products = Productstemp.Where(m => m.IsPublic==true).ToList();
            return View(Products);
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
             var productstemp = await (from e in _context.Product
                          join a in _context.User on e.User.Id equals a.Id
                          select new AuctionProject.Models.Product
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
                          }).ToListAsync(); // Await the asynchronous operation
            /*            var UpComingEvent = products.Where(e => e.DateEnd > DateTime.Now);
                        var PastEvent = products.Where(e => e.DateEnd < DateTime.Now);
                        ViewBag.UpComingEvents = UpComingEvent;
                        ViewBag.PastEvents = PastEvent;*/
            var products = productstemp.Where(m => m.IsPublic == true).ToList();
            return Json(products);
        }

        [HttpGet]
        public async Task<IActionResult> PastEvent()
        {
            var products = await (from e in _context.Product
                                  join a in _context.User on e.User.Id equals a.Id
                                  select new AuctionProject.Models.Product
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

                                  }).ToListAsync(); // Await the asynchronous operation
           /* var UpComingEvent = products.Where(e => e.DateEnd > DateTime.Now);*/
            var PastEvent = products.Where(e => e.DateEnd < DateTime.Now);
            /*ViewBag.UpComingEvents = UpComingEvent;
            ViewBag.PastEvents = PastEvent;*/
            return Json(PastEvent);
        }

        [HttpGet]
        public async Task<IActionResult> NowEvent()
        {
            var products = await (from e in _context.Product
                                  join a in _context.User on e.User.Id equals a.Id
                                  select new AuctionProject.Models.Product
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

                                  }).ToListAsync(); // Await the asynchronous operation
             var UpComingEvent = products.Where(e => e.DateEnd > DateTime.Now);
            //var PastEvent = products.Where(e => e.DateEnd < DateTime.Now);
            /*ViewBag.UpComingEvents = UpComingEvent;
            ViewBag.PastEvents = PastEvent;*/
            return Json(UpComingEvent);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Privacy()
        {
            var Products = (from e in _context.Product
                            join a in _context.User on e.User.Id equals a.Id
                            select new AuctionProject.Models.Product
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
            var UpComingEvent = Products.Where(e => e.DateEnd > DateTime.Now);
            var PastEvent = Products.Where(e => e.DateEnd < DateTime.Now);
            ViewBag.UpComingEvents = UpComingEvent;
            ViewBag.PastEvents = PastEvent;
            return View(Products);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /* public async Task<IActionResult> OnPostUploadAsync(List<IFormFile> files)
         {
             string wwwPath = this.Environment.WebRootPath;
             string contentPath = this.Environment.ContentRootPath;
             long size = files.Sum(f => f.Length);
             var folderPath = Path.Combine(wwwPath,"Upload"); // duong dan den thu muc + file

             var time = DateAndTime.Now;
             foreach (var formFile in files)
             {
                 if (formFile.Length > 0)
                 {
                     var filePath = Path.Combine(folderPath, formFile.FileName);
                     using (var stream = System.IO.File.Create(filePath))
                     {                 
                         await formFile.CopyToAsync(stream);
                     }
                 }
             }

             // Process uploaded files
             // Don't rely on or trust the FileName property without validation.
             *//*return View();*//*
             return Ok(new { count = files.Count, size });
         }*/
        public async Task<IActionResult> OnPostUploadAsync(List<IFormFile> files)
        {
            // nhan moi truong tu server hoac may tinh
            string wwwPath = this.Environment.WebRootPath;
            string contentPath = this.Environment.ContentRootPath;
            // kiem tra size
            long size = files.Sum(f => f.Length);
            // duong dan den thu muc + file -> wwwPath se dan den file wwwroot
            var folderPath = Path.Combine(contentPath, "Upload");

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
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }

            // Process uploaded files
            // Don't rely on or trust the FileName property without validation.
            /*return View();*/
            return Ok(new { count = files.Count, size });
        }
        //offer
        [HttpPost]
        public async Task<IActionResult> Offer(int? productId, float? priceFinish)
        {
            var product = await _context.Product.FirstOrDefaultAsync(m => m.Id == productId);
            List<ApplicationUser> userList = await _context.Users.ToListAsync();
            var userBackMoney = new ApplicationUser();
            foreach (var user in userList)
            {
                if(user == product.UserGet)
                {
                    userBackMoney = await _context.User.FirstOrDefaultAsync(m => m.Id == user.Id);
                }
            }
            ApplicationUser author = await _userManager.GetUserAsync(HttpContext.User) ?? new ApplicationUser();
            if(author.Email == null || string.IsNullOrEmpty(author.Id))
            {
                return Json("Please Login to Continue");
            }
            // check if the auction end
            else if (product.DateEnd <= DateTime.Now)
            {
                return Json("The product ended");
            }
            else if (author.Money < priceFinish)
            {
                return Json("You dont have enough money to offer");
            }
            else if(product.UserGet == author)
            {
                return Json("You are the highest bidder");
            }
            else
            { 
                product.UserGet = author;
                //add author to list
            
                    if (product.PriceFinish  != priceFinish)
                    {
                        TempData["ErrorMessage"] = "Someone has offered a higher price than the current one.";
                    }
                    else
                    {
                        var tempPriceFinish = product.PriceFinish;
                        if (tempPriceFinish != null && tempPriceFinish >= 0)
                        {
                            product.PriceFinish = product.PriceJump + tempPriceFinish;
                            author.Money = author.Money - ((int?)tempPriceFinish);
                            userBackMoney.Money = userBackMoney.Money + ((int?)tempPriceFinish);
                        }
                        else
                        {
                            product.PriceFinish = product.PriceStart + product.PriceJump + tempPriceFinish;
                            author.Money = author.Money - ((int?)tempPriceFinish);
                            userBackMoney.Money = userBackMoney.Money + ((int?)tempPriceFinish);
                        }
                         checkListJoin(product, author);
                        _context.Update(userBackMoney);
                        _context.Update(author);
                        _context.Update(product);
                        await _context.SaveChangesAsync();
                        /*return View("~/Products");*/
                    }
                await SendGmailAsync(author.Email,userBackMoney.Email);
                return Json("Success");
            }
        }

        //send mail
        public async Task SendGmailAsync(string? user, string? userbackMoney)
        {
            if(user == null || userbackMoney == null)
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


        public void checkListJoin(AuctionProject.Models.Product? product, ApplicationUser? author)
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
            foreach (string element in elements)
            {
                if (element == author.Email)
                {
                    checkList = true;
                }
            }
            if (checkList == false)
            {
                product.ListUserJoin = listUserJoin + "," + author.Email;
            }
        }




        [HttpGet]
        public async Task<IActionResult> GetMoneyBalance()
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User) ?? new ApplicationUser();
            return Json(currentUser.Money);
        }



        // pay
        // card 4242 4242 4242 4242 01/25 123                                                               
        public IActionResult DonatePage()
        {
            return View();
        }

        public IActionResult CreateCheckoutSession(string amount)
        {
            var currency = "usd";// ... (previous code remains unchanged)

            var successUrl = "https://localhost:7000/Home/success";
            var cancelUrl = "https://localhost:7000/Home/cancel";
            StripeConfiguration.ApiKey = _stripeSettings.SecretKey;

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
            {
                "card"
            },
                LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = currency,
                        UnitAmount = Convert.ToInt32(amount) * 100,
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Product Name",
                            Description = "Product Description"
                        }
                    },
                    Quantity = 1
                }
            },
                Mode = "payment",
                SuccessUrl = successUrl,
                CancelUrl = cancelUrl
            };

            var service = new SessionService();
            var session = service.Create(options);

            TempData["Amount"] = amount; // Store the amount in TempData

            return Redirect(session.Url);
        }

        public async Task<IActionResult> success()
        {
            // Get the current user
            ApplicationUser author = await _userManager.GetUserAsync(HttpContext.User) ?? new ApplicationUser();

            // Retrieve the amount from TempData
            var amount = TempData["Amount"]?.ToString();

            // Parse amount to integer (or float if Money is a float property)
            if (int.TryParse(amount, out int amountInt))
            {
                // Assign the parsed amount to the Money property
                author.Money += amountInt;
            }
            else
            {
                // Handle the case where the conversion fails
                // You might want to log an error or take appropriate action
            }

            // Update the user in the database
            _context.Update(author);
            await _context.SaveChangesAsync();

            // Pass the amount to the view
            ViewData["Amount"] = amount;

            // Return to the DonatePage view
            return View("DonatePage");
        }

       

        public IActionResult cancel()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Erro()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



        // pay withdraw
        public IActionResult WithdrawPage()
        {
            return View();
        }
        public async Task<IActionResult> InitiateWithdrawal(string amount)
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User) ?? new ApplicationUser();

            if (user.Money >= Convert.ToInt32(amount))
            {
                var currency = "usd";
                // Assuming you are in a controller or have access to the UrlHelper

                // Example in a controller action method
                var successUrl = Url.Action("WithdrawalSuccess", "Home", null, Request.Scheme);
                var cancelUrl = Url.Action("WithdrawalCancel", "Home", null, Request.Scheme);

                StripeConfiguration.ApiKey = _stripeSettings.SecretKey;

                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string>
            {
                "card"
            },
                    LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = currency,
                        UnitAmount = Convert.ToInt32(amount) * 100,
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Withdrawal",
                            Description = "Withdrawal from your account"
                        }
                    },
                    Quantity = 1
                }
            },
                    Mode = "payment",
                    SuccessUrl = successUrl,
                    CancelUrl = cancelUrl,

                };

                var service = new SessionService();
                var session = service.Create(options);

                TempData["WithdrawalAmount"] = amount; // Store the withdrawal amount in TempData

                return Redirect(session.Url);
            }
            else
            {
                TempData["WithdrawalMessage"] = "Insufficient funds for withdrawal.";
                return RedirectToAction("WithdrawPage");
            }
        }

        public async Task<IActionResult> WithdrawalSuccess()
        {
            ApplicationUser author = await _userManager.GetUserAsync(HttpContext.User) ?? new ApplicationUser();

            // Retrieve the withdrawal amount from TempData using the correct key
            var amount = TempData["WithdrawalAmount"]?.ToString();

            // Parse amount to integer (or float if Money is a float property)
            if (int.TryParse(amount, out int amountInt))
            {
                // Deduct the withdrawal amount from the user's account
                author.Money -= amountInt;

                // Update the user in the database
                _context.Update(author);
                await _context.SaveChangesAsync();

                // Add any additional logic you need for withdrawal success
                ViewData["WithdrawalAmount"] = amount;
            }
            else
            {
                // Handle the case where the conversion fails
                // You might want to log an error or take appropriate action
            }

            return View("WithdrawPage"); // Create a view for withdrawal success
        }


        public IActionResult WithdrawalCancel()
        {
            // Handle cancellation logic if needed

            ViewData["WithdrawalAmount"] = "Withdrawal canceled.";

            return View("WithdrawPage"); // Create a view for withdrawal cancellation
        }




        public async Task<IActionResult> WithdrawHistory()
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User) ?? new ApplicationUser();

            // Retrieve withdrawal history for the current user
            List<Withdrawal> withdrawals = _context.Withdrawals.Where(w => w.User.Id == user.Id).ToList();

            return View(withdrawals);
        }




        // notification
        // update count
        [HttpPost]
        public async Task<IActionResult> UpdateNotificationCount()
        {
            // Assuming you have access to the current user
            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User) ?? new ApplicationUser();

            // Reset the NotificationCount to 0
            if (currentUser != null)
            {
                currentUser.NotificationCount = 0;
                _context.Update(currentUser);
                await _context.SaveChangesAsync();
            }

            return Ok(); 
        }
        // update count real time
        [HttpGet]
        public async Task<IActionResult> GetNotificationCount()
        {
            // Get the current user
            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User) ?? new ApplicationUser();

            // Return the notification count as JSON
            return Json(currentUser.NotificationCount);
        }

        // list notication of the author
        [HttpGet]
        public async Task<IActionResult> GetNotifications()
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(HttpContext.User) ?? new ApplicationUser();
            // Retrieve notifications from your data source (e.g., database)
            var notifications = _context.Notifications
                .Where(n => n.UserId == currentUser.Id)
                .OrderByDescending(n => n.Id)
                .Select(n => new { Timestamp = n.Timestamp, Message = n.Message, IsRead = n.IsRead })
                .ToList();
            return Json(notifications);
        }


    }
}