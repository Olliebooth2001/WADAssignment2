using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WAD.Models;
using WAD.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;

namespace WAD.Controllers
{
    public class SecurityController : Controller
    {

        private readonly UserManager<AppIdentityUser> userManager;
        private readonly RoleManager<AppIdentityRole> roleManager;
        private readonly SignInManager<AppIdentityUser> signinManager;

        public SecurityController(UserManager<AppIdentityUser> userManager, RoleManager<AppIdentityRole> roleManager,
            SignInManager<AppIdentityUser> signinManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signinManager = signinManager;
        }


        public IActionResult Register()
        {
            if (HttpContext.Session.GetString("name") == null)
            {
                ViewData["disName"] = "Not Set";
            }
            else
            {
                ViewData["disName"] = HttpContext.Session.GetString("name");
            }
            return View();
        }
        public IActionResult DisplayName()
        {   if (HttpContext.Session.GetString("name") == null)
            {
                ViewData["disName"] = "Not Set";
            }
            else
            {
                ViewData["disName"] = HttpContext.Session.GetString("name");
            }
            return View();
        }
        
        [HttpPost]
        public IActionResult DisplayName(IFormCollection form)
        {
            string newName = form["disName"].ToString();
            HttpContext.Session.SetString("name", newName);
            ViewData["disName"] = newName;

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(Register obj)
        {
            if (ModelState.IsValid)
            {
                if (!roleManager.RoleExistsAsync("Manager").Result)
                {
                    AppIdentityRole role = new AppIdentityRole();
                    role.Name = "Manager";
                    IdentityResult roleResult = roleManager.
                    CreateAsync(role).Result;
                }

                AppIdentityUser user = new AppIdentityUser();
                user.UserName = obj.UserName;
                user.Email = obj.Email;
                user.FullName = obj.FullName;
                user.BirthDate = obj.BirthDate;

                IdentityResult result = userManager.CreateAsync
                (user, obj.Password).Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Manager").Wait();
                    return RedirectToAction("SignIn", "Security");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid user details");
                }
            }
            return View(obj);
        }
        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult SignIn()
        {
            if (HttpContext.Session.GetString("name") == null)
            {
                ViewData["disName"] = "Not Set";
            }
            else
            {
                ViewData["disName"] = HttpContext.Session.GetString("name");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SignIn(SignIn obj)
        {
            if (ModelState.IsValid)
            {
                var result = signinManager.PasswordSignInAsync
                (obj.UserName, obj.Password,
                    obj.RememberMe, false).Result;
                if (result.Succeeded)
                {

                    //return RedirectToAction("Index", "CMS");
                    return RedirectToAction("DisplayName", "Security");

                }
                else
                {
                    ModelState.AddModelError("", "Invalid user details");
                }
               
            }
            return View(obj);
        }
        
       

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult SignOut()
        {
            HttpContext.Session.Clear();
            signinManager.SignOutAsync().Wait();
            return RedirectToAction("SignIn", "Security");
        }


    }
}