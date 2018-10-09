using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClockInApp.Models
{
    public class InOutLogs
    {
        public List<DateTime> logInList { get; set; }
        public List<DateTime> logOutList { get; set; }
    }
}