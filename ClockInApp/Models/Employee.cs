using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClockInApp.Models
{
    public class Employee
    {
        [DataType(DataType.DateTime)]
        public DateTime? LogInTime { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? LogOutTime { get; set; }
        [Required]
        [Range(1, 10000)]
        [Display(Name = "Employee Id")]
        public int? EmployeeId { get; set; }
        public string HoursWorkedInGivenPeriod { get; set; }
        [DataType(DataType.DateTime)]
        [Display(Name = "From")]
        public DateTime? PeriodStart { get; set; }
        [DataType(DataType.DateTime)]
        [Display(Name = "To")]
        public DateTime? PeriodEnd { get; set; }
        public DateTime? LastLog { get; set; }
     
    }
}