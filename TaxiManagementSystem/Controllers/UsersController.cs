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
            else
            {
                HttpContext.Session.SetString("CurrentUserId", loginViewModel.UserId.ToString());
                HttpContext.Session.SetString("CurrentUserName", loginViewModel.UserName.ToString());
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

                    using (SmtpClient smtp = new SmtpClient(_config.GetValue<string>("Email:Smtp:Username"), Convert.ToInt32(_config.GetValue<string>("Email:Smtp:Port"))))
                    {
                        smtp.UseDefaultCredentials = true;
                        smtp.Credentials = new NetworkCredential(_config.GetValue<string>("Email:Smtp:Username"), _config.GetValue<string>("Email:Smtp:Password"));
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }

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
    }
}
