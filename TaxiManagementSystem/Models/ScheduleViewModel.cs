using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaxiManagementSystem.Models
{
    public partial class ScheduleViewModel :BaseViewModel
    {
       
        public List<Schedule> AllSchedules { get; set; }
        public List<Driver> AllDrivers { get; set; }
        public List<Taxi> AllTaxis { get; set; }

        public int ScheduleId { get; set; }
    
        public int DriverId { get; set; }
        
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy hh:mm tt}")]
        public DateTime ShifTime { get; set; }
        
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy hh:mm tt}")]
        public DateTime ShifTimeEnd { get; set; }
        
        public int TaxiId { get; set; }

        public Taxi Taxi { get; set; }
        public Driver Driver { get; set; }
        [StringLength(250)]
        public string Comments { get; set; }

        public bool DisplayEdit { get; set; }

        public List<Event> Events { get; set; }

        //Edit model
        public int EditScheduleId { get; set; }

        public int EditDriverId { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy hh:mm tt}")]
        public DateTime EditShifTime { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy hh:mm tt}")]
        public DateTime EditShifTimeEnd { get; set; }

        public int EditTaxiId { get; set; }

        public Taxi EditTaxi { get; set; }
        public Driver EditDriver { get; set; }
        [StringLength(250)]
        public string EditComments { get; set; }

        public int DeleteScheduleId { get; set; }

    }
}
