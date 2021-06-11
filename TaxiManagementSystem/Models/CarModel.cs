using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TaxiManagementSystem.Models
{
    public partial class CarModel
    {
        public CarModel()
        {
            Taxi = new HashSet<Taxi>();
        }

        [Key]
        public int ModelId { get; set; }
        [Required]
        [StringLength(255)]
        public string Model { get; set; }
        public int MakeId { get; set; }

        [ForeignKey(nameof(MakeId))]
        [InverseProperty(nameof(CarMake.CarModel))]
        public virtual CarMake Make { get; set; }
        [InverseProperty("ModelNavigation")]
        public virtual ICollection<Taxi> Taxi { get; set; }
    }
}
