using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TaxiManagementSystem.Models
{
    public partial class TaxiViewModel : BaseViewModel
    {
        public TaxiViewModel()
        {
            RegoExpiry = DateTime.Now;
        }

        public int TaxiId { get; set; }
        [Required]
        public int ModelId { get; set; }
        public int MakeId { get; set; }
        [Required]
        [StringLength(10)]
        public string Registration { get; set; }
        [StringLength(255)]
        public string Comments { get; set; }
        [DisplayName("Is currently used")]
        public bool IsWorking { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime RegoExpiry { get; set; }
        public CarMake Make { get; set; }
        public CarModel Model { get; set; }
    }
}