using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaxiManagementSystem.Models
{
    public class AddDriverViewModel
    {

        public int UserId { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string FirstName { get; set; }
        public int? Age { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public int ErrorNumber { get; set; }
        [DisplayName("Are you the Owner?")]
        public bool IsOwner { get; set; }
        public int PhoneNumber { get; set; }
        [StringLength(50)]
        public string DriversLicense { get; set; }
        public bool IsActive { get; set; }
    }
}
