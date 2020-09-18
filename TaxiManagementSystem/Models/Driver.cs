using System;
using System.Collections.Generic;

namespace TaxiManagementSystem.Models
{
    public partial class Driver
    {
        public int DriverId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public int? Age { get; set; }
        public string Email { get; set; }
        public int Phonenumber { get; set; }
        public string DriversLicense { get; set; }
    }
}
