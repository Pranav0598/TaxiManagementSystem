using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TaxiManagementSystem.Models
{
    public partial class Owner
    {
        public Owner()
        {
            OwnerDriver = new HashSet<OwnerDriver>();
        }

        [Key]
        public int OwnerId { get; set; }
        [Required]
        [StringLength(255)]
        public string LastName { get; set; }
        [StringLength(255)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(255)]
        public string Email { get; set; }
        public int Phonenumber { get; set; }
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(Users.Owner))]
        public virtual Users User { get; set; }
        [InverseProperty("Owner")]
        public virtual ICollection<OwnerDriver> OwnerDriver { get; set; }
    }
}
