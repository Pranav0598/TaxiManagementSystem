using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaxiManagementSystem.Model;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TaxiManagementSystem.Models
{
    public partial class Taxi
    {
        [Key]
        public int TaxiId { get; set; }
        [Required]
        public int Model { get; set; }
        public int Make { get; set; }
        [Required]
        [StringLength(255)]
        public string Registration { get; set; }
        [StringLength(255)]
        public string Comments { get; set; }
        [DisplayName("Is currently used")]
        public bool? IsWorking { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? RegoExpiry { get; set; }
        [StringLength(255)]
        public string Report { get; set; }
        [ForeignKey(nameof(Make))]
        [InverseProperty(nameof(CarMake.Taxi))]
        public virtual CarMake MakeNavigation { get; set; }
        [ForeignKey(nameof(Model))]
        [InverseProperty(nameof(CarModel.Taxi))]
        public virtual CarModel ModelNavigation { get; set; }

        [InverseProperty("Taxi")]
        public virtual ICollection<Earnings> Earnings { get; set; }
        [InverseProperty("Taxi")]
        public virtual ICollection<Schedule> Schedule { get; set; }
    }
}