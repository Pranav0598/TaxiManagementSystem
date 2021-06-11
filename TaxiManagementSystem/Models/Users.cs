
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TaxiManagementSystem.Models
{
    public partial class Users
    {
        public Users()
        {
            Driver = new HashSet<Driver>();
            Earnings = new HashSet<Earnings>();
            Owner = new HashSet<Owner>();
        }

        [Key]
        public int UserId { get; set; }
        [Required]
        [StringLength(255)]
        public string UserName { get; set; }
        [Required]
        [StringLength(255)]
        public string LastName { get; set; }
        [StringLength(255)]
        public string FirstName { get; set; }
        public int? Age { get; set; }
        [Required]
        [StringLength(255)]
        public string Email { get; set; }
        [Required]
        [StringLength(255)]
        public string Password { get; set; }
        public bool? IsPasswordReset { get; set; }
        public bool IsOwner { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<Driver> Driver { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<Earnings> Earnings { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<Owner> Owner { get; set; }
    }
}
