using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TaxiManagementSystem.Models
{
    public partial class AddTaxiViewModel :BaseViewModel
    {
        public IEnumerable<TaxiViewModel> Taxis { get; set; }
        public TaxiViewModel NewTaxi { get; set; }

        public TaxiViewModel EditTaxi { get; set; }

        public bool DisplayEdit { get; set; }

        public SearchViewModel SearchModel { get; set; }

        public List<CarMake> Makes { get; set; }

        public List<CarModel> Models { get; set; }
    }
}