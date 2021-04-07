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
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDriver(OwnerDriversViewModel owner)
        {
            if (owner?.SelectedDriver == null || owner.SelectedDriver == 0)
            {
                return RedirectToAction( "Index", "Drivers");
            }

            string userId = HttpContext.Session.GetString("CurrentUserId");
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Users");
            var currentOwner = _context.Owner
              .FirstOrDefault(m => m.UserId == int.Parse(userId));

            var exists = _context.OwnerDriver.Any(x => x.DriverId == owner.SelectedDriver && x.OwnerId == currentOwner.OwnerId);
            if (exists)
            {
                return RedirectToAction("Index", "Drivers");
            }

            var ownerDriver = new OwnerDriver {
                OwnerId = currentOwner.OwnerId,
                DriverId = owner.SelectedDriver
            };

            _context.OwnerDriver.Add(ownerDriver);
            _context.SaveChanges();

            return RedirectToAction("Index", "Drivers");
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
        public async Task<IActionResult> Create([Bind("OwnerId,LastName,FirstName,Email,Phonenumber")] Owner owner)
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
        public async Task<IActionResult> Edit(int id, [Bind("OwnerId,LastName,FirstName,Email,Phonenumber")] Owner owner)
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
    }
}
