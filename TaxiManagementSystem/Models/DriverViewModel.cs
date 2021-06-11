using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaxiManagementSystem.Models
{
    public class DriverViewModel : BaseViewModel
    {
        public int DriverId { get; set; }
        [Required]
        [StringLength(500)]
        public string Name { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        public int? Age { get; set; }
        [Required]
        [StringLength(255)]
        public string Email { get; set; }
        public int PhoneNumber { get; set; }
        [StringLength(50)]
        public string DriversLicense { get; set; }
        public bool IsActive { get; set; }
    }
}
