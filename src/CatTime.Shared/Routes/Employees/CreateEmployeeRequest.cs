﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatTime.Shared.Routes.Employees
{
    public class CreateEmployeeRequest
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
        
        public string EmailAddress { get; set; }

        public string Password { get; set; }

        public EmployeeRole Role { get; set; }

        public string Department { get; set; }

        public string PhoneNumber { get; set; }
    }
}
