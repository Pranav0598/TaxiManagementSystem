using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TaxiManagementSystem.Models
{
    public class EarningsViewModel
    {
        [Required]
        [Column(TypeName = "date")]
        public DateTime ShiftDate { get; set; }
        [Required]
        public int UserId { get; set; }
        public bool IsActive { get; set; }
        public double Earning { get; set; }
        public double Expenditure { get; set; }
        public double IncomeEarned { get; set; }   

        //Display model
        public IEnumerable<Earnings> Earnings { get; set; }
        public double WeeklyEarnings { get; set; }
        public double MonthlyEarnings { get; set; }
        public DateTime EarningsOn { get; set; }
        
        public string Error { get; set; }
        
    }
}
