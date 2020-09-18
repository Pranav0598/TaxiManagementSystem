using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office.CustomXsn;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaxiManagementSystem.Models;
using TaxiManagementSystem.Security;

namespace TaxiManagementSystem.Controllers
{
    public class UsersController : Controller
    {
        private readonly TaxiManagementSystemContext _context;       

        public UsersController(TaxiManagementSystemContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            if (loginViewModel.UserName == null || loginViewModel.Password == null)
            {
                loginViewModel.UserId = -1;
                return View(loginViewModel);
            }            
            Users user = _context.Users.Where(e => e.UserName == loginViewModel.UserName).FirstOrDefault();
            if (user != null && !string.IsNullOrEmpty(user.Password))
            {
                string decryptedPassword = Encryption.Decrypt(user.Password);
                loginViewModel.UserId = decryptedPassword == loginViewModel.Password ? user.UserId : -1;
            }
            else 
            {
                loginViewModel.UserId = -1;
            }

            if (loginViewModel.UserId == -1)
                return View(loginViewModel);
            else
            {
                HttpContext.Session.SetString("CurrentUserId", loginViewModel.UserId.ToString());
                HttpContext.Session.SetString("CurrentUserName", loginViewModel.UserName.ToString());               
                return RedirectToAction("Index", "Home");
            }
        }


        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegistrationAsync(RegistrationViewModel registrationViewModel)
        {
            registrationViewModel = validateRegistration(registrationViewModel);
            if (registrationViewModel.UserId == -1)
                return View(registrationViewModel);

            Users user = MapRegistrationModelToUser(registrationViewModel);

            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("SuccessfullRegistration", "Users");
            }

            return View(registrationViewModel);
          
               
        }

       

        public Users MapRegistrationModelToUser(RegistrationViewModel registrationViewModel) 
        {
            Users user = new Users();
            user.UserName = registrationViewModel.UserName;
            user.FirstName = registrationViewModel.FirstName;
            user.LastName = registrationViewModel.LastName;
            user.Email = registrationViewModel.Email;
            user.Age = registrationViewModel.Age;
            user.Password = Encryption.Encrypt(registrationViewModel.Password);
            return user;
        }

        public RegistrationViewModel validateRegistration(RegistrationViewModel registrationViewModel) 
        {
            Users user = _context.Users.Where(e => e.UserName == registrationViewModel.UserName).FirstOrDefault();
            registrationViewModel.UserId = user?.UserId ?? 0;
            return registrationViewModel;
        }

        [HttpGet]
        public ActionResult SuccessfullRegistration()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Logout()
        {
            HttpContext.Session.Remove("CurrentUserId");
            HttpContext.Session.Remove("CurrentUserName");
            return View();
        }


        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (users == null)
            {
                return NotFound();
            }

            return View(users);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,UserName,LastName,FirstName,Age,Email,Password")] Users users)
        {
            if (ModelState.IsValid)
            {
                _context.Add(users);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(users);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users = await _context.Users.FindAsync(id);
            if (users == null)
            {
                return NotFound();
            }
            return View(users);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,UserName,LastName,FirstName,Age,Email,Password")] Users users)
        {
            if (id != users.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(users);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsersExists(users.UserId))
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
            return View(users);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (users == null)
            {
                return NotFound();
            }

            return View(users);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var users = await _context.Users.FindAsync(id);
            _context.Users.Remove(users);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsersExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
