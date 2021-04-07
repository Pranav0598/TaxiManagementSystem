using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TaxiManagementSystem.Models
{
    public partial class OwnerDriver
    {
        [Key]
        public int OwnerDriverId { get; set; }
        public int OwnerId { get; set; }
        public int DriverId { get; set; }
        public bool? IsActiveDriver { get; set; }

        [ForeignKey(nameof(DriverId))]
        [InverseProperty("OwnerDriver")]
        public virtual Driver Driver { get; set; }
        [ForeignKey(nameof(OwnerId))]
        [InverseProperty("OwnerDriver")]
        public virtual Owner Owner { get; set; }
    }
}
