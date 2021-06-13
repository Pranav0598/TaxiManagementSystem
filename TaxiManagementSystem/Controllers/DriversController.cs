using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
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
    public class DriversController : Controller
    {
        private readonly TaxiManagementSystemContext _context;
        private SmtpClient smtpClient;
        private readonly IConfiguration _config;

        public DriversController(TaxiManagementSystemContext context, SmtpClient smtpClient, IConfiguration config)
        {
            _context = context;
            this.smtpClient = smtpClient;
            _config = config;
        }

        // GET: Drivers
        public async Task<IActionResult> Index()
        {
            string userId = HttpContext.Session.GetString("CurrentUserId");
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Users");
            var model = Load(userId);
            return View(model);
        }

        private OwnerDriversViewModel Load(string userId)
        {
            Owner owner = _context.Owner.FirstOrDefault(x => x.UserId == int.Parse(userId));

            IEnumerable<DriverViewModel> allDrivers = GetAllDrivers();
            IEnumerable<DriverViewModel> drivers = GetDrivers(owner.OwnerId);
          
            OwnerDriversViewModel model = new OwnerDriversViewModel();
            model.AllDrivers = allDrivers;
            model.CurrentDrivers = drivers;
            model.EditDriver = new DriverViewModel();
            model.searchModel = new SearchViewModel();
            return model;
        }

        public IEnumerable<DriverViewModel> GetAllDrivers()
        {
            IEnumerable<Driver> drivers = (IEnumerable<Driver>)_context.Driver.ToList();
            IEnumerable<DriverViewModel> modelDrivers = MapDriverToViewModels(drivers);

            return modelDrivers;
        }


        public IEnumerable<DriverViewModel> GetDrivers(int ownerId)
        {
            List<int> driverIds = _context.OwnerDriver.Where(x=>x.OwnerId == ownerId).Select(x=>x.DriverId).ToList(); 
            IEnumerable<Driver> drivers =_context.Driver.Where(x=> driverIds.Any(y=>y == x.DriverId)).ToList();
            IEnumerable<DriverViewModel> modelDrivers = MapDriverToViewModels(drivers);

            return modelDrivers;
        }

        private IEnumerable<DriverViewModel> MapDriverToViewModels(IEnumerable<Driver> drivers) 
        {
            List<DriverViewModel> viewModelDrivers = new List<DriverViewModel>();
            foreach (Driver d in drivers) 
            {
                DriverViewModel driver = new DriverViewModel();
                driver.DriverId = d.DriverId;
                driver.Name = d.FirstName+" "+d.LastName;
                driver.PhoneNumber = d.Phonenumber;
                driver.Email = d.Email;
                driver.DriversLicense = d.DriversLicense;
                driver.IsActive = d.IsActive ?? false;
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
        public async Task<IActionResult> Create([Bind("DriverId,LastName,FirstName,Age,Email,PhoneNumber,DriversLicense")] Driver driver)
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
            string userId = HttpContext.Session.GetString("CurrentUserId");
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Users");

            var model = Load(userId);
            if (id == null)
            {
                model.Error = "Cannot edit the driver's profile. Please try again!";
                return View("Index", model);
            }

            Driver driver = await _context.Driver.FindAsync(id);

            if (driver == null)
            {
                return NotFound();
            }
            
            Owner owner = _context.Owner.FirstOrDefault(x => x.UserId == int.Parse(userId));
            
            IEnumerable<DriverViewModel> allDrivers = GetAllDrivers();
            IEnumerable<DriverViewModel> drivers = GetDrivers(owner.OwnerId);
            foreach (var d in allDrivers)
            {
                d.IsActive = drivers.FirstOrDefault(x => d.DriverId == x.DriverId)?.IsActive ?? false;
            }

            model.CurrentDrivers = drivers;
            model.EditDriver = MapDriverToViewModel(driver);
            model.searchModel = new SearchViewModel();
            model.DisplayEdit = true;
            return View("Index", model);
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
            if (ownerDriver != null)
            {
                _context.OwnerDriver.Remove(ownerDriver);
            }

            var driver = _context.Driver.FirstOrDefault(x=>x.DriverId == id);
            var user = _context.Users.FirstOrDefault(x => x.UserId == driver.UserId);
            var earnings = _context.Earnings.Where(x => x.DriverId == id);
            var schedule = _context.Schedule.Where(x => x.DriverId == id);

            _context.Earnings.RemoveRange(earnings);
            _context.Schedule.RemoveRange(schedule);
            _context.Driver.Remove(driver);
            _context.Users.Remove(user);

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



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveDriver(OwnerDriversViewModel owner)
        {
            if (owner?.EditDriver?.DriverId == 0)
            {
                return RedirectToAction("Index", "Drivers");
            }

            Driver currentDriver = _context.Driver.Include(x => x.User).FirstOrDefault(x => x.DriverId == owner.EditDriver.DriverId);

            string userId = HttpContext.Session.GetString("CurrentUserId");
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Users");


            if (owner.EditDriver.Email != currentDriver.Email)
            {
                var emailExists = _context.Users.Any(x => x.Email == owner.EditDriver.Email);
                if (emailExists)
                {
                    owner.Error = "Driver's email cannot be updated";
                    return View("Index", owner);
                }
            }
            
            var currentOwner = _context.Owner.FirstOrDefault(m => m.UserId == int.Parse(userId));
            var existsInOD = _context.OwnerDriver.Any(x => x.DriverId == owner.EditDriver.DriverId && x.OwnerId == currentOwner.OwnerId);
            
            Driver upadteDriver = MapViewModelToDriver(owner.EditDriver, currentDriver);
            upadteDriver.User.Email = currentDriver.Email;

            _context.Update(upadteDriver);
            _context.SaveChanges();
            
            return RedirectToAction("Index", "Drivers");
        }


        public Driver MapViewModelToDriver(DriverViewModel dv, Driver d)
        {
            d.FirstName = dv.FirstName;
            d.LastName = dv.LastName;
            d.Age = dv.Age;
            d.DriversLicense = dv.DriversLicense;
            d.DriverId = dv.DriverId;
            d.Email = dv.Email;
            d.Phonenumber = dv.PhoneNumber;
            d.IsActive = dv.IsActive;
            return d;

        }

        private DriverViewModel MapDriverToViewModel(Driver d)
        {
                DriverViewModel driver = new DriverViewModel();
                driver.DriverId = d.DriverId;
                driver.Name = d.FirstName + " " + d.LastName;
                driver.FirstName = d.FirstName;
                driver.LastName = d.LastName;
                driver.Age = d.Age;
                driver.PhoneNumber = d.Phonenumber;
                driver.Email = d.Email;
                driver.DriversLicense = d.DriversLicense;
                driver.IsActive = d.IsActive ?? false;
            return driver;
        }

        // POST: Drivers/Delete/5
        [HttpPost, ActionName("Search")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Search(OwnerDriversViewModel driversViewModel)
        {
            string userId = HttpContext.Session.GetString("CurrentUserId");
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Users");

            

            var keyword = driversViewModel.searchModel?.keyword;

            var drivers = GetAllDrivers();

            if (!string.IsNullOrEmpty(keyword))
            {
                var ogdrivers = _context.Driver.Where(x => (x.FirstName + " " + x.LastName).Contains(keyword)).ToList();
                drivers = MapDriverToViewModels(ogdrivers);
            }
            
            foreach (var d in drivers)
            {
                d.IsActive = drivers.FirstOrDefault(x => d.DriverId == x.DriverId)?.IsActive ?? false;
            }

            driversViewModel.AllDrivers = drivers;

            await _context.SaveChangesAsync();
            return View("Index", driversViewModel);
        }

        // GET: Drivers/ResetDriverPwd/5
        public async Task<IActionResult> ResetDriverPwd(int? driverId)
        {
            string userId = HttpContext.Session.GetString("CurrentUserId");
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Users");
            var model = Load(userId);
            if (driverId == 0 || driverId == null)
            {
                model.Error = "Cannot reset the driver's pwd. Please try again!";
                return View("Index", model);
            }

            var driver = _context.Driver.Include(x => x.User).FirstOrDefault( x=> x.DriverId == driverId);

            if (driver == null || driver.DriverId == 0 || driver?.User == null || driver?.User?.UserName == null)
            {
                model.Error = "Cannot reset the driver's pwd. Please try again!";
                return View("Index", model);
            }
            
            string pwd = GeneratePwd();
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("pranavsai0507@gmail.com");
                mail.To.Add(driver.User.Email);
                mail.Subject = "Password reset - Fateh Taxi Management System";
                mail.Body = "<h1>Fateh Management System - Reset Password</h1><p>Hi " + driver.FirstName + ",</p><p>Your new password is <b>" + pwd + "</b></p><p>Please login using this password and reset on login.</p>";
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient(_config.GetValue<string>("Email:Smtp:Host"), Convert.ToInt32(_config.GetValue<string>("Email:Smtp:Port"))))
                {
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = new NetworkCredential(_config.GetValue<string>("Email:Smtp:Username"), _config.GetValue<string>("Email:Smtp:Password"));
                    smtp.EnableSsl = true;
                   smtp.Send(mail);
                }
                driver.User.IsPasswordReset = true;
                driver.User.Password = Encryption.Encrypt(pwd);
                _context.Update(driver.User);
                await _context.SaveChangesAsync();
            }

            model.Status = "An email with a new password has been sent to "+ driver.FirstName+" "+driver.LastName+".";

            return View("Index", model);
        }

        private string GeneratePwd()
        {

            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < 8; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            return builder.ToString().ToUpper();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDriver(OwnerDriversViewModel owner)
        {
           
            
            string userId = HttpContext.Session.GetString("CurrentUserId");
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Users");
            IEnumerable<DriverViewModel> allDrivers = GetAllDrivers();
            IEnumerable<DriverViewModel> drivers = GetAllDrivers();
            foreach (var d in allDrivers)
            {
                d.IsActive = drivers.FirstOrDefault(x => d.DriverId == x.DriverId)?.IsActive ?? false;
            }

            owner.CurrentDrivers = drivers;
            owner.searchModel = new SearchViewModel();


            var emailExists = _context.Driver.Any(x => x.Email == owner.AddDriver.Email);
            var usernameExists = _context.Driver.Include(x=>x.User).Any(x=>x.User.UserName == owner.AddDriver.UserName);

            if (emailExists)
                {
                    owner.Error = "Email already exists. Driver's email cannot be created";
                    return View("Index", owner);
                }

            if (usernameExists)
            {
                owner.Error = "Username already exisits. Driver's email cannot be created";
                return View("Index", owner);
            }

            Users user = new Users();
            user.UserName = owner.AddDriver.UserName;
            user.FirstName = owner.AddDriver.FirstName;
            user.LastName = owner.AddDriver.LastName;
            user.Email = owner.AddDriver.Email;
            user.Age = owner.AddDriver.Age;
            var tempPassword = GeneratePwd();
            user.Password = Encryption.Encrypt(tempPassword);
            _context.Users.Add(user);
            _context.SaveChanges();

            Driver driver = new Driver();
            driver.FirstName = owner.AddDriver.FirstName;
            driver.LastName = owner.AddDriver.LastName;
            driver.Email = owner.AddDriver.Email;
            driver.Age = owner.AddDriver.Age;
            driver.DriversLicense = owner.AddDriver.DriversLicense;
            driver.IsActive = true;
            driver.UserId = user.UserId;
            _context.Driver.Add(driver);
            _context.SaveChanges();

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("pranavsai0507@gmail.com");
                mail.To.Add(driver.User.Email);
                mail.Subject = "New driver - Fateh Taxi Management System";
                mail.Body = "<h1>Fateh Management System - New driver</h1><p>Hi " + driver.FirstName + ",</p><p> Welcome to Fateh Taxi Management System. You account has been created here are your user details.<br>" +
                            "Username:<b>"+user.UserName+"<b> <br> Password: <b>" + tempPassword + "</b></p><p>Please login using this password and remember to reset your password once you login.</p>";
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient(_config.GetValue<string>("Email:Smtp:Host"), Convert.ToInt32(_config.GetValue<string>("Email:Smtp:Port"))))
                {
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = new NetworkCredential(_config.GetValue<string>("Email:Smtp:Username"), _config.GetValue<string>("Email:Smtp:Password"));
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }

            owner.Status = "An email with a new password has been sent to " + driver.FirstName + " " + driver.LastName + ".";


            return RedirectToAction("Index", "Drivers");

        }

    }
}
