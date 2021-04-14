using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [StringLength(255)]
        public string Model { get; set; }
        [StringLength(255)]
        public string Make { get; set; }
        [Required]
        [StringLength(255)]
        public string Registration { get; set; }
        [StringLength(255)]
        public string Comments { get; set; }
        [DisplayName("Is currently used")]
        public bool? IsWorking { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? RegoExpiry { get; set; }
    }
}