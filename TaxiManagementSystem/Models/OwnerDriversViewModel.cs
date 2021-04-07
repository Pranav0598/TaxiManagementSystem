using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace TaxiManagementSystem.Models
{
    public class OwnerDriversViewModel
    {
        public IEnumerable<DriverViewModel> CurrentDrivers { get; set; }

        public IEnumerable<DriverViewModel> AllDrivers { get; set; }

        [DisplayName("Select a driver")]
        public int SelectedDriver { get; set; }
    }
}
