using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WAD.Models;
using System.Text.Json;
using System.IO;

namespace WAD.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        const string SessionCart = "_Cart";
        public HomeController(ILogger<HomeController> logger , ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }



        [HttpGet]
        public IActionResult Upload()
        {
            if (HttpContext.Session.GetString("name") == null)
            {
                ViewData["disName"] = "Not set";
            }
            else
            {
                ViewData["disName"] = HttpContext.Session.GetString("name");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            var filePaths = new List<string>();
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {

                    var filePath = Path.Combine(Directory.GetCurrentDirectory() + "/wwwroot/Images/", formFile.FileName);

                    filePaths.Add(filePath);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }
            return RedirectToAction("Index", "CMS");

            //return Ok();
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("name") == null)
            {
                ViewData["disName"] = "Not set";
            }
            else
            {
                ViewData["disName"] = HttpContext.Session.GetString("name");
            }
            return View();
        }
      
        public IActionResult OneFilm()
        {

            Gig model = _context.Gigs.FirstOrDefault();

            return View(model);

        }
        public IActionResult AllFilms()

        {
            if (HttpContext.Session.GetString("name") == null)
            {
                ViewData["disName"] = "Not set";
            }
            else
            {
                ViewData["disName"] = HttpContext.Session.GetString("name");
            }

            List<Gig> model = _context.Gigs.ToList();

            return View(model);

        }
        public IActionResult CheapGigs()
        {

            if (HttpContext.Session.GetString("name") == null)
            {
                ViewData["disName"] = "Not set";
            }
            else
            {
                ViewData["disName"] = HttpContext.Session.GetString("name");
            }


            var gigs = from m in _context.Gigs

                       where m.GigPrice <= 150

                       select m;

            List<Gig> model = gigs.ToList();


            return View(model);

        }
        public IActionResult MediumPriceGigs()
        {

            if (HttpContext.Session.GetString("name") == null)
            {
                ViewData["disName"] = "Not set";
            }
            else
            {
                ViewData["disName"] = HttpContext.Session.GetString("name");
            }


            var gigs = from m in _context.Gigs

                       where m.GigPrice >150 && m.GigPrice <300

                       select m;

            List<Gig> model = gigs.ToList();


            return View(model);

        }
        public IActionResult ExpensiveGigs()
        {
            if (HttpContext.Session.GetString("name") == null)
            {
                ViewData["disName"] = "Not set";
            }
            else
            {
                ViewData["disName"] = HttpContext.Session.GetString("name");
            }

            var gigs = from m in _context.Gigs

                       where m.GigPrice >= 300

                       select m;

            List<Gig> model = gigs.ToList();


            return View(model);

        }

        public IActionResult FilmDetails (int id){
            if (HttpContext.Session.GetString("name") == null)
            {
                ViewData["disName"] = "Not set";
            }
            else
            {
                ViewData["disName"] = HttpContext.Session.GetString("name");
            }
            Gig model = _context.Gigs.Find(id);

            return View(model);
        
        }

        [HttpPost]
        public IActionResult FilmDetails(IFormCollection form)

        {

            int GigID = int.Parse(form["GigID"]);
            string GigTitle = form["GigTitle"].ToString();
            decimal GigPrice = Decimal.Parse(form["GigPrice"]);
            int OrderQuantity = int.Parse(form["OrderQuantity"]);
            CartItem newOrder = new CartItem();
            newOrder.GigID = GigID;
            newOrder.GigTitle = GigTitle;
            newOrder.GigPrice = GigPrice;
            newOrder.OrderQuantity = OrderQuantity;
            newOrder.OrderDate = DateTime.Now;

            var CartList = new List<CartItem>();
            if (HttpContext.Session.GetString(SessionCart) != null)
            {
                string serialJSON = HttpContext.Session.GetString(SessionCart);
                CartList = JsonSerializer.Deserialize<List<CartItem>>(serialJSON);
                //
                var item = CartList.FirstOrDefault(o => o.GigID == GigID);
                if (item != null)
                {
                    item.OrderQuantity += OrderQuantity;
                }
                else
                {
                    CartList.Add(newOrder);
                }

            }
            else
            {
                CartList.Add(newOrder);
            }
            HttpContext.Session.SetString(SessionCart, JsonSerializer.Serialize(CartList));
            return RedirectToAction("FilmDetails");

        }
        [HttpGet]
        public IActionResult ManageCart()
        {
            if (HttpContext.Session.GetString("name") == null)
            {
                ViewData["disName"] = "Not set";
            }
            else
            {
                ViewData["disName"] = HttpContext.Session.GetString("name");
            }

            List<CartItem> cart = new List<CartItem>();
            if (HttpContext.Session.GetString(SessionCart) != null)
            {
                string serialJSON = HttpContext.Session.GetString(SessionCart);
                cart = JsonSerializer.Deserialize<List<CartItem>>(serialJSON);
            }
            if (TempData["msg"] != null)
            {
                ViewBag.msg = TempData["msg"].ToString();
            }
            return View(cart);

        }
        [HttpPost]
        public IActionResult RemoveCartItem(IFormCollection form)
        {
            int GigID = int.Parse(form["GigID"]);
            var CartList = new List<CartItem>();
            if (HttpContext.Session.GetString(SessionCart) != null)
            {
                string serialJSON = HttpContext.Session.GetString(SessionCart);
                CartList = JsonSerializer.Deserialize<List<CartItem>>(serialJSON);
                var item = CartList.FirstOrDefault(o => o.GigID == GigID);
                if (item != null)
                {
                    CartList.Remove(item);
                }

            }

            HttpContext.Session.SetString(SessionCart, JsonSerializer.Serialize(CartList));
            TempData["msg"] = "Item Removed";
            return RedirectToAction("ManageCart");
        }
        public IActionResult Search(String SearchString)
        {
            if (HttpContext.Session.GetString("name") == null)
            {
                ViewData["disName"] = "Not set";
            }
            else
            {
                ViewData["disName"] = HttpContext.Session.GetString("name");
            } 

            if (!string.IsNullOrEmpty(SearchString))

            {


                var gigs = from m in _context.Gigs

                            where m.GigTitle.Contains(SearchString)

                            select m;

                List<Gig> model = gigs.ToList();

                ViewData["SearchString"] = SearchString;

                return View(model);

            }
            else
            {
                return View();
            }
        }
        public IActionResult Gigs()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult PopArtists()
        {
            return View();
        }
        public IActionResult UpComing()
        {
            return View();
        }
        public IActionResult PopSongs()
        {
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
