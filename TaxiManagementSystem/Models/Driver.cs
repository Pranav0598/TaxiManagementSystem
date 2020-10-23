using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TaxiManagementSystem.Models
{
    public partial class Driver
    {
        [Key]
        public int DriverId { get; set; }
        [Required]
        [StringLength(255)]
        public string LastName { get; set; }
        [StringLength(255)]
        public string FirstName { get; set; }
        public int? Age { get; set; }
        [Required]
        [StringLength(255)]
        public string Email { get; set; }
        public int Phonenumber { get; set; }
        [Required]
        [StringLength(50)]
        public string DriversLicense { get; set; }
    }
}
