using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TaxiManagementSystem.Models
{
    public partial class CarMake
    {
        public CarMake()
        {
            CarModel = new HashSet<CarModel>();
            Taxi = new HashSet<Taxi>();
        }

        [Key]
        public int MakeId { get; set; }
        [Required]
        [StringLength(255)]
        public string Make { get; set; }

        [InverseProperty("Make")]
        public virtual ICollection<CarModel> CarModel { get; set; }
        [InverseProperty("MakeNavigation")]
        public virtual ICollection<Taxi> Taxi { get; set; }
    }
}
