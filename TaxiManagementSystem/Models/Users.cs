using System;
using System.Collections.Generic;

namespace TaxiManagementSystem.Models
{
    public partial class Users
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public int? Age { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
