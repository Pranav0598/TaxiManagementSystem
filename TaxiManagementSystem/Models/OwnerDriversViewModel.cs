using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace TaxiManagementSystem.Models
{
    public class OwnerDriversViewModel : BaseViewModel
    {
        public IEnumerable<DriverViewModel> CurrentDrivers { get; set; }

        public IEnumerable<DriverViewModel> AllDrivers { get; set; }
        
        public DriverViewModel EditDriver { get; set; }

        public bool DisplayEdit { get; set; }

        public SearchViewModel searchModel { get; set; }

        public IEnumerable<ScheduleViewModel> TodaysSchedule { get; set; }

        public AddDriverViewModel AddDriver { get; set; }
    }
}
