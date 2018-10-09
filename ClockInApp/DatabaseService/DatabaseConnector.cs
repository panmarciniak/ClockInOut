using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Configuration;
using ClockInApp.Models;

namespace ClockInApp.DatabaseService
{
    public class DatabaseConnector
    {
        public void InsertClockInTime(DateTime logInTime, int? employeeId)
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                conn.Open();
                SqlCommand insertCommand = new SqlCommand("INSERT INTO ClockIn (LogInTime, EmployeeId) VALUES (@logInTime, @employeeId)", conn);
                insertCommand.Parameters.Add(new SqlParameter("logInTime", logInTime));
                insertCommand.Parameters.Add(new SqlParameter("employeeId", employeeId));
                insertCommand.ExecuteNonQuery();
            }
        }

        public void InsertClockOutTime(DateTime logOutTime, int? employeeId)
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                conn.Open();
                SqlCommand insertCommand = new SqlCommand("INSERT INTO ClockIn (LogOUtTime, EmployeeId) VALUES (@logOutTime, @employeeId)", conn);
                insertCommand.Parameters.Add(new SqlParameter("logOutTime", logOutTime));
                insertCommand.Parameters.Add(new SqlParameter("employeeId", employeeId));
                insertCommand.ExecuteNonQuery();
            }
        }

        public InOutLogs CollectLogs(DateTime? periodStart, DateTime? periodEnd, int? eployeeId)
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                conn.Open();
                SqlCommand collectDataCommand = new SqlCommand("SELECT * FROM ClockIn WHERE EmployeeId = @employeeId AND "+
                    "LogInTime BETWEEN @periodStart AND @periodEnd OR EmployeeId = @employeeId AND LogOutTime BETWEEN @periodStart AND @periodEnd", conn);
                collectDataCommand.Parameters.Add(new SqlParameter("employeeId", eployeeId));
                collectDataCommand.Parameters.Add(new SqlParameter("periodStart", periodStart));
                collectDataCommand.Parameters.Add(new SqlParameter("periodEnd", periodEnd));
                InOutLogs inOutLogs = new InOutLogs();
                inOutLogs.logInList = new List<DateTime>();
                inOutLogs.logOutList = new List<DateTime>();
                using (SqlDataReader reader = collectDataCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0))
                        {
                            DateTime log = reader.GetDateTime(0);
                            inOutLogs.logInList.Add(log);
                        }
                        if (!reader.IsDBNull(1))
                        {
                            DateTime log = reader.GetDateTime(1);
                            inOutLogs.logOutList.Add(log);
                        }
                    }
                    return inOutLogs;
                }
            }
        }

        public InOutLogs CollectAllLogs(int? eployeeId)
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                conn.Open();
                SqlCommand collectDataCommand = new SqlCommand("SELECT * FROM ClockIn WHERE EmployeeId = @employeeId", conn);
                collectDataCommand.Parameters.Add(new SqlParameter("employeeId", eployeeId));
                InOutLogs inOutLogs = new InOutLogs();
                inOutLogs.logInList = new List<DateTime>();
                inOutLogs.logOutList = new List<DateTime>();
                using (SqlDataReader reader = collectDataCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0))
                        {
                            DateTime log = reader.GetDateTime(0);
                            inOutLogs.logInList.Add(log);
                        }
                        if (!reader.IsDBNull(1))
                        {
                            DateTime log = reader.GetDateTime(1);
                            inOutLogs.logOutList.Add(log);
                        }
                    }
                    return inOutLogs;
                }
            }
        }
    }
}



