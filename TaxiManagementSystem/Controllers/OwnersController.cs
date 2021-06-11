using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TaxiManagementSystem.Model;
using TaxiManagementSystem.Models;

namespace TaxiManagementSystem.Controllers
{
    public class OwnersController : Controller
    {
        private readonly TaxiManagementSystemContext _context;

        public OwnersController(TaxiManagementSystemContext context)
        {
            _context = context;
        }

        // GET: Owners
        public async Task<IActionResult> Index()
        {
            OwnerDriversViewModel view = new OwnerDriversViewModel();
            var schedules = _context.Schedule.Include(s => s.Driver).Where(x => ((x.ShiftTime.Date >= DateTime.Now)&& x.ShiftTime.Date <= DateTime.Now.AddDays(1)) ||  (x.ShiftTimeEnd >= DateTime.Now && x.ShiftTimeEnd <= DateTime.Now.AddDays(1)));

                //var drivers = _context.Driver.ToList();
                view.TodaysSchedule = new List<ScheduleViewModel>();
            var listofschedules = new List<ScheduleViewModel>();
            if (schedules != null && schedules.Any())
            {
                foreach (var sched in schedules)
                {
                  //  sched.Driver = drivers.FirstOrDefault(x=>x.DriverId == sched.DriverId);
                    var newSchedule = convertToScheduleViewModel(sched);
                    listofschedules.Add(newSchedule);
                }

                view.TodaysSchedule = listofschedules;
            }

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
            
            return View(view);
        }

        // GET: Owners/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var owner = await _context.Owner
                .FirstOrDefaultAsync(m => m.OwnerId == id);
            if (owner == null)
            {
                return NotFound();
            }

            return View(owner);
        }

        // GET: Owners/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Owners/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OwnerId,LastName,FirstName,Email,PhoneNumber")] Owner owner)
        {
            if (ModelState.IsValid)
            {
                _context.Add(owner);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(owner);
        }

        // GET: Owners/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var owner = await _context.Owner.FindAsync(id);
            if (owner == null)
            {
                return NotFound();
            }
            return View(owner);
        }

        // POST: Owners/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OwnerId,LastName,FirstName,Email,PhoneNumber")] Owner owner)
        {
            if (id != owner.OwnerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(owner);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OwnerExists(owner.OwnerId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(owner);
        }

        // GET: Owners/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var owner = await _context.Owner
                .FirstOrDefaultAsync(m => m.OwnerId == id);
            if (owner == null)
            {
                return NotFound();
            }

            return View(owner);
        }

        // POST: Owners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var owner = await _context.Owner.FindAsync(id);
            _context.Owner.Remove(owner);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OwnerExists(int id)
        {
            return _context.Owner.Any(e => e.OwnerId == id);
        }

        private ScheduleViewModel convertToScheduleViewModel(Schedule schedule)
        {
            ScheduleViewModel scheduleViewModel = new ScheduleViewModel();
            scheduleViewModel.ScheduleId = schedule.ScheduleId;
            scheduleViewModel.TaxiId = schedule.TaxiId;
            scheduleViewModel.DriverId = schedule.DriverId;
            scheduleViewModel.Comments = schedule.Comments;
            scheduleViewModel.ShifTime = schedule.ShiftTime;
            scheduleViewModel.ShifTimeEnd = schedule.ShiftTimeEnd;
            scheduleViewModel.Driver = schedule.Driver;
            return scheduleViewModel;
        }
    }
}
