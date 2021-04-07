using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TaxiManagementSystem.Model;
using TaxiManagementSystem.Models;

namespace TaxiManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        private readonly TaxiManagementSystemContext _context;

        public HomeController(TaxiManagementSystemContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult MyEarnings()
        {
            return View();
        }


        public IActionResult MyAccount()
        {
            string userId = HttpContext.Session.GetString("CurrentUserId");
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Users");

            int currentUserId = Int32.Parse(userId);

            Users user = _context.Users.Where(e => e.UserId == currentUserId).FirstOrDefault();

            return View(user ?? new Users());
        }



        public IActionResult Privacy()
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
