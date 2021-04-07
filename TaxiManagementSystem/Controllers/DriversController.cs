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
    public class DriversController : Controller
    {
        private readonly TaxiManagementSystemContext _context;

        public DriversController(TaxiManagementSystemContext context)
        {
            _context = context;
        }

        // GET: Drivers
        public async Task<IActionResult> Index()
        {
            string userId = HttpContext.Session.GetString("CurrentUserId");
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Users");

            Owner owner = _context.Owner.FirstOrDefault(x=>x.UserId == int.Parse(userId));

            IEnumerable<DriverViewModel> allDrivers = GetAllDrivers();
            IEnumerable<DriverViewModel> drivers = GetDrivers(owner.OwnerId);
            OwnerDriversViewModel model = new OwnerDriversViewModel();
            model.AllDrivers = allDrivers;
            model.CurrentDrivers = drivers;
            model.SelectedDriver = new int();
            return View(model);
        }

        public IEnumerable<DriverViewModel> GetAllDrivers()
        {
            IEnumerable<Driver> drivers = (IEnumerable<Driver>)_context.Driver.ToList();
            IEnumerable<DriverViewModel> modelDrivers = MapDriverToViewModel(drivers);

            return modelDrivers;
        }


        public IEnumerable<DriverViewModel> GetDrivers(int ownerId)
        {
            List<int> driverIds = _context.OwnerDriver.Where(x=>x.OwnerId == ownerId).Select(x=>x.DriverId).ToList(); 
            IEnumerable<Driver> drivers =_context.Driver.Where(x=> driverIds.Any(y=>y == x.DriverId)).ToList();
            IEnumerable<DriverViewModel> modelDrivers = MapDriverToViewModel(drivers);

            return modelDrivers;
        }

        private IEnumerable<DriverViewModel> MapDriverToViewModel(IEnumerable<Driver> drivers) 
        {
            List<DriverViewModel> viewModelDrivers = new List<DriverViewModel>();
            foreach (Driver d in drivers) 
            {
                DriverViewModel driver = new DriverViewModel();
                driver.DriverId = d.DriverId;
                driver.Name = d.FirstName+" "+d.LastName;
                driver.Phonenumber = d.Phonenumber;
                driver.Email = d.Email;
                driver.DriversLicense = d.DriversLicense;
                viewModelDrivers.Add(driver);
            }
            return (IEnumerable<DriverViewModel>)viewModelDrivers;
        }

        // GET: Drivers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driver = await _context.Driver
                .FirstOrDefaultAsync(m => m.DriverId == id);
            if (driver == null)
            {
                return NotFound();
            }

            return View(driver);
        }

        // GET: Drivers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Drivers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DriverId,LastName,FirstName,Age,Email,Phonenumber,DriversLicense")] Driver driver)
        {
            if (ModelState.IsValid)
            {
                _context.Add(driver);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(driver);
        }

        // GET: Drivers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driver = await _context.Driver.FindAsync(id);
            if (driver == null)
            {
                return NotFound();
            }
            return View(driver);
        }

        // POST: Drivers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DriverId,LastName,FirstName,Age,Email,Phonenumber,DriversLicense")] Models.Driver driver)
        {
            if (id != driver.DriverId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(driver);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DriverExists(driver.DriverId))
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
            return View(driver);
        }

        // GET: Drivers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }

            string userId = HttpContext.Session.GetString("CurrentUserId");
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Users");
            var currentOwner = _context.Owner
              .FirstOrDefault(m => m.UserId == int.Parse(userId));

            var ownerDriver = _context.OwnerDriver.FirstOrDefault(x=>x.DriverId == id && x.OwnerId == currentOwner.OwnerId);           
            if (ownerDriver == null)
            {
                return RedirectToAction(nameof(Index));
            }
            _context.OwnerDriver.Remove(ownerDriver);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));           
            }

        // POST: Drivers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var driver = await _context.Driver.FindAsync(id);
            _context.Driver.Remove(driver);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DriverExists(int id)
        {
            return _context.Driver.Any(e => e.DriverId == id);
        }
    }
}
