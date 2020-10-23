﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaxiManagementSystem.Models
{
    public partial class Earnings
    {
        [Key]
        public int EarningId { get; set; }
        [Column(TypeName = "date")]
        public DateTime ShiftDate { get; set; }
        public int UserId { get; set; }
        public double? Earning { get; set; }
        public double? Expenditure { get; set; }
        public double? IncomeEarned { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(Users.Earnings))]
        public virtual Users User { get; set; }
    }
}
