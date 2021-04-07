using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office.CustomXsn;
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
    public class UsersController : Controller
    {
        private readonly TaxiManagementSystemContext _context;
        private SmtpClient smtpClient;
        private readonly IConfiguration _config;
        public UsersController(TaxiManagementSystemContext context, SmtpClient smtpClient, IConfiguration config)
        {
            _context = context;
            this.smtpClient = smtpClient;
            _config = config;
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
            Users user = GetUser(loginViewModel.UserName);
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
            else if (user.IsPasswordReset == true) 
            {                
                ResetPasswordViewModel resetPasswordViewModel = new ResetPasswordViewModel() { UserName = user.UserName, UserId = user.UserId};
                return RedirectToAction("ResetPassword", "Users", resetPasswordViewModel);
            }
            else
            {
                HttpContext.Session.SetString("CurrentUserId", loginViewModel.UserId.ToString());
                HttpContext.Session.SetString("CurrentUserName", loginViewModel.UserName.ToString());

                if (user.IsOwner)
                {
                    return RedirectToAction("Index", "Owners");
                }

                return RedirectToAction("Index", "Home");
            }
        }

        public Users GetUser(string username){
          return  _context.Users.Where(e => e.UserName == username).FirstOrDefault();
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
            if (registrationViewModel.UserId != 0)
                return View(registrationViewModel);

            Users user = MapRegistrationModelToUser(registrationViewModel);
            Driver driver = MapRegistrationModelToDriver(registrationViewModel);
            Owner owner = MapRegistrationModelToOwner(registrationViewModel);

            if (ModelState.IsValid)
            {                 
                _context.Add(user);
                await _context.SaveChangesAsync();
                if (registrationViewModel.IsOwner) 
                {
                    owner.UserId = user.UserId;
                    _context.Owner.Add(owner);
                }
                else
                {
                    driver.UserId = user.UserId;
                    _context.Driver.Add(driver);
                }
                    

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
            user.IsOwner = registrationViewModel.IsOwner;
            return user;
        }

        public Driver MapRegistrationModelToDriver(RegistrationViewModel registrationViewModel)
        {
            Driver driver = new Driver();
            driver.FirstName = registrationViewModel.FirstName;
            driver.LastName = registrationViewModel.LastName;
            driver.Email = registrationViewModel.Email;
            driver.Age = registrationViewModel.Age;            
            return driver;
        }

        public Owner MapRegistrationModelToOwner(RegistrationViewModel registrationViewModel)
        {
            Owner owner = new Owner();
            owner.FirstName = registrationViewModel.FirstName;
            owner.LastName = registrationViewModel.LastName;
            owner.Email = registrationViewModel.Email;               
            return owner;
        }

        public RegistrationViewModel validateRegistration(RegistrationViewModel registrationViewModel) 
        {
            Users user = _context.Users.Where(e => e.UserName == registrationViewModel.UserName || e.Email == registrationViewModel.Email).FirstOrDefault();
            registrationViewModel.UserId = user?.UserId??0;
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

        [HttpGet]
        public ActionResult ForgotPassword()
        {           
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ValidateForgotPassword(LoginViewModel loginViewModel)
        {
            Users user = GetUser(loginViewModel.UserName);
            if (user != null)
            {
                string pwd = GeneratePwd();
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("pranavsai0507@gmail.com");
                    mail.To.Add(user.Email);
                    mail.Subject = "Password renewal";
                    mail.Body = "<h1>Pasword Renewal</h1><p>Your new password is <b>"+pwd+"</b></p><p>Please login using this password and reset on login.</p>";
                    mail.IsBodyHtml = true;

                    using (SmtpClient smtp = new SmtpClient(_config.GetValue<string>("Email:Smtp:Host"), Convert.ToInt32(_config.GetValue<string>("Email:Smtp:Port"))))
                    {
                        smtp.UseDefaultCredentials = true;
                        smtp.Credentials = new NetworkCredential(_config.GetValue<string>("Email:Smtp:Username"), _config.GetValue<string>("Email:Smtp:Password"));
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                    user.IsPasswordReset = true;
                    user.Password =  Encryption.Encrypt(pwd);
                    _context.Update(user);
                    await _context.SaveChangesAsync();                    
                }
            }

            return RedirectToAction("Login", "Users");
        }


        public string GeneratePwd() 
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
        private bool UsersExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }


        [HttpGet]
        public ActionResult ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
        {
            return View(resetPasswordViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ValidateResetPassword(ResetPasswordViewModel resetPasswordViewModel)
        {
            Users user = GetUser(resetPasswordViewModel.UserName);
            if (user != null)
            {
                user.IsPasswordReset = false;
                user.Password = Encryption.Encrypt(resetPasswordViewModel.ResetPassword);
                _context.Update(user);
                await _context.SaveChangesAsync();
            }
            HttpContext.Session.SetString("CurrentUserId", resetPasswordViewModel.UserId.ToString());
            HttpContext.Session.SetString("CurrentUserName", resetPasswordViewModel.UserName.ToString());
            return RedirectToAction("Index", "Home");
        }

        }
}
