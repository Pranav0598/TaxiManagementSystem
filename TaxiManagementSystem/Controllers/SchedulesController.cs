using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaxiManagementSystem.Model;
using TaxiManagementSystem.Models;

namespace TaxiManagementSystem.Controllers
{
    public class SchedulesController : Controller
    {
        private readonly TaxiManagementSystemContext _context;

        public SchedulesController(TaxiManagementSystemContext context)
        {
            _context = context;
        }

        // GET: Schedules
        public async Task<IActionResult> Index()
        {
            ScheduleViewModel viewModel = new ScheduleViewModel();
            viewModel = Load(viewModel);
            viewModel.ShifTime = DateTime.Now;
            viewModel.ShifTimeEnd = viewModel.ShifTime.AddHours(8);
            return View(viewModel);
        }

        private ScheduleViewModel Load(ScheduleViewModel viewModel)
        {
            viewModel.AllSchedules = _context.Schedule.Include(s => s.Driver).Include(s => s.Taxi).ToList();
            viewModel.AllDrivers = _context.Driver.ToList();
            viewModel.AllTaxis = _context.Taxi.Where(x=>x.IsWorking == true).ToList();
            viewModel.Comments = null;
            viewModel.Events = mapScheduleToEvents(viewModel.AllSchedules);
            return viewModel;
        }

        private List<Event> mapScheduleToEvents( List<Schedule> schdules)
        {
            List<Event> events = new List<Event>();
            if (schdules != null && schdules.Any())
            {
                foreach (var s in schdules)
                {
                    Event e = new Event();
                    e.id = s.ScheduleId;
                    e.title = s?.Driver?.FirstName + " " + s?.Driver?.LastName;
                    e.start = s.ShiftTime;
                    e.end = s.ShiftTimeEnd;
                    events.Add(e);
                }
            }

            return events;
        }

        public async Task<IActionResult> DriverSchedule()
        {
            string userId = HttpContext.Session.GetString("CurrentUserId");
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Users");

            var driver = _context.Driver.FirstOrDefault(x => x.UserId == Int32.Parse(userId));

            ScheduleViewModel viewModel = new ScheduleViewModel();
            viewModel.AllSchedules = _context.Schedule.Include(s => s.Driver).Include(s => s.Taxi).Where(x => x.DriverId == driver.DriverId).ToList();
                       
            viewModel.Events = mapScheduleToEvents(viewModel.AllSchedules);
            viewModel.ShifTime = DateTime.Now;
            viewModel.ShifTimeEnd = viewModel.ShifTime.AddHours(8);
            return View(viewModel);
        }

        
        // POST: Schedules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddShift(ScheduleViewModel scheduleViewModel)
        {
            if (scheduleViewModel.ScheduleId != 0)
            {//Edit
                if (ModelState.IsValid)
                {
                    Schedule schedule = ConvertViewModeltoSchedule(scheduleViewModel);
                    scheduleViewModel.DisplayEdit = false;
                    try
                    {
                        _context.Update(schedule);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        scheduleViewModel.ScheduleId = 0;
                        if (!ScheduleExists(schedule.ScheduleId))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            }
            else
            {//Add
                if (ModelState.IsValid)
                {
                    Schedule schedule = ConvertViewModeltoSchedule(scheduleViewModel);
                    _context.Add(schedule);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    scheduleViewModel.Error = "Schedule could not be added";
                }
            }

            scheduleViewModel = Load(scheduleViewModel);

            return View("Index", scheduleViewModel);
        }

        private Schedule ConvertViewModeltoSchedule(ScheduleViewModel scheduleViewModel)
        {
            Schedule schedule = new Schedule();
            schedule.ScheduleId = scheduleViewModel.ScheduleId;
            schedule.TaxiId = scheduleViewModel.TaxiId;
            schedule.DriverId = scheduleViewModel.DriverId;
            schedule.Comments = scheduleViewModel.Comments;
            schedule.ShiftTime = scheduleViewModel.ShifTime;
            schedule.ShiftTimeEnd = scheduleViewModel.ShifTimeEnd;
            return schedule;
        }

        private Schedule ConvertEditViewModeltoSchedule(ScheduleViewModel scheduleViewModel)
        {
            Schedule schedule = new Schedule();
            schedule.ScheduleId = scheduleViewModel.EditScheduleId;
            schedule.TaxiId = scheduleViewModel.EditTaxiId;
            schedule.DriverId = scheduleViewModel.EditDriverId;
            schedule.Comments = scheduleViewModel.EditComments;
            schedule.ShiftTime = scheduleViewModel.EditShifTime;
            schedule.ShiftTimeEnd = scheduleViewModel.EditShifTimeEnd;
            return schedule;
        }

        // GET: Schedules/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedule = _context.Schedule.Find(id);
            if (schedule == null)
            {
                return NotFound();
            }
            ScheduleViewModel viewModel = new ScheduleViewModel();
            viewModel.AllSchedules = _context.Schedule.Include(s => s.Driver).Include(s => s.Taxi).ToList();
            viewModel.AllDrivers = _context.Driver.ToList();
            viewModel.AllTaxis = _context.Taxi.Where(x=>x.IsWorking==true).ToList();
            viewModel = convertToScheduleViewModel(schedule, viewModel);
            viewModel.DisplayEdit = true;
            return Ok(viewModel); 
        }
        
        private ScheduleViewModel convertToScheduleViewModel(Schedule schedule, ScheduleViewModel scheduleViewModel)
        {
            scheduleViewModel.ScheduleId = schedule.ScheduleId;
            scheduleViewModel.TaxiId = schedule.TaxiId;
            scheduleViewModel.DriverId = schedule.DriverId;
            scheduleViewModel.Comments = schedule.Comments;
            scheduleViewModel.ShifTime = schedule.ShiftTime;
            scheduleViewModel.ShifTimeEnd = schedule.ShiftTimeEnd;
            return scheduleViewModel;
        }

        // POST: Schedules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ScheduleViewModel scheduleViewModel)
        {
            if (ModelState.IsValid)
            {
                Schedule schedule = ConvertEditViewModeltoSchedule(scheduleViewModel);
                scheduleViewModel.DisplayEdit = false;
                try
                {
                    _context.Update(schedule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    scheduleViewModel.ScheduleId = 0;
                    if (!ScheduleExists(schedule.ScheduleId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return RedirectToAction("Index", "Schedules", scheduleViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteShift(ScheduleViewModel scheduleViewModel)
        {
            if (scheduleViewModel.DeleteScheduleId == 0)
            {
                scheduleViewModel.Error = "Internal error! cannot delete this shift.";
                return RedirectToAction("Index", "Schedules", scheduleViewModel);
            }
            var schedule = await _context.Schedule.FindAsync(scheduleViewModel.DeleteScheduleId);
            _context.Schedule.Remove(schedule);
            await _context.SaveChangesAsync();

            ScheduleViewModel viewModel = new ScheduleViewModel();
            viewModel = Load(viewModel);
            viewModel.ShifTime = DateTime.Now;
            viewModel.ShifTimeEnd = viewModel.ShifTime.AddHours(8);

            return RedirectToAction("Index", "Schedules", viewModel);
        }
        
        private bool ScheduleExists(int id)
        {
            return _context.Schedule.Any(e => e.ScheduleId == id);
        }
    }
}
