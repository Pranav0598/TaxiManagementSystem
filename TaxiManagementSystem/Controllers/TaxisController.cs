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
    public class TaxisController : Controller
    {
        private readonly TaxiManagementSystemContext _context;

        public TaxisController(TaxiManagementSystemContext context)
        {
            _context = context;
        }

        // GET: Taxis
        public async Task<IActionResult> Index()
        {
            string userId = HttpContext.Session.GetString("CurrentUserId");
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Users");
            

            IEnumerable<Taxi> allTaxi = _context.Taxi.ToList();
            TaxiViewModel model = new TaxiViewModel();
            model.Taxis = allTaxi;
            model.NewTaxi = new Taxi();
            return View(model);
        }

        // GET: Taxis/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taxi = await _context.Taxi
                .FirstOrDefaultAsync(m => m.TaxiId == id);
            if (taxi == null)
            {
                return NotFound();
            }

            return View(taxi);
        }

        // GET: Taxis/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Taxis/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TaxiViewModel taxiViewModel)
        {
            if (ModelState.IsValid)
            {
                taxiViewModel.NewTaxi.IsWorking = true;

                _context.Add(taxiViewModel.NewTaxi);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            IEnumerable<Taxi> allTaxi = _context.Taxi.ToList();
            taxiViewModel.Taxis = allTaxi;
            taxiViewModel.NewTaxi = taxiViewModel.NewTaxi;
            return RedirectToAction("Index", "Taxis", taxiViewModel); 
        }

        // GET: Taxis/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taxi = await _context.Taxi.FindAsync(id);
            if (taxi == null)
            {
                return NotFound();
            }
            return View(taxi);
        }

        // POST: Taxis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TaxiId,Model,Make,Registration,Comments,IsWorking,RegoExpiry")] Taxi taxi)
        {
            if (id != taxi.TaxiId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taxi);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaxiExists(taxi.TaxiId))
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
            return View(taxi);
        }

        // GET: Taxis/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taxi = await _context.Taxi
                .FirstOrDefaultAsync(m => m.TaxiId == id);
            if (taxi == null)
            {
                return NotFound();
            }

            return View(taxi);
        }

        // POST: Taxis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var taxi = await _context.Taxi.FindAsync(id);
            _context.Taxi.Remove(taxi);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaxiExists(int id)
        {
            return _context.Taxi.Any(e => e.TaxiId == id);
        }
    }
}
