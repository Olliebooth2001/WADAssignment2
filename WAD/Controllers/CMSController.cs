using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WAD.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace WAD.Controllers
{
    [Authorize(Roles = "Manager")]
    public class CMSController : Controller
    {
        private readonly ILogger<CMSController> _logger;

        private readonly ApplicationDbContext _context;

        public CMSController(ILogger<CMSController> logger, ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        // GET: /<controller>/
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
            List<Gig> model = _context.Gigs.ToList();
            return View(model);
        }
        [HttpGet]
        public IActionResult AddFilm()
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
        public IActionResult AddFilm(GigForm model)
        {
            if (ModelState.IsValid)
            {
                Gig newGig = new Gig
                {
                    GigTitle = model.GigTitle,
                    GigDescription = model.GigDescription,
                    GigImage = model.GigImage,
                    GigPrice = model.GigPrice,
                    ReleaseDate = DateTime.Now,
                };
                _context.Add(newGig);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View();


        }
        [HttpGet]
        public IActionResult UpdateFilm(int id)
        {
            if (HttpContext.Session.GetString("name") == null)
            {
                ViewData["disName"] = "Not set";
            }
            else
            {
                ViewData["disName"] = HttpContext.Session.GetString("name");
            }
            Gig model = _context.Gigs.Find(id);
            GigForm formModel = new GigForm
            {
                GigID = model.GigID,
                GigTitle = model.GigTitle,
                GigDescription = model.GigDescription,
                GigImage = model.GigImage,
                GigPrice = model.GigPrice,
                ReleaseDate = model.ReleaseDate,
            };
            ViewBag.ImageName = model.GigImage;
            return View(formModel);
        }
        [HttpPost]
        public IActionResult UpdateFilm(GigForm model)
        {
            if (ModelState.IsValid)
            {
                Gig editGig = new Gig
                {
                    GigID = model.GigID,
                    GigTitle = model.GigTitle,
                    GigDescription = model.GigDescription,
                    GigImage = model.GigImage,
                    GigPrice = model.GigPrice,
                    ReleaseDate = model.ReleaseDate
                };
                _context.Gigs.Update(editGig);

                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult DeleteGig(int Id)
        {
            if (HttpContext.Session.GetString("name") == null)
            {
                ViewData["disName"] = "Not set";
            }
            else
            {
                ViewData["disName"] = HttpContext.Session.GetString("name");
            }
            Gig model = _context.Gigs.Find(Id);
            return View(model);
        }
        [HttpPost]
        public IActionResult DeleteGig(Gig model)
        {
            _context.Gigs.Remove(model);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }



    }
}
