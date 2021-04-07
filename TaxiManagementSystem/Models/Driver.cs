using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TaxiManagementSystem.Models
{
    public partial class Driver
    {
        public Driver()
        {
            OwnerDriver = new HashSet<OwnerDriver>();
        }

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
        [StringLength(50)]
        public string DriversLicense { get; set; }
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(Users.Driver))]
        public virtual Users User { get; set; }
        [InverseProperty("Driver")]
        public virtual ICollection<OwnerDriver> OwnerDriver { get; set; }
    }
}
