using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClockInApp.Models;
using System.Web.Mvc;
using ClockInApp.Test;
using ClockInApp.DatabaseService;
using ClockInApp.HomeServices;

namespace ClockInApp.Controllers
{
    public class HomeController : Controller
    {
        private TimeSpan timeWorkedInGivenPeriod;
        private string timeToDisplay = "";
        private HomeService homeService;
        public HomeController()
        {
            homeService = new HomeService();
        }

        [HttpGet]
        public ActionResult Index()
        {
            Employee employee = new Employee();
            return View(employee);
        }

        public ActionResult ShowLastLog(int employeeId)
        {
            Employee employee = new Employee();
            employee.LastLog = homeService.CollectLastLog(employeeId);
            ModelState.Clear();
            return View("Index", employee);
        }


        [HttpPost]
        public ActionResult Index(Employee employee, string command)
        {
            if (ModelState.IsValid)
            {
                InOutLogs logs = homeService.GetAllLogs(employee.EmployeeId);
                if (command.Equals("Clock In") && logs.logInList.Count() == logs.logOutList.Count())
                {
                    homeService.LogIn(DateTime.Now, employee.EmployeeId);
                    employee.LastLog = homeService.CollectLastLog(employee.EmployeeId);
                    return RedirectToAction("ShowLastLog", "Home", new { employeeId = employee.EmployeeId });
                }
                if (command.Equals("Clock Out") && logs.logInList.Count() > logs.logOutList.Count())
                {
                    homeService.LogOut(DateTime.Now, employee.EmployeeId);
                    employee.LastLog = homeService.CollectLastLog(employee.EmployeeId);
                    return RedirectToAction("ShowLastLog", "Home", new { employeeId = employee.EmployeeId });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "You have not loged in or loged out.");
                    ModelState.Remove("EmployeeId");
                    return View();
                }
            }else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult HoursWorkedInPeriod(Employee employee)
        {
            if (ModelState.IsValid)
            {
                timeWorkedInGivenPeriod = homeService.CountHoursWorked(employee);
                timeToDisplay = (timeWorkedInGivenPeriod.Days * 24 + timeWorkedInGivenPeriod.Hours) + " Hours " + timeWorkedInGivenPeriod.Minutes + " Minutus";
                Employee newEmployee = new Employee();
                newEmployee.HoursWorkedInGivenPeriod = timeToDisplay;
                ModelState.Clear();
                return View("Index", newEmployee);
            }
            return View("Index");
        }
    }
}