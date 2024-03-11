using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;

namespace labMonitor.Models
{
    public class ScheduleDAL
    {
        private string GetConnected()
        {
            return "Server= sql.neit.edu\\studentsqlserver,4500; Database=SE265_LabMonitorProj; User Id=SE265_LabMonitorProj;Password=FaridRyanSpencer;";
        }

        public void GenerateSchedule(User user)
        {
            string textSchedule = "off,off,off,off,off,off,off";
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnected()))
                {
                    string sql = "INSERT INTO schedule (student_ID, text_Schedule, dept_ID) VALUES (@student_ID, @text_Schedule, @dept_ID)";
                    using (SqlCommand command = new SqlCommand(sql, con))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@student_ID", user.userID);
                        command.Parameters.AddWithValue("@dept_ID", user.userDept);
                        command.Parameters.AddWithValue("@text_Schedule", textSchedule);
                        con.Open();

                        command.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }
            catch (SqlException e)
            {
                // Log SQL-specific exceptions
                Console.WriteLine($"A database error occurred while generating schedule for user {user.userID}. {e}");
            }
            catch (Exception e)
            {
                // Log generic exceptions
                Console.WriteLine($"An error occurred while generating schedule for user {user.userID}. {e}");
            }
        }

        public String GetUserSchedule(User user)
        {
            string timeSchedule = "off,off,off,off,off,off,off";
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnected()))
                {
                    string strSQL = "SELECT * FROM Schedule WHERE student_ID = @userID";
                    using (SqlCommand cmd = new SqlCommand(strSQL, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@userID", user.userID);
                        con.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            if (rdr.Read())
                            {
                                timeSchedule = rdr["text_Schedule"].ToString();
                                return timeSchedule;
                            }
                            else
                            {
                                GenerateSchedule(user);
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                // Log SQL-specific exceptions
                Console.WriteLine($"A database error occurred while fetching schedule for user {user.userID}. {e}");
            }
            catch (Exception e)
            {
                // Log generic exceptions
                Console.WriteLine($"An error occurred while fetching schedule for user {user.userID}. {e}");
            }
            return timeSchedule;
        }

        public void SetUserSchedule(User user, string timeSchedule)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnected()))
                {
                    string strSQL = "UPDATE Schedule SET text_Schedule = @text_Schedule WHERE student_ID = @userID";
                    using (SqlCommand cmd = new SqlCommand(strSQL, con))
                    {
                        cmd.Parameters.AddWithValue("@userID", user.userID);
                        cmd.Parameters.AddWithValue("@text_Schedule", timeSchedule);
                        cmd.CommandType = CommandType.Text;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }
            catch (SqlException e)
            {
                // Log SQL-specific exceptions
                Console.WriteLine($"A database error occurred while setting schedule for user {user.userID}. {e}");
            }
            catch (Exception e)
            {
                // Log generic exceptions
                Console.WriteLine($"An error occurred while setting schedule for user {user.userID}. {e}");
            }
        }

        public static string GetOperatingHours(List<string> schedules)
        {
            string[] operatingHours = new string[7];

            for (int dayIndex = 0; dayIndex < 7; dayIndex++)
            {
                List<string> hoursList = new List<string>();
                foreach (string schedule in schedules)
                {
                    string[] scheduleSplit = schedule.Split(',');
                    string hours = scheduleSplit[dayIndex].Trim();
                    if (hours != "off")
                    {
                        hoursList.Add(hours);
                    }
                }
                if (hoursList.Count > 0)
                {
                    List<DateTime[]> shifts = hoursList.Select(s =>
                    {
                        string[] parts = s.Split('-');
                        DateTime start = DateTime.ParseExact(parts[0], "HH:mm", CultureInfo.InvariantCulture);
                        DateTime end = DateTime.ParseExact(parts[1], "HH:mm", CultureInfo.InvariantCulture);
                        return new DateTime[] { start, end };
                    }).ToList();

                    DateTime earliestStart = shifts.Min(shift => shift[0]);
                    DateTime latestEnd = shifts.Max(shift => shift[1]);

                    operatingHours[dayIndex] = earliestStart.ToString("HH:mm") + "-" + latestEnd.ToString("HH:mm");
                }
                else
                {
                    operatingHours[dayIndex] = "off";
                }
            }

            return string.Join(",", operatingHours);
        }

        public string GetDeptSchedule(int dept)
        {
            string operatingSchedule = "off,off,off,off,off,off,off";
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnected()))
                {
                    string strSQL = "SELECT * FROM Schedule WHERE dept_ID = @dept_ID";
                    using (SqlCommand cmd = new SqlCommand(strSQL, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@dept_ID", dept);
                        con.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            List<string> dept_schedule = new List<string>();
                            while (rdr.Read())
                            {
                                dept_schedule.Add(rdr["text_Schedule"].ToString());
                            }
                            operatingSchedule = GetOperatingHours(dept_schedule);
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                // Log SQL-specific exceptions
                Console.WriteLine($"A database error occurred while fetching department schedule for department {dept}. {e}");
            }
            catch (Exception e)
            {
                // Log generic exceptions
                Console.WriteLine($"An error occurred while fetching department schedule for department {dept}. {e}");
            }
            return operatingSchedule;
        }
    }
}
