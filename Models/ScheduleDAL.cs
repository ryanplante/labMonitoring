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

        public void ChangeMonitorDept(int userID, int dept)
        {
            User tUser = new User();
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnected()))
                {
                    string strSQL = "UPDATE users SET userDept = @userDept, userPrivilege = 1 WHERE userID = @userID";
                    SqlCommand cmd = new SqlCommand(strSQL, con);
                    cmd.Parameters.AddWithValue("@userID", userID);
                    cmd.Parameters.AddWithValue("@userDept", dept);
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
                tUser.userFeedback = "ERROR: " + e.Message;
            }
        }

        public void GetDeptSchedule()
        {

        }

        public void DelSchedule()
        {

        }

        public void DelUserUnavailbility()
        {

        }

        public void AddSchedule()
        {

        }

        public void AddUnavailbility()
        {

        }
    }
}