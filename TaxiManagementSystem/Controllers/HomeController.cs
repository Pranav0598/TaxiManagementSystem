using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TaxiManagementSystem.Model;
using TaxiManagementSystem.Models;
using TaxiManagementSystem.Security;

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

        public IActionResult Index(EarningsViewModel earningsViewModel)
        {
            string userId = HttpContext.Session.GetString("CurrentUserId");
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Users");


            if (earningsViewModel.UserId == 0)
            {
                earningsViewModel = new EarningsViewModel();
                earningsViewModel.UserId = Int32.Parse(userId);
                earningsViewModel.ShiftDate = DateTime.Now;
            }

            var driver = _context.Driver.FirstOrDefault(x => x.UserId == Int32.Parse(userId));

            earningsViewModel = GetEarningsForDriver(earningsViewModel);
            
            earningsViewModel.AllSchedules = _context.Schedule.Include(c => c.Taxi).Where(x => x.DriverId == earningsViewModel.DriverId)
                .ToList();
            //graph data
            var earnings = _context.Earnings.Where(x=>x.DriverId == driver.DriverId).ToList();
            DateTime firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            var a = earnings?.Where(x => x.DriverId == driver.DriverId && x.ShiftDate >= firstDayOfMonth && x.ShiftDate <= lastDayOfMonth);
            int weeksInMonth = (int)Math.Ceiling(((double)firstDayOfMonth.DayOfWeek + lastDayOfMonth.Day) / 7.0);
            List<DataPoint> dataPoints1 = new List<DataPoint>();

            var startdate = firstDayOfMonth;
            for (int i = 1; i <= weeksInMonth; i++)
            {
                dataPoints1.Add(new DataPoint(startdate.ToString("dd MMM yyy"), earnings.Where(x => x.ShiftDate.Date >= startdate && x.ShiftDate <= startdate.AddDays(7)).Sum(x => x.IncomeEarned) ?? 0));
                startdate = startdate.AddDays(7);
            }

            ViewBag.DataPoints1 = JsonConvert.SerializeObject(dataPoints1);
            return View();
        }

        public EarningsViewModel GetEarningsForDriver(EarningsViewModel earning)
        {
            earning.Earnings = _context.Earnings.Where(x => x.DriverId == earning.DriverId)
                .OrderBy(x => x.ShiftDate).ToList();
            earning.MonthlyEarnings = (double)earning.Earnings.Sum(x => x.IncomeEarned);

            return earning;
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

            MyAccountViewModel vm = new MyAccountViewModel();
            vm.UserId = user.UserId;
            vm.Email = user?.Email;
            vm.FirstName = user?.FirstName;
            vm.LastName = user.LastName;
            vm.UserName = user?.UserName;

            return View(vm);
        }

        public IActionResult OwnerAccount()
        {
            string userId = HttpContext.Session.GetString("CurrentUserId");
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Users");

            int currentUserId = Int32.Parse(userId);

            Users user = _context.Users.Where(e => e.UserId == currentUserId).FirstOrDefault();

            MyAccountViewModel vm = new MyAccountViewModel();
            vm.UserId = user.UserId;
            vm.Email = user?.Email;
            vm.FirstName = user?.FirstName;
            vm.LastName = user.LastName;
            vm.UserName = user?.UserName;

            return View(vm);
        }

        public IActionResult SaveOwnerAccount(MyAccountViewModel model)
        {
            string userId = HttpContext.Session.GetString("CurrentUserId");
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Users");

            int currentUserId = Int32.Parse(userId);

            Users user = _context.Users.Where(e => e.UserId == currentUserId).FirstOrDefault();

          
                user.FirstName = model.FirstName ?? user.FirstName;
            user.LastName = model.LastName ?? user.LastName;
            user.Password = model.Password!= null ?Encryption.Encrypt(model.Password) : user.Password;
            user.Email = model.Email ?? user.Email;
            _context.Users.Update(user);
            _context.SaveChanges();

            MyAccountViewModel vm = new MyAccountViewModel();
            vm.UserId = user.UserId;
            vm.Email = user?.Email;
            vm.FirstName = user?.FirstName;
            vm.LastName = user.LastName;
            vm.UserName = user?.UserName;

            return View("MyAccount", vm);
        }


        public IActionResult SaveMyAccount(MyAccountViewModel model)
        {
            string userId = HttpContext.Session.GetString("CurrentUserId");
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Users");

            int currentUserId = Int32.Parse(userId);

            Users user = _context.Users.Where(e => e.UserId == currentUserId).FirstOrDefault();


            user.FirstName = model.FirstName ?? user.FirstName;
            user.LastName = model.LastName ?? user.LastName;
            user.Password = model.Password != null ? Encryption.Encrypt(model.Password) : user.Password;
            _context.Users.Update(user);
            _context.SaveChanges();

            MyAccountViewModel vm = new MyAccountViewModel();
            vm.UserId = user.UserId;
            vm.Email = user?.Email;
            vm.FirstName = user?.FirstName;
            vm.LastName = user.LastName;
            vm.UserName = user?.UserName;

            return View("OwnerAccount", vm);
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
