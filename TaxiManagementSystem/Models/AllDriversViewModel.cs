using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Bibliography;

namespace TaxiManagementSystem.Models
{
    public class AllDriversViewModel
    {
        public IEnumerable<DriverViewModel> CurrentDrivers { get; set; }

        public IEnumerable<DriverViewModel> AllDrivers { get; set; }
        
        public int SelectedDriver { get; set; }

        public bool DisplayEdit { get; set; }
    }
}
