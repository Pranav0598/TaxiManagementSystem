using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TaxiManagementSystem.Models
{
    public partial class Schedule
    {
        [Key]
        public int ScheduleId { get; set; }
        [StringLength(250)]
        public string Comments { get; set; }
        public int DriverId { get; set; }
        public int TaxiId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ShiftTime { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ShiftTimeEnd { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(DriverId))]
        [InverseProperty("Schedule")]
        public virtual Driver Driver { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(TaxiId))]
        [InverseProperty("Schedule")]
        public virtual Taxi Taxi { get; set; }
    }
}
