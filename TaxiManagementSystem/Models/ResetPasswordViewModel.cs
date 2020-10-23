using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaxiManagementSystem.Models
{
    public class ResetPasswordViewModel
    {        
        public int UserId { get; set; }
        [Required]
        public string UserName { get; set; }

        [RegularExpression(@"^.*(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*\(\)_\-+=]).*$", ErrorMessage = "Password needs to have a captial letter, a number and a symbol")]
        [StringLength(20, MinimumLength = 9, ErrorMessage = "Password is short")]
        [Display(Name ="New Password")]
        [DataType(DataType.Password)]
        public string ResetPassword { get; set; }

        [RegularExpression(@"^.*(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*\(\)_\-+=]).*$", ErrorMessage = "Password needs to have a captial letter, a number and a symbol")]
        [StringLength(20, MinimumLength = 9, ErrorMessage = "Password is short")]
        [Compare("ResetPassword")]
        [Display(Name = "Confirm New Password")]
        [DataType(DataType.Password)]
        public string ConfirmResetPassword { get; set; }
    }
}
