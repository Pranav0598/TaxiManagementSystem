using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaxiManagementSystem.Models
{
    public partial class Users
    {

        public Users()
        {
            Earnings = new HashSet<Earnings>();
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

        [InverseProperty("User")]
        public virtual ICollection<Earnings> Earnings { get; set; }
    }
}
