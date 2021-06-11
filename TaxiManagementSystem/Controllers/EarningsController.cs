using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing.ChartDrawing;
using DocumentFormat.OpenXml.Office.CustomXsn;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
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


        [HttpGet]
        public IActionResult MyEarnings(EarningsViewModel? earning)
        {
            string userId = HttpContext.Session.GetString("CurrentUserId");
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Users");

            var driver = _context.Driver.FirstOrDefault(x => x.UserId == int.Parse(userId));


            if (earning.UserId == 0)
            {
                earning = new EarningsViewModel();
                earning.UserId = Int32.Parse(userId);
                earning.ShiftDate = DateTime.Now;
            }

            earning.EarningsOn = earning.EarningsOn == new DateTime() ? DateTime.Now : earning.EarningsOn;
            earning = GetEarnings(earning);
            earning.DriverId = driver.DriverId;
            earning.IsActive =
                _context.OwnerDriver.Any(x => x.DriverId == driver.DriverId && x.IsActiveDriver != false);
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
                        ShiftDate = earningsViewModel.ShiftDate, EarningsOn = earningsViewModel.EarningsOn,
                        UserId = earningsViewModel.UserId, WeeklyEarnings = earningsViewModel.WeeklyEarnings,
                        MonthlyEarnings = earningsViewModel.MonthlyEarnings, DriverId = earningsViewModel.DriverId
                    });

            Earnings earning = MapViewModelToEarnings(earningsViewModel);
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
                    ShiftDate = earningsViewModel.ShiftDate, EarningsOn = earningsViewModel.EarningsOn,
                    UserId = earning.UserId, WeeklyEarnings = earningsViewModel.WeeklyEarnings,
                    MonthlyEarnings = earningsViewModel.MonthlyEarnings, DriverId = earningsViewModel.DriverId
                });

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
            return RedirectToAction("MyEarnings", "Earnings",
                new
                {
                    ShiftDate = earningsViewModel.ShiftDate, EarningsOn = earning.EarningsOn, UserId = earning.UserId,
                    WeeklyEarnings = earning.WeeklyEarnings, MonthlyEarnings = earning.MonthlyEarnings,
                    DriverId = earningsViewModel.DriverId
                });
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
            earning.DriverId = earningsViewModel.DriverId;
            earning.TaxiId = earningsViewModel.TaxiId;

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
            DateTime monday = earning.EarningsOn.AddDays(-(int) earning.EarningsOn.DayOfWeek + (int) DayOfWeek.Monday);
            DateTime sunday = monday.AddDays(6);
            earning.Earnings = _context.Earnings.Where(e => e.ShiftDate >= monday && e.ShiftDate <= sunday)
                .OrderBy(x => x.ShiftDate);

            earning.WeeklyEarnings = earning.Earnings.Select(x => x.IncomeEarned ?? 0).Sum();
            DateTime firstDayOfMonth = new DateTime(earning.EarningsOn.Year, earning.EarningsOn.Month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            earning.MonthlyEarnings = _context.Earnings
                .Where(e => e.ShiftDate >= firstDayOfMonth && e.ShiftDate <= lastDayOfMonth)
                .Select(x => x.IncomeEarned ?? 0).Sum();
            return earning;
        }



        [HttpGet]
        public IActionResult OwnerEarnings(EarningsViewModel? earning)
        {
            string userId = HttpContext.Session.GetString("CurrentUserId");
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Users");


            if (earning.UserId == 0)
            {
                earning = new EarningsViewModel();
                earning.UserId = Int32.Parse(userId);
                earning.ShiftDate = DateTime.Now;
            }

            earning.Earnings = _context.Earnings.Include(x=>x.Taxi).Include(y=>y.Driver).ToList();

            earning.WeeklyEarnings = earning.Earnings.Select(x => x.IncomeEarned ?? 0).Sum();
            DateTime firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            earning.MonthlyEarnings = _context.Earnings
                    .Where(e => e.ShiftDate >= firstDayOfMonth && e.ShiftDate <= lastDayOfMonth)
                    .Select(x => x.IncomeEarned ?? 0).Sum();

            //graph data
            var earnings = _context.Earnings.ToList();

            var a = earnings?.Where(x => x.ShiftDate.Month == 1).Sum(x => x.IncomeEarned) ?? 0;

            List<DataPoint> dataPoints1 = new List<DataPoint>();

            dataPoints1.Add(new DataPoint("Jan", earnings?.Where(x => x.ShiftDate.Month == 1 && x.ShiftDate.Year == DateTime.Now.Year).Sum(x => x.IncomeEarned) ?? 0));
            dataPoints1.Add(new DataPoint("Feb", earnings?.Where(x => x.ShiftDate.Month == 2 && x.ShiftDate.Year == DateTime.Now.Year).Sum(x => x.IncomeEarned) ?? 0));
            dataPoints1.Add(new DataPoint("Mar", earnings?.Where(x => x.ShiftDate.Month == 3 && x.ShiftDate.Year == DateTime.Now.Year).Sum(x => x.IncomeEarned) ?? 0));
            dataPoints1.Add(new DataPoint("Apr", earnings?.Where(x => x.ShiftDate.Month == 4 && x.ShiftDate.Year == DateTime.Now.Year).Sum(x => x.IncomeEarned) ?? 0));
            dataPoints1.Add(new DataPoint("May", earnings?.Where(x => x.ShiftDate.Month == 5 && x.ShiftDate.Year == DateTime.Now.Year).Sum(x => x.IncomeEarned) ?? 0));
            dataPoints1.Add(new DataPoint("Jun", earnings?.Where(x => x.ShiftDate.Month == 6 && x.ShiftDate.Year == DateTime.Now.Year).Sum(x => x.IncomeEarned) ?? 0));
            dataPoints1.Add(new DataPoint("Jul", earnings?.Where(x => x.ShiftDate.Month == 7 && x.ShiftDate.Year == DateTime.Now.Year).Sum(x => x.IncomeEarned) ?? 0));
            dataPoints1.Add(new DataPoint("Aug", earnings?.Where(x => x.ShiftDate.Month == 8 && x.ShiftDate.Year == DateTime.Now.Year).Sum(x => x.IncomeEarned) ?? 0));
            dataPoints1.Add(new DataPoint("Sep", earnings?.Where(x => x.ShiftDate.Month == 9 && x.ShiftDate.Year == DateTime.Now.Year).Sum(x => x.IncomeEarned) ?? 0));
            dataPoints1.Add(new DataPoint("Oct", earnings?.Where(x => x.ShiftDate.Month == 10 && x.ShiftDate.Year == DateTime.Now.Year).Sum(x => x.IncomeEarned) ?? 0));
            dataPoints1.Add(new DataPoint("Nov", earnings?.Where(x => x.ShiftDate.Month == 11 && x.ShiftDate.Year == DateTime.Now.Year).Sum(x => x.IncomeEarned) ?? 0));
            dataPoints1.Add(new DataPoint("Dec", earnings?.Where(x => x.ShiftDate.Month == 12 && x.ShiftDate.Year == DateTime.Now.Year).Sum(x => x.IncomeEarned) ?? 0));


            ViewBag.DataPoints1 = JsonConvert.SerializeObject(dataPoints1);
            return View("OwnerEarnings",earning);
        }

        public EarningsViewModel Search(EarningsViewModel earning)
        {
            earning.Earnings = _context.Earnings.Include(x => x.Taxi).Include(x => x.Driver).Where(e =>
                e.Taxi.Registration.Contains(earning.Search) ||
                (e.Driver.FirstName + " " + e.Driver.LastName).Contains(earning.Search)).OrderBy(x => x.ShiftDate).ToList();
            earning.WeeklyEarnings = 0;
            earning.MonthlyEarnings = 0;
            

            earning.Earnings = earning?.Earnings ?? new List<Earnings>();

            return earning;
        }

        public EarningsViewModel GetOwnerEarningsForDate(EarningsViewModel earning)
        {
            DateTime monday = earning.EarningsOn.AddDays(-(int)earning.EarningsOn.DayOfWeek + (int)DayOfWeek.Monday);
            DateTime sunday = monday.AddDays(6);
            earning.Earnings = _context.Earnings.Where(e => e.ShiftDate >= monday && e.ShiftDate <= sunday)
                .OrderBy(x => x.ShiftDate).ToList();

            earning.WeeklyEarnings = earning.Earnings.Select(x => x.IncomeEarned ?? 0).Sum();
            DateTime firstDayOfMonth = new DateTime(earning.EarningsOn.Year, earning.EarningsOn.Month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
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
            string userId = HttpContext.Session.GetString("CurrentUserId");
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Users");



            EarningsViewModel earning = new EarningsViewModel();
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
                ears = (List<Earnings>)(earning.Earnings.Any()
                   ? (searchResult.Earnings.Any() ? earning?.Earnings.Concat(searchResult.Earnings) : earning?.Earnings)
                   : searchResult.Earnings);
                earning.Earnings = ears;
            }
            else
            {
                earning.Earnings = _context.Earnings.ToList();
            }

            if (earning?.Earning == null || (!earning?.Earnings?.Any() ?? true) )
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
            earning.UserId = Int32.Parse(userId);

            //graph data
            var earnings = _context.Earnings.ToList();

            var a = earnings?.Where(x => x.ShiftDate.Month == 1).Sum(x => x.IncomeEarned) ?? 0;

            List<DataPoint> dataPoints1 = new List<DataPoint>();

            dataPoints1.Add(new DataPoint("Jan", earnings?.Where(x => x.ShiftDate.Month == 1 && x.ShiftDate.Year == DateTime.Now.Year).Sum(x => x.IncomeEarned) ?? 0));
            dataPoints1.Add(new DataPoint("Feb", earnings?.Where(x => x.ShiftDate.Month == 2 && x.ShiftDate.Year == DateTime.Now.Year).Sum(x => x.IncomeEarned) ?? 0));
            dataPoints1.Add(new DataPoint("Mar", earnings?.Where(x => x.ShiftDate.Month == 3 && x.ShiftDate.Year == DateTime.Now.Year).Sum(x => x.IncomeEarned) ?? 0));
            dataPoints1.Add(new DataPoint("Apr", earnings?.Where(x => x.ShiftDate.Month == 4 && x.ShiftDate.Year == DateTime.Now.Year).Sum(x => x.IncomeEarned) ?? 0));
            dataPoints1.Add(new DataPoint("May", earnings?.Where(x => x.ShiftDate.Month == 5 && x.ShiftDate.Year == DateTime.Now.Year).Sum(x => x.IncomeEarned) ?? 0));
            dataPoints1.Add(new DataPoint("Jun", earnings?.Where(x => x.ShiftDate.Month == 6 && x.ShiftDate.Year == DateTime.Now.Year).Sum(x => x.IncomeEarned) ?? 0));
            dataPoints1.Add(new DataPoint("Jul", earnings?.Where(x => x.ShiftDate.Month == 7 && x.ShiftDate.Year == DateTime.Now.Year).Sum(x => x.IncomeEarned) ?? 0));
            dataPoints1.Add(new DataPoint("Aug", earnings?.Where(x => x.ShiftDate.Month == 8 && x.ShiftDate.Year == DateTime.Now.Year).Sum(x => x.IncomeEarned) ?? 0));
            dataPoints1.Add(new DataPoint("Sep", earnings?.Where(x => x.ShiftDate.Month == 9 && x.ShiftDate.Year == DateTime.Now.Year).Sum(x => x.IncomeEarned) ?? 0));
            dataPoints1.Add(new DataPoint("Oct", earnings?.Where(x => x.ShiftDate.Month == 10 && x.ShiftDate.Year == DateTime.Now.Year).Sum(x => x.IncomeEarned) ?? 0));
            dataPoints1.Add(new DataPoint("Nov", earnings?.Where(x => x.ShiftDate.Month == 11 && x.ShiftDate.Year == DateTime.Now.Year).Sum(x => x.IncomeEarned) ?? 0));
            dataPoints1.Add(new DataPoint("Dec", earnings?.Where(x => x.ShiftDate.Month == 12 && x.ShiftDate.Year == DateTime.Now.Year).Sum(x => x.IncomeEarned) ?? 0));


            ViewBag.DataPoints1 = JsonConvert.SerializeObject(dataPoints1);

            return View("OwnerEarnings", earningsViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Export(EarningsViewModel earningsViewModel)
        {
            var listofIds = earningsViewModel?.Earnings?.Select(x=>x.EarningId).ToList() ?? new List<int>();
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
                {
                    if (e?.Driver != null && e?.Taxi != null)
                    {
                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = e.Driver.FirstName + " " + e.Driver.LastName;
                        worksheet.Cell(currentRow, 2).Value = e.ShiftDate;
                        worksheet.Cell(currentRow, 3).Value = e.Taxi.Registration;
                        worksheet.Cell(currentRow, 4).Value = "$ " + e.IncomeEarned;
                    }
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
            string userId = HttpContext.Session.GetString("CurrentUserId");
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Users");


            if (earning.UserId == 0)
            {
                earning = new EarningsViewModel();
                earning.UserId = Int32.Parse(userId);
                earning.ShiftDate = DateTime.Now;
            }

            earning.AllDrivers = _context.Driver.ToList();
            
            return View("DriverEarnings", earning);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GetDriverEarnings(EarningsViewModel earningsViewModel)
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

            earningsViewModel.EarningsOn = earningsViewModel.EarningsOn == new DateTime() ? DateTime.Now : earningsViewModel.EarningsOn;

            earningsViewModel = GetEarningsForDriver(earningsViewModel);
            earningsViewModel.DriverId = earningsViewModel.DriverId;
            earningsViewModel.IsActive =
                _context.OwnerDriver.Any(x => x.DriverId == earningsViewModel.DriverId && x.IsActiveDriver != false);
            earningsViewModel.AllSchedules = _context.Schedule.Include(c => c.Taxi).Where(x => x.DriverId == earningsViewModel.DriverId)
                .ToList();
            earningsViewModel.AllTaxis = earningsViewModel.AllSchedules.Select(x => x.Taxi).ToList();
            earningsViewModel.AllDrivers = _context.Driver.ToList();

            //graph data
            var earnings = _context.Earnings.Where(x => x.DriverId == earningsViewModel.DriverId).ToList();
            DateTime firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            var a = earnings?.Where(x => x.DriverId == earningsViewModel.DriverId && x.ShiftDate >= firstDayOfMonth && x.ShiftDate <= lastDayOfMonth);
            int weeksInMonth = (int)Math.Ceiling(((double)firstDayOfMonth.DayOfWeek + lastDayOfMonth.Day) / 7.0);
            List<DataPoint> dataPoints1 = new List<DataPoint>();

            var startdate = firstDayOfMonth;
            for (int i = 1; i <= weeksInMonth; i++)
            {
                dataPoints1.Add(new DataPoint(startdate.ToString("dd MMM yyy"), earnings.Where(x => x.ShiftDate.Date >= startdate&& x.ShiftDate <= startdate.AddDays(7)).Sum(x => x.IncomeEarned) ?? 0));
                startdate = startdate.AddDays(7);
            }

            ViewBag.DataPoints1 = JsonConvert.SerializeObject(dataPoints1);

            return View("DriverEarnings", earningsViewModel);
        }

        public EarningsViewModel GetEarningsForDriver(EarningsViewModel earning)
        {
            earning.Earnings = _context.Earnings.Where(x=>x.DriverId == earning.DriverId)
                .OrderBy(x => x.ShiftDate);
            earning.MonthlyEarnings = (double)earning.Earnings.Sum(x =>x.IncomeEarned);

            return earning;
        }
    }
}
