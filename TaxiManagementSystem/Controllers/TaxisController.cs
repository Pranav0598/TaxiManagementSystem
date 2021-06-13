using System;
using System.Collections;
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

            AddTaxiViewModel model = new AddTaxiViewModel();
            IEnumerable<Taxi> allTaxi = (from t in _context.Taxi
                join make in _context.CarMake on t.Make equals make.MakeId
                join mod in _context.CarModel on t.Model equals mod.ModelId
                select new Taxi()
                {
                    TaxiId = t.TaxiId,
                    ModelNavigation = mod,
                    MakeNavigation = make,
                    Registration = t.Registration,
                    Comments = t.Comments,
                    IsWorking = t.IsWorking
                }).ToList();
            var taxis = new List<TaxiViewModel>();

            foreach (var taxi in allTaxi)
            {
                var taxiViewModel = MapToTaxiViewModel(taxi);
                taxis.Add( taxiViewModel );
            }

            model.Taxis = taxis;
            model.NewTaxi = new TaxiViewModel();
            model.SearchModel = new SearchViewModel();
            model.Makes = _context.CarMake.ToList();
            model.Models = _context.CarModel.ToList();
            if (model.DisplayEdit)
            {
                model.EditTaxi = model.EditTaxi;
            }
            
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
        public async Task<IActionResult> Create(AddTaxiViewModel taxiViewModel)
        {
            if (ModelState.IsValid)
            {
                taxiViewModel.NewTaxi.IsWorking = true;

                var taxi = MapTaxiViewModelToTaxi(taxiViewModel.NewTaxi, new Taxi());
                var currentTaxi = _context.Taxi.FirstOrDefault(x=>x.Registration == taxiViewModel.NewTaxi.Registration);
                taxiViewModel.DisplayEdit = false;
                taxiViewModel.SearchModel = new SearchViewModel();
                IEnumerable<Taxi> allTaxi = (from t in _context.Taxi
                    join make in _context.CarMake on t.Make equals make.MakeId
                    join mod in _context.CarModel on t.Model equals mod.ModelId
                    select new Taxi()
                    {
                        TaxiId = t.TaxiId,
                        ModelNavigation = mod,
                        MakeNavigation = make,
                        Registration = t.Registration,
                        Comments = t.Comments,
                        IsWorking = t.IsWorking
                    }).ToList();
                var taxis = new List<TaxiViewModel>();
                taxiViewModel.NewTaxi = new TaxiViewModel();
                taxiViewModel.SearchModel = new SearchViewModel();
                taxiViewModel.Makes = _context.CarMake.ToList();
                taxiViewModel.Models = _context.CarModel.ToList();

                foreach (var taxi1 in allTaxi)
                {
                    var viewModel = MapToTaxiViewModel(taxi1);
                    taxis.Add(viewModel);
                }

                taxiViewModel.Taxis = taxis;
                if (currentTaxi != null)
                {
                    taxiViewModel.NewTaxi = new TaxiViewModel();
                    taxiViewModel.Error = "The taxi with registration" + currentTaxi.Registration + " already exists.";
                  
                  
                    return View("Index",  taxiViewModel);
                }

                _context.Add(taxi);
                await _context.SaveChangesAsync();
                taxiViewModel.NewTaxi = new TaxiViewModel();
                return RedirectToAction(nameof(Index));
            }
           
            return RedirectToAction("Index", "Taxis", taxiViewModel); 
        }

        // GET: Taxis/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }



            var edittaxi = await _context.Taxi.FindAsync(id);
            if (edittaxi == null)
            {
                return NotFound();
            }

            AddTaxiViewModel taxiViewModel = new AddTaxiViewModel();
            IEnumerable<Taxi> allTaxi = _context.Taxi.Include(x => x.MakeNavigation).Include(y => y.ModelNavigation).ToList();
            taxiViewModel.EditTaxi = MapToTaxiViewModel(edittaxi);
            var allTaxis = new List<TaxiViewModel>();

            foreach (var taxi in allTaxi)
            {
                var viewModel = MapToTaxiViewModel(taxi);
                allTaxis.Add(viewModel);
            }

            taxiViewModel.Taxis = allTaxis;
            taxiViewModel.DisplayEdit = true;
            taxiViewModel.SearchModel = new SearchViewModel();
            taxiViewModel.Makes = _context.CarMake.ToList();
            taxiViewModel.Models = _context.CarModel.ToList();
            taxiViewModel.NewTaxi = new TaxiViewModel();
            return View("Index", taxiViewModel);
        }

        // POST: Taxis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AddTaxiViewModel taxiViewModel)
        {
            if (id != taxiViewModel.EditTaxi.TaxiId)
            {
                return NotFound();
            }

            IEnumerable<Taxi> allTaxi = _context.Taxi.Include(x => x.MakeNavigation).Include(y => y.ModelNavigation).ToList();
            
            var allTaxis = new List<TaxiViewModel>();

            foreach (var taxi in allTaxi)
            {
                var viewModel = MapToTaxiViewModel(taxi);
                allTaxis.Add(viewModel);
            }

            taxiViewModel.Taxis = allTaxis;
            taxiViewModel.DisplayEdit = false;
            taxiViewModel.SearchModel = new SearchViewModel();
            taxiViewModel.Makes = _context.CarMake.ToList();
            taxiViewModel.Models = _context.CarModel.ToList();
            taxiViewModel.NewTaxi = new TaxiViewModel();

            if (ModelState.IsValid)
            {
                try
                {
                    var currentTaxi = _context.Taxi.FirstOrDefault(x=>x.TaxiId == taxiViewModel.EditTaxi.TaxiId);

                    var taxi = MapTaxiViewModelToTaxi(taxiViewModel.EditTaxi, currentTaxi);
                    var regoExists = allTaxis.Any(x=>x.Registration == taxiViewModel.EditTaxi.Registration);
                    if (regoExists && currentTaxi.Registration != taxiViewModel.EditTaxi.Registration)
                    {
                        taxiViewModel.EditTaxi = new TaxiViewModel();
                        taxiViewModel.Error = "The taxi with registration" + currentTaxi.Registration + " already exists.";
                        return View("Index", taxiViewModel);
                    }

                    _context.Update(taxi);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaxiExists(taxiViewModel.EditTaxi.TaxiId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                taxiViewModel.EditTaxi = new TaxiViewModel();

                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("Index", "Taxis", taxiViewModel);
        }

        // GET: Taxis/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var taxi = await _context.Taxi.FindAsync(id);
            _context.Taxi.Remove(taxi);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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

        private TaxiViewModel MapToTaxiViewModel(Taxi taxi)
        {
            TaxiViewModel viewModel = new TaxiViewModel();
            viewModel.TaxiId = taxi.TaxiId;
            viewModel.ModelId = taxi.Model;
            viewModel.MakeId = taxi.Make;
            viewModel.Registration = taxi.Registration;
            viewModel.Comments = taxi.Comments;
            viewModel.IsWorking = taxi.IsWorking??false;
            viewModel.RegoExpiry = taxi.RegoExpiry ?? DateTime.Now;
            viewModel.Model = new CarModel()
                {ModelId = taxi.ModelNavigation?.ModelId??0, Model = taxi.ModelNavigation?.Model};
            viewModel.Make = new CarMake()
                {MakeId = taxi.MakeNavigation?.MakeId??0, Make = taxi.MakeNavigation?.Make};
            return viewModel;
        }


        private Taxi MapTaxiViewModelToTaxi(TaxiViewModel taxi ,Taxi model)
        {
            model.Model = taxi.ModelId;
            model.Make = taxi.MakeId;
            model.Registration = taxi.Registration;
            model.Comments = taxi.Comments;
            model.IsWorking = taxi.IsWorking;
            model.RegoExpiry = taxi.RegoExpiry;
            return model;
        }


        // POST: Drivers/Delete/5
        [HttpPost, ActionName("Search")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Search(AddTaxiViewModel addTaxiView)
        {
            string userId = HttpContext.Session.GetString("CurrentUserId");
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Users");



            var keyword = addTaxiView.SearchModel?.keyword;

            IEnumerable<Taxi> allTaxi = _context.Taxi.Include(x=> x.MakeNavigation).Include(x=>x.ModelNavigation).ToList();
            var taxis = new List<TaxiViewModel>();


            addTaxiView.Taxis = taxis;
            addTaxiView.NewTaxi = new TaxiViewModel();
            addTaxiView.EditTaxi = new TaxiViewModel();
            
            if (!string.IsNullOrEmpty(keyword))
            {
                allTaxi = _context.Taxi.Include(x=>x.MakeNavigation).Include(y=>y.ModelNavigation).ToList();
                allTaxi = allTaxi.Where(x => (x.MakeNavigation.Make+ " "+ x.ModelNavigation.Model+" "+x.Registration).Contains(keyword)).ToList();
            }

            foreach (var taxi in allTaxi)
            {
                var taxiViewModel = MapToTaxiViewModel(taxi);
                taxis.Add(taxiViewModel);
            }
            addTaxiView.Makes = _context.CarMake.ToList();
            addTaxiView.Models = _context.CarModel.ToList();

            return View("Index", addTaxiView);
        }

    }
}
