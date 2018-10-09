using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClockInApp.DatabaseService;
using ClockInApp.Models;

namespace ClockInApp.HomeServices
{
    public class HomeService
    {
        private DatabaseConnector dataBaseConnector;
        public HomeService()
        {
            dataBaseConnector = new DatabaseConnector();
        }

        public InOutLogs GetLogs(DateTime? logInTime, DateTime? logOutTime, int? eployeeId)
        {
            return dataBaseConnector.CollectLogs(logInTime, logOutTime, eployeeId);
        }

        public InOutLogs GetAllLogs(int? eployeeId)
        {
            return dataBaseConnector.CollectAllLogs(eployeeId);
        }

        public void LogIn(DateTime logInTime, int? employeeId)
        {
            dataBaseConnector.InsertClockInTime(logInTime, employeeId);
        }

        public void LogOut(DateTime logInTime, int? employeeId)
        {
            dataBaseConnector.InsertClockOutTime(logInTime, employeeId);
        }

        public TimeSpan CountHoursWorked(Employee employee)
        {
            TimeSpan timeWorked = TimeSpan.Zero;
            InOutLogs logs = dataBaseConnector.CollectLogs(employee.PeriodStart, employee.PeriodEnd, employee.EmployeeId);
            foreach (var logIn in logs.logInList)
            {
                List<DateTime> logsOut = logs.logOutList.Where(m => m > logIn).ToList();
                if (logsOut.Count() > 0)
                {
                    DateTime logOut = logsOut.Min();
                    timeWorked += logOut - logIn;
                }
            }
            return timeWorked;
        }

        public DateTime CollectLastLog(int? employeeId)
        {
            InOutLogs logs = dataBaseConnector.CollectAllLogs(employeeId);
            List<DateTime> list = new List<DateTime>();
            list.AddRange(logs.logInList);
            list.AddRange(logs.logOutList);
            return list.Max();
        }
    }
}