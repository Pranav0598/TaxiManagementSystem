using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using TaxiManagementSystem.Model;
using TaxiManagementSystem.Models;

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


        [HttpGet]
        public IActionResult MyEarnings(EarningsViewModel earning)
        {
            var userId = HttpContext.Session.GetString("CurrentUserId");
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Users");

            var driver = _context.Driver.FirstOrDefault(x => x.UserId == int.Parse(userId));


            if (earning.UserId == 0)
            {
                earning = new EarningsViewModel();
                earning.UserId = int.Parse(userId);
                earning.ShiftDate = DateTime.Now;
            }

            earning.EarningsOn = earning.EarningsOn == new DateTime() ? DateTime.Now : earning.EarningsOn;
            earning.DriverId = driver.DriverId;
            earning = GetEarnings(earning);
            earning.IsActive =
                _context.Driver.Any(x => x.DriverId == driver.DriverId && x.IsActive != false);
            earning.AllSchedules = _context.Schedule.Include(c => c.Taxi).Where(x => x.DriverId == driver.DriverId)
                .ToList();
            earning.AllTaxis = earning.AllSchedules.Select(x => x.Taxi).ToList();
            return View(earning);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddEarningsAsync(EarningsViewModel earningsViewModel)
        {
            earningsViewModel = validateEarning(earningsViewModel);
            if (earningsViewModel.Error != null)
                return RedirectToAction("MyEarnings", "Earnings",
                    new
                    {
                        earningsViewModel.ShiftDate,
                        earningsViewModel.EarningsOn,
                        earningsViewModel.UserId,
                        earningsViewModel.WeeklyEarnings,
                        earningsViewModel.MonthlyEarnings,
                        earningsViewModel.DriverId
                    });

            var earning = MapViewModelToEarnings(earningsViewModel);
            var earningExists =
                _context.Earnings.FirstOrDefault(
                    x => x.DriverId == earning.DriverId && x.ShiftDate == earning.ShiftDate);

            if (ModelState.IsValid && earningExists == null)
            {
                _context.Add(earning);
                await _context.SaveChangesAsync();
            }
            else
            {
                earningExists.Earning = earning.Earning;
                earningExists.IncomeEarned = earning.IncomeEarned;
                earningExists.Expenditure = earning.Expenditure;
                earningExists.TaxiId = earning.TaxiId;
                _context.Update(earningExists);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("MyEarnings", "Earnings",
                new
                {
                    earningsViewModel.ShiftDate,
                    earningsViewModel.EarningsOn,
                    earning.UserId,
                    earningsViewModel.WeeklyEarnings,
                    earningsViewModel.MonthlyEarnings,
                    earningsViewModel.DriverId
                });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GetEarningsForDate(EarningsViewModel earningsViewModel)
        {
            var userId = HttpContext.Session.GetString("CurrentUserId");
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Users");

            var driver = _context.Driver.First(x => x.UserId == int.Parse(userId));

            earningsViewModel.Earnings = new List<Earnings>();
            var dateearnings = new LinkedList<Earnings>();
            if (earningsViewModel.IncludeDateSearch)
            {
                earningsViewModel.EarningsOn = earningsViewModel.SearchDate;
                earningsViewModel = GetOwnerEarningsForDate(earningsViewModel);
            }

            else if (earningsViewModel.Search != null)
            {
                var searchResult = SearchByDriver(earningsViewModel, driver.DriverId);
                var l1 = earningsViewModel.Earnings.Select(x => x.EarningId).Concat(searchResult.Earnings.Select(x => x.EarningId)).ToList();

                earningsViewModel.Earnings = _context.Earnings.Where(x => l1.Contains(x.EarningId)).ToList();
            }
            else
            {
                earningsViewModel.Earnings = _context.Earnings.ToList();
            }

            if (earningsViewModel?.Earning == null || (!earningsViewModel?.Earnings?.Any() ?? true))
            {
                earningsViewModel.Earnings = new List<Earnings>();
                earningsViewModel.MonthlyEarnings = 0;
                earningsViewModel.WeeklyEarnings = 0;
            }
            else
            {
                earningsViewModel.WeeklyEarnings = earningsViewModel
                    .Earnings.Select(x => x.IncomeEarned ?? 0).Sum();
            }

            earningsViewModel.Earnings = earningsViewModel.Earnings;
            earningsViewModel.MonthlyEarnings = earningsViewModel.MonthlyEarnings;
            earningsViewModel.UserId = int.Parse(userId);


            var earning = new EarningsViewModel();
            earning.EarningsOn = earningsViewModel.EarningsOn;
            earning.UserId = int.Parse(userId);
            return RedirectToAction("MyEarnings", "Earnings",
                new
                {
                    earningsViewModel.ShiftDate,
                    earning.EarningsOn,
                    earning.UserId,
                    earning.WeeklyEarnings,
                    earning.MonthlyEarnings,
                    earningsViewModel.DriverId
                });
        }

        public EarningsViewModel validateEarning(EarningsViewModel earningsViewModel)
        {
            if (earningsViewModel.ShiftDate == new DateTime()) earningsViewModel.Error = "Invalid date";

            return earningsViewModel;
        }

        public Earnings MapViewModelToEarnings(EarningsViewModel earningsViewModel)
        {
            var earning = new Earnings();
            earning.UserId = earningsViewModel.UserId;
            earning.Earning = earningsViewModel.Earning;
            earning.Expenditure = earningsViewModel.Expenditure;
            earning.ShiftDate = earningsViewModel.ShiftDate;
            earning.IncomeEarned = CalculateIncome(earningsViewModel);
            earning.DriverId = earningsViewModel.DriverId;
            earning.TaxiId = earningsViewModel.TaxiId;

            return earning;
        }

        public double CalculateIncome(EarningsViewModel earningsViewModel)
        {
            var profit = earningsViewModel.Earning - earningsViewModel.Expenditure;
            if (profit > 0)
                return profit / 2;
            return profit;
        }

        public EarningsViewModel GetEarnings(EarningsViewModel earning)
        {
            var monday = earning.EarningsOn.AddDays(-(int) earning.EarningsOn.DayOfWeek + (int) DayOfWeek.Monday);
            var sunday = monday.AddDays(6);
            earning.Earnings = _context.Earnings.Where(x => x.DriverId == earning.DriverId).ToList();

            earning.WeeklyEarnings = earning.Earnings.Where(e => e.ShiftDate >= monday && e.ShiftDate <= sunday)
                .OrderBy(x => x.ShiftDate).Select(x => x.IncomeEarned ?? 0).Sum();
            var firstDayOfMonth = new DateTime(earning.EarningsOn.Year, earning.EarningsOn.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            earning.MonthlyEarnings = _context.Earnings
                .Where(e => e.ShiftDate >= firstDayOfMonth && e.ShiftDate <= lastDayOfMonth && e.DriverId == earning.DriverId)
                .Select(x => x.IncomeEarned ?? 0).Sum();
            return earning;
        }


        [HttpGet]
        public IActionResult OwnerEarnings(EarningsViewModel? earning)
        {
            var userId = HttpContext.Session.GetString("CurrentUserId");
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Users");


            if (earning.UserId == 0)
            {
                earning = new EarningsViewModel();
                earning.UserId = int.Parse(userId);
                earning.ShiftDate = DateTime.Now;
            }

            earning.Earnings = _context.Earnings.Include(x => x.Taxi).Include(y => y.Driver).ToList();

            earning.WeeklyEarnings = earning.Earnings.Select(x => x.IncomeEarned ?? 0).Sum();
            var firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            earning.MonthlyEarnings = _context.Earnings
                .Where(e => e.ShiftDate >= firstDayOfMonth && e.ShiftDate <= lastDayOfMonth)
                .Select(x => x.IncomeEarned ?? 0).Sum();

            //graph data
            var earnings = _context.Earnings.Include(x => x.Driver).ToList();
            var income = (from e in earnings
                    group e by e.Driver
                    into g
                    select new {Name = g.Key.FirstName, income = g.Sum(x => x.IncomeEarned)})
                .OrderByDescending(x => x.income);


            var a = earnings?.Where(x => x.ShiftDate.Month == 1).Sum(x => x.IncomeEarned) ?? 0;

            var dataPoints1 = new List<DataPoint>();
            foreach (var i in income) dataPoints1.Add(new DataPoint(i.Name, i.income ?? 0));


            ViewBag.DataPoints1 = JsonConvert.SerializeObject(dataPoints1);
            return View("OwnerEarnings", earning);
        }

        public EarningsViewModel Search(EarningsViewModel earning)
        {
            earning.Earnings = _context.Earnings.Include(x => x.Taxi).Include(x => x.Driver).Where(e =>
                    e.Taxi.Registration.Contains(earning.Search) ||
                    (e.Driver.FirstName + " " + e.Driver.LastName).Contains(earning.Search)).OrderBy(x => x.ShiftDate)
                .ToList();
            earning.WeeklyEarnings = 0;
            earning.MonthlyEarnings = 0;


            earning.Earnings = earning?.Earnings ?? new List<Earnings>();

            return earning;
        }

        public EarningsViewModel SearchByDriver(EarningsViewModel earning, int driverId)
        {
            earning.Earnings = _context.Earnings.Include(x => x.Taxi).Include(x => x.Driver).Where(e =>
                    e.DriverId == driverId && (e.Taxi.Registration.Contains(earning.Search) ||
                                               (e.Driver.FirstName + " " + e.Driver.LastName).Contains(earning.Search)))
                .OrderBy(x => x.ShiftDate).ToList();
            earning.WeeklyEarnings = 0;
            earning.MonthlyEarnings = 0;


            earning.Earnings = earning?.Earnings ?? new List<Earnings>();

            return earning;
        }

        public EarningsViewModel GetOwnerEarningsForDate(EarningsViewModel earning, int? driverId = 0)
        {
            var monday = earning.EarningsOn.AddDays(-(int) earning.EarningsOn.DayOfWeek + (int) DayOfWeek.Monday);
            var sunday = monday.AddDays(6);
            earning.Earnings = _context.Earnings.Where(e => e.ShiftDate >= monday && e.ShiftDate <= sunday)
                .OrderBy(x => x.ShiftDate).ToList();
            if (driverId != null && driverId != 0)
                earning.Earnings = earning.Earnings.Where(x => x.DriverId == driverId).ToList();

            earning.WeeklyEarnings = earning.Earnings.Select(x => x.IncomeEarned ?? 0).Sum();
            var firstDayOfMonth = new DateTime(earning.EarningsOn.Year, earning.EarningsOn.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            earning.MonthlyEarnings = _context.Earnings
                .Where(e => e.ShiftDate >= firstDayOfMonth && e.ShiftDate <= lastDayOfMonth)
                .Select(x => x.IncomeEarned ?? 0).Sum();

            earning.Earnings = earning?.Earnings ?? new List<Earnings>();
            return earning;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GetOwnersEarnings(EarningsViewModel earningsViewModel)
        {
            var userId = HttpContext.Session.GetString("CurrentUserId");
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Users");


            var earning = new EarningsViewModel();
            earning.Earnings = new List<Earnings>();
            var dateearnings = new LinkedList<Earnings>();
            if (earningsViewModel.IncludeDateSearch)
            {
                earningsViewModel.EarningsOn = earningsViewModel.SearchDate;
                earning = GetOwnerEarningsForDate(earningsViewModel);
            }

            else if (earningsViewModel.Search != null)
            {
                var ears = new List<Earnings>();
                var searchResult = Search(earningsViewModel);

                var l1 = earning.Earnings.Select(x => x.EarningId).Concat(searchResult.Earnings.Select(x => x.EarningId)).ToList();

                earning.Earnings = _context.Earnings.Where(x => l1.Contains(x.EarningId)).ToList();
            }
            else
            {
                earning.Earnings = _context.Earnings.ToList();
            }

            if (earning?.Earning == null || (!earning?.Earnings?.Any() ?? true))
            {
                earning.Earnings = new List<Earnings>();
                earning.MonthlyEarnings = 0;
                earning.WeeklyEarnings = 0;
            }
            else
            {
                earning.WeeklyEarnings = earning
                    .Earnings.Select(x => x.IncomeEarned ?? 0).Sum();
            }

            earningsViewModel.Earnings = earning.Earnings;
            earningsViewModel.MonthlyEarnings = earning.MonthlyEarnings;
            earning.UserId = int.Parse(userId);

            //graph data
            var earnings = _context.Earnings.Include(x=>x.Driver).ToList();

            var a = earnings?.Where(x => x.ShiftDate.Month == 1).Sum(x => x.IncomeEarned) ?? 0;

            var list = from e in earnings
                group e by e.Driver
                into g
                select new {Name = g.Key.FirstName + " " + g.Key.LastName, earnings = g.Sum(c => c.Earning)};
                    
            

            var dataPoints1 = new List<DataPoint>();

            foreach (var e in list)
            {
                dataPoints1.Add(new DataPoint(e.Name,e?.earnings??0));
            }


            ViewBag.DataPoints1 = JsonConvert.SerializeObject(dataPoints1);

            return View("OwnerEarnings", earningsViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Export(EarningsViewModel earningsViewModel)
        {
            var listofIds = earningsViewModel?.Earnings?.Select(x => x.EarningId).ToList() ?? new List<int>();
            var earn = _context.Earnings.Include(x => x.Driver).Include(y => y.Taxi).ToList();
            //.Where(x => listofIds.Contains(x.EarningId)).ToList();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Earnings");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Driver";
                worksheet.Cell(currentRow, 2).Value = "Shift Date";
                worksheet.Cell(currentRow, 3).Value = "Vehicle driven";
                worksheet.Cell(currentRow, 4).Value = "Income (AUD)";
                foreach (var e in earn)
                    if (e?.Driver != null && e?.Taxi != null)
                    {
                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = e.Driver.FirstName + " " + e.Driver.LastName;
                        worksheet.Cell(currentRow, 2).Value = e.ShiftDate;
                        worksheet.Cell(currentRow, 3).Value = e.Taxi.Registration;
                        worksheet.Cell(currentRow, 4).Value = "$ " + e.IncomeEarned;
                    }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "earnings.xlsx");
                }
            }
        }

        [HttpGet]
        public IActionResult DriverEarnings(EarningsViewModel? earning)
        {
            var userId = HttpContext.Session.GetString("CurrentUserId");
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Users");
            if (earning.UserId == 0)
            {
                earning = new EarningsViewModel();
                earning.UserId = int.Parse(userId);
                earning.ShiftDate = DateTime.Now;
            }

            earning.AllDrivers = _context.Driver.ToList();

            return View("DriverEarnings", earning);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GetDriverEarnings(EarningsViewModel earningsViewModel)
        {
            var userId = HttpContext.Session.GetString("CurrentUserId");
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Users");


            if (earningsViewModel.UserId == 0)
            {
                earningsViewModel = new EarningsViewModel();
                earningsViewModel.UserId = int.Parse(userId);
                earningsViewModel.ShiftDate = DateTime.Now;
            }

            earningsViewModel.EarningsOn = earningsViewModel.EarningsOn == new DateTime()
                ? DateTime.Now
                : earningsViewModel.EarningsOn;

            earningsViewModel = GetEarningsForDriver(earningsViewModel);
            earningsViewModel.DriverId = earningsViewModel.DriverId;
            earningsViewModel.IsActive =
                _context.OwnerDriver.Any(x => x.DriverId == earningsViewModel.DriverId && x.IsActiveDriver != false);
            earningsViewModel.AllSchedules = _context.Schedule.Include(c => c.Taxi)
                .Where(x => x.DriverId == earningsViewModel.DriverId)
                .ToList();
            earningsViewModel.AllTaxis = earningsViewModel.AllSchedules.Select(x => x.Taxi).ToList();
            earningsViewModel.AllDrivers = _context.Driver.ToList();

            //graph data
            var earnings = _context.Earnings.Where(x => x.DriverId == earningsViewModel.DriverId).ToList();
            var firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            var a = earnings?.Where(x =>
                x.DriverId == earningsViewModel.DriverId && x.ShiftDate >= firstDayOfMonth &&
                x.ShiftDate <= lastDayOfMonth);
            var weeksInMonth = (int) Math.Ceiling(((double) firstDayOfMonth.DayOfWeek + lastDayOfMonth.Day) / 7.0);
            var dataPoints1 = new List<DataPoint>();

            var startdate = firstDayOfMonth;
            for (var i = 1; i <= weeksInMonth; i++)
            {
                dataPoints1.Add(new DataPoint(startdate.ToString("dd MMM yyy"),
                    earnings.Where(x => x.ShiftDate.Date >= startdate && x.ShiftDate <= startdate.AddDays(7))
                        .Sum(x => x.IncomeEarned) ?? 0));
                startdate = startdate.AddDays(7);
            }

            ViewBag.DataPoints1 = JsonConvert.SerializeObject(dataPoints1);

            return View("DriverEarnings", earningsViewModel);
        }

        public EarningsViewModel GetEarningsForDriver(EarningsViewModel earning)
        {
            earning.Earnings = _context.Earnings.Where(x => x.DriverId == earning.DriverId)
                .OrderBy(x => x.ShiftDate).ToList();
            earning.MonthlyEarnings = (double) earning.Earnings.Sum(x => x.IncomeEarned);

            return earning;
        }
    }
}