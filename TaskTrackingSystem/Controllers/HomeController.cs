using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SampleWebApp.Data.Interfaces;
using SampleWebApp.Models;

namespace SampleWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUser _user;
        public HomeController(IUser user)
        {
            _user = user;
        }
        public IActionResult Index()
        {
            if (_user.IsCurrentAuthenticated())
            {
                if (_user.IsInRole(_user.GetCurrentUser(), "Administrator"))
                    return RedirectToAction("Index", "Administrator");
                else if (_user.IsInRole(_user.GetCurrentUser(), "Manager"))
                    return RedirectToAction("Index", "Manager");
                else if (_user.IsInRole(_user.GetCurrentUser(), "ITUser"))
                    return RedirectToAction("Index", "ITUser");
            }
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
