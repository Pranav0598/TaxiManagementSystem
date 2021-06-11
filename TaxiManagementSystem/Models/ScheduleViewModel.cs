using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaxiManagementSystem.Models
{
    public partial class ScheduleViewModel :BaseViewModel
    {
        public int ScheduleId { get; set; }
        public List<Schedule> AllSchedules { get; set; }
        public List<Driver> AllDrivers { get; set; }
        public List<Taxi> AllTaxis { get; set; }
        [Required]
        public int DriverId { get; set; }
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy hh:mm tt}")]
        public DateTime ShifTime { get; set; }
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy hh:mm tt}")]
        public DateTime ShifTimeEnd { get; set; }
        [Required]
        public int TaxiId { get; set; }

        public Taxi Taxi { get; set; }
        public Driver Driver { get; set; }
        [StringLength(250)]
        public string Comments { get; set; }

        public bool DisplayEdit { get; set; }

        public List<Event> Events { get; set; }
    }
}
