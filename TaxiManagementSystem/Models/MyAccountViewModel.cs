﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaxiManagementSystem.Models
{
    public class MyAccountViewModel
    {
        public int UserId { get; set; }
     
        public string UserName { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [RegularExpression(@"^.*(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*\(\)_\-+=]).*$", ErrorMessage = "Password needs to have a captial letter, a number and a symbol")]
        [StringLength(20, MinimumLength = 9, ErrorMessage = "Password is short")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [RegularExpression(@"^.*(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*\(\)_\-+=]).*$", ErrorMessage = "Password needs to have a captial letter, a number and a symbol")]
        [StringLength(20, MinimumLength = 9, ErrorMessage = "Password is short")]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public int ErrorNumber { get; set; }
        [DisplayName("Are you the Owner?")]
        public bool IsOwner { get; set; } 
    }
}