using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office.CustomXsn;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TaxiManagementSystem.Model;
using TaxiManagementSystem.Models;
using TaxiManagementSystem.Security;

namespace TaxiManagementSystem.Controllers
{
    public class EarningsController : Controller
    {
        private readonly TaxiManagementSystemContext _context;
        private SmtpClient smtpClient;
        private readonly IConfiguration _config;
        public EarningsController(TaxiManagementSystemContext context, SmtpClient smtpClient, IConfiguration config)
        {
            _context = context;
            this.smtpClient = smtpClient;
            _config = config;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        [HttpGet]
        public IActionResult MyEarnings(EarningsViewModel? earning)
        {
            string userId = HttpContext.Session.GetString("CurrentUserId");
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Users");

            var driverId = _context.Driver.FirstOrDefault(x=>x.UserId == int.Parse(userId)).DriverId;
            

            if (earning.UserId == 0)
            {
                earning = new EarningsViewModel();
                earning.UserId = Int32.Parse(userId);
                earning.ShiftDate = DateTime.Now;                             
            }
            earning.EarningsOn = earning.EarningsOn == new DateTime()? DateTime.Now : earning.EarningsOn;
            earning = GetEarnings(earning);
            earning.IsActive = _context.OwnerDriver.Any(x => x.DriverId == driverId && x.IsActiveDriver != false);
            return View(earning);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddEarningsAsync(EarningsViewModel earningsViewModel)
        {
            earningsViewModel = validateEarning(earningsViewModel);
            if (earningsViewModel.Error != null )
                return RedirectToAction("MyEarnings", "Earnings", new { ShiftDate = earningsViewModel.ShiftDate, EarningsOn = earningsViewModel.EarningsOn, UserId = earningsViewModel.UserId, WeeklyEarnings = earningsViewModel.WeeklyEarnings, MonthlyEarnings = earningsViewModel.MonthlyEarnings });

            Earnings earning = MapViewModelToEarnings(earningsViewModel);

            if (ModelState.IsValid)
            {
                _context.Add(earning);
                await _context.SaveChangesAsync();                
            }
            return RedirectToAction("MyEarnings", "Earnings", new { ShiftDate = earningsViewModel.ShiftDate, EarningsOn = earningsViewModel.EarningsOn, UserId = earning.UserId, WeeklyEarnings = earningsViewModel.WeeklyEarnings, MonthlyEarnings = earningsViewModel.MonthlyEarnings });

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GetEarningsForDate(EarningsViewModel earningsViewModel)
        {
            string userId = HttpContext.Session.GetString("CurrentUserId");
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Users");


            EarningsViewModel earning = new EarningsViewModel();
            earning.EarningsOn = earningsViewModel.EarningsOn;
            earning.UserId = Int32.Parse(userId);
            earning = GetEarnings(earning);
            return RedirectToAction("MyEarnings", "Earnings", new { ShiftDate = earningsViewModel.ShiftDate, EarningsOn = earning.EarningsOn, UserId = earning.UserId, WeeklyEarnings = earning.WeeklyEarnings, MonthlyEarnings = earning.MonthlyEarnings });
        }

        public EarningsViewModel validateEarning(EarningsViewModel earningsViewModel) 
        {
            if (earningsViewModel.ShiftDate == new DateTime()) 
            {
                earningsViewModel.Error = "Invalid date";               
            }

            return earningsViewModel;
        }

        public Earnings MapViewModelToEarnings(EarningsViewModel earningsViewModel) 
        {          
            Earnings earning = new Earnings();
            earning.UserId = earningsViewModel.UserId;
            earning.Earning = earningsViewModel.Earning;
            earning.Expenditure = earningsViewModel.Expenditure;
            earning.ShiftDate = earningsViewModel.ShiftDate;
            earning.IncomeEarned = CalculateIncome(earningsViewModel);
            return earning;
        }

        public double CalculateIncome(EarningsViewModel earningsViewModel) 
        {
            double profit = earningsViewModel.Earning - earningsViewModel.Expenditure;
            if (profit > 0)
                return profit / 2;
            else
                return profit;
        }

        public EarningsViewModel GetEarnings(EarningsViewModel earning) 
        {
            DateTime monday = earning.EarningsOn.AddDays(-(int)earning.EarningsOn.DayOfWeek + (int)DayOfWeek.Monday);
            DateTime sunday = monday.AddDays(6);
            earning.Earnings = _context.Earnings.Where(e => e.ShiftDate>= monday && e.ShiftDate <= sunday).OrderBy(x => x.ShiftDate);

            earning.WeeklyEarnings = earning.Earnings.Select(x => x.IncomeEarned??0).Sum();
            DateTime firstDayOfMonth = new DateTime(earning.EarningsOn.Year, earning.EarningsOn.Month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            earning.MonthlyEarnings = _context.Earnings.Where(e => e.ShiftDate >= firstDayOfMonth && e.ShiftDate <= lastDayOfMonth).Select(x => x.IncomeEarned??0).Sum();
            return earning;
        }

    }
}
