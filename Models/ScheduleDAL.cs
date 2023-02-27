using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace labMonitor.Models
{
    public class ScheduleDAL
    {
        private string GetConnected()
        {
            return "Server= sql.neit.edu\\studentsqlserver,4500; Database=SE133_RPlante; User Id=SE133_RPlante;Password=008016590;";
        }

        public void GenerateSchedule(User user)
        {
            string textSchedule = "off,off,off,off,off,off,off";
            using (SqlConnection con = new SqlConnection(GetConnected()))
            {
                string sql = "INSERT INTO schedule (student_ID, text_Schedule, dept_ID) VALUES (@student_ID, @text_Schedule, @dept_ID)";
                try
                {
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
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
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
                    SqlCommand cmd = new SqlCommand(strSQL, con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@userID", user.userID);
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.Read()) // if the schedule doesn't exist, then generate it
                    {
                        timeSchedule = rdr["text_Schedule"].ToString();
                        return timeSchedule;
                    }
                    else
                    {
                        GenerateSchedule(user); // user doesn't have a schedule, so generate it for the db
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return timeSchedule; // this will return if the schedule doesn't exist for the user, it'll generate the schedule in the else clause and return a blank schedule back to Calendar.aspx
        }


        public void SetUserSchedule(User user, string timeSchedule)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnected()))
                {
                    string strSQL = "UPDATE Schedule SET text_Schedule = @text_Schedule WHERE student_ID = @userID";
                    SqlCommand cmd = new SqlCommand(strSQL, con);
                    cmd.Parameters.AddWithValue("@userID", user.userID);
                    cmd.Parameters.AddWithValue("@text_Schedule", timeSchedule);
                    cmd.CommandText = strSQL;
                    cmd.CommandType = CommandType.Text;
                    // fill parameters with form values

                    // perform the update
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        static string GetDayOfWeekName(int dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case 0:
                    return "Sunday";
                case 1:
                    return "Monday";
                case 2:
                    return "Tuesday";
                case 3:
                    return "Wednesday";
                case 4:
                    return "Thursday";
                case 5:
                    return "Friday";
                case 6:
                    return "Saturday";
                default:
                    throw new ArgumentException("Invalid day of week");
            }
        }

        static Dictionary<string, Tuple<TimeSpan, TimeSpan>> GetEarliestLatestTimes(List<string> schedules)
        {
            var earliestLatest = new Dictionary<string, Tuple<TimeSpan, TimeSpan>>();

            // Loop through each day of the week
            for (int dayOfWeek = 0; dayOfWeek < 7; dayOfWeek++)
            {
                var times = new List<TimeSpan>();

                // Loop through each user's schedule for this day of the week
                foreach (var schedule in schedules)
                {
                    var scheduleParts = schedule.Split(',');
                    var scheduleForDay = scheduleParts[dayOfWeek];

                    // If the user is off, skip to the next schedule
                    if (scheduleForDay == "off")
                    {
                        continue;
                    }

                    // Parse the start and end times for this user's schedule
                    var startEnd = scheduleForDay.Split('-');
                    var startTime = TimeSpan.Parse(startEnd[0]);
                    var endTime = TimeSpan.Parse(startEnd[1]);

                    times.Add(startTime);
                    times.Add(endTime);
                }

                // Get the earliest and latest times for this day of the week
                var earliest = times.Min();
                var latest = times.Max();

                earliestLatest.Add(GetDayOfWeekName(dayOfWeek), Tuple.Create(earliest, latest));
            }

            return earliestLatest;
        }

        public Dictionary<string, Tuple<TimeSpan, TimeSpan>>GetDeptSchedule(int dept)
        {
            var earliestLatest = new Dictionary<string, Tuple<TimeSpan, TimeSpan>>();
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnected()))
                {
                    string strSQL = "SELECT * FROM Schedule WHERE dept_ID = @dept_ID";
                    SqlCommand cmd = new SqlCommand(strSQL, con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@dept_ID", dept);
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    List<string> dept_schedule = new List<string>();
                    // Loop through everyone's schedule in the department
                    while (rdr.Read())
                    {
                        dept_schedule.Add(rdr["text_Schedule"].ToString());
                    }
                    earliestLatest = GetEarliestLatestTimes(dept_schedule);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return earliestLatest; // this will return if the schedule doesn't exist for the user, it'll generate the schedule in the else clause and return a blank schedule back to Calendar.aspx
        }
    }
}