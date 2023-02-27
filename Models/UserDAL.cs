using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations; // Validation via Data Annotations

// Added for use of SQL Database components
using System.Data;
using System.Data.SqlClient;

// Added in order to use IConfiguration, so we can get our DB Connection string from the appsettings.json file
using Microsoft.Extensions.Configuration;

// Adedd for Session vars
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Session;
// Added to encrypt/decrypt passwords
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace labMonitor.Models
{
    public class UserDAL
    {

        //------------------- Notes ------------------------->>
        //
        //  ValidateCredentials(str userName, str userPassword)
        //     *This will perform an SQL query to compare userPassword with the
        //      password on the database to validate credentials.If they are valid, return the user database object.
        //      If they are not, return false. 
        //  ChangePassword(str id, str newPassword)
        //     *This will update the userPassword with the new supplied password in the database.
        //      It uses SHA-1 encryption with userSalt for security purposes.
        //  SetPicture(int id, newPicture)
        //     *This method will just upload a new picture to the database and rename the file<userID.jpg>.
        //  GetPicture(int id)
        //     *This method will get the picture from the database given the userID is set.
        //      If it doesn’t exist or there is an error loading the image,
        //      return the default picture, which will just be a gray glyphicon of a user account.
        //  GetPrivilege(int id)
        //     *This method just returns an integer representing the user’s privilege level.
        //      The privileges are tiered as followed:
        //
        //     *0: Student level.Students only have access to a read only schedule for when the labs are open.
        //
        //--------------------------------------------------->>


        // 1. Create Connection with "user" DataBase
        // 2. 
        // 3.
        // 4.
        // 5.
        // 6.
        // 7.
        // 8.
        // 9.
        // 10.

        //using appsetings,json --> 

        //private string GetConnected() // Creates the connection with db ( Temporarry meto )
        //{
        //    return @"Server=sql.neit.edu\studentsqlserver,4500;Database=capstone;User Id=;Password=";
        //}

        // The Razor page that creates this data factory and passes the configuration onject to it.
        public UserDAL()
        {
        }

        private string GetConnected()
        {
            return "Server= sql.neit.edu\\studentsqlserver,4500; Database=SE133_RPlante; User Id=SE133_RPlante;Password=008016590;";
        }

        public void AddUser()
        {

        }

        private void EncryptPassword(string password)
        {
            // Generate a random salt
            byte[] salt = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            // Combine the password and salt and hash them
            byte[] passwordWithSalt = Encoding.UTF8.GetBytes(password + Convert.ToBase64String(salt));
            byte[] hashedPasswordWithSalt = SHA1.Create().ComputeHash(passwordWithSalt);

        }

        public User GetOneUser(int ?id)
        {
            User user = new User();
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnected()))
                {
                    string strSQL = "SELECT * FROM users WHERE UserID = @UserID;";
                    SqlCommand cmd = new SqlCommand(strSQL, con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@UserID", id);
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        user.userID = Convert.ToInt32(rdr["userID"]);
                        user.userFName = rdr["userFName"].ToString();
                        user.userLName = rdr["userLName"].ToString();
                        user.userDept = Convert.ToInt32(rdr["userDept"]);
                        user.userPrivilege = Convert.ToInt32(rdr["userPrivilege"]);
                    }
                }
            }
            catch (Exception e)
            {
            }
            return user;

        }

        public void Create()
        {
            using (SqlConnection connection = new SqlConnection(GetConnected()))
            {
                string sql = "INSERT Into users (userID, userFName, userLName, userPassword, userSalt, userDept, userPrivilege) VALUES (@userID, @userFName, @userLName, @userPassword, @userSalt, @userDept, @userPrivilege)";

                try
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@userID", 1);
                        // Generate a random salt
                        byte[] salt = new byte[16];
                        using (var rng = new RNGCryptoServiceProvider())
                        {
                            rng.GetBytes(salt);
                        }
                        // Combine the password and salt and hash them
                        byte[] passwordWithSalt = Encoding.UTF8.GetBytes("password" + Convert.ToBase64String(salt));
                        byte[] hashedPasswordWithSalt = SHA1.Create().ComputeHash(passwordWithSalt);

                        command.Parameters.AddWithValue("@userSalt", Convert.ToBase64String(salt));
                        command.Parameters.AddWithValue("@userPassword", Convert.ToBase64String(hashedPasswordWithSalt));
                        
                        command.Parameters.AddWithValue("@userFName", "Test");
                        command.Parameters.AddWithValue("@userLName", "Test");
                        command.Parameters.AddWithValue("@userDept", 0);
                        command.Parameters.AddWithValue("@userPrivilege", 0);

                        connection.Open();

                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
                catch (Exception e)
                {
                    //todo
                }
            }
        }

        public IEnumerable<User>GetMonitorsByDept(int? deptID)
        {
            List<User> monitors = new List<User>(); // Listings from DB table
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnected()))
                {
                    String strSQL = ("Select * FROM users WHERE userDept = @deptID AND userPrivilege = 1");
                    SqlCommand cmd = new SqlCommand(strSQL, con);
                    cmd.Parameters.AddWithValue("@deptID", deptID);
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        User labMonitor = new User();
                        labMonitor.userID = Convert.ToInt32(rdr["userID"]);
                        labMonitor.userDept = Convert.ToInt32(rdr["userDept"]);
                        labMonitor.userFName = rdr["userFName"].ToString();
                        labMonitor.userLName = rdr["userLName"].ToString();
                        monitors.Add(labMonitor);
                    }
                    con.Close();
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            return monitors;
        }

        //--------------------------------------------------------------------------------------------------------------------------------------------------------->>>>

        public bool ValidateCredentials(int userID, string password)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnected()))
                {
                    string strSQL = "SELECT userPassword, userSalt FROM users WHERE userID = @userID";
                    SqlCommand cmd = new SqlCommand(strSQL, con);
                    cmd.CommandType = CommandType.Text;

                    // Fill in seatch params with login from data
                    cmd.Parameters.AddWithValue("@userID", userID);

                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader(); // Populate the data reader (rdr) from DB

                    if (rdr.Read())
                    {
                        string encryptedPassword = rdr["userPassword"].ToString();
                        string salt = rdr["userSalt"].ToString();
                        // Convert the encrypted password and salt to bytes
                        byte[] encryptedPasswordBytes = Convert.FromBase64String(encryptedPassword);
                        byte[] saltBytes = Convert.FromBase64String(salt);

                        // Combine the password and salt and hash them
                        byte[] passwordWithSalt = Encoding.UTF8.GetBytes(password + salt);
                        byte[] hashedPasswordWithSalt = SHA1.Create().ComputeHash(passwordWithSalt);

                        // Compare the hashed password with the encrypted password
                        if (Convert.ToBase64String(hashedPasswordWithSalt) == Convert.ToBase64String(encryptedPasswordBytes))
                        {
                            return true;
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception err)
            {
                // Nothing at this moment
            }
            return false;
        }


        public IEnumerable<User> GetNamesByID(int? userID)
        {
            List<User> monitors = new List<User>(); // Listings from DB table
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnected()))
                {
                    String strSQL = ("Select * FROM users WHERE userID like @userID % AND userPrivilege = 0");
                    SqlCommand cmd = new SqlCommand(strSQL, con);
                    cmd.Parameters.AddWithValue("@userID", userID);
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        User labMonitor = new User();
                        labMonitor.userID = Convert.ToInt32(rdr["userID"]);
                        labMonitor.userFName = rdr["userFName"].ToString();
                        labMonitor.userLName = rdr["userLName"].ToString();
                        monitors.Add(labMonitor);
                    }
                    con.Close();
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            return monitors;
        }

        public IEnumerable<User> SearchByName(string fname, string lname)
        {
            List<User> monitors = new List<User>(); // Listings from DB table
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnected()))
                {
                    String strSQL = ("Select * FROM users WHERE userFName like % @userFName % AND % @userFName % = 0");
                    SqlCommand cmd = new SqlCommand(strSQL, con);
                    //cmd.Parameters.AddWithValue("@userID", userID);
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        User labMonitor = new User();
                        labMonitor.userID = Convert.ToInt32(rdr["userID"]);
                        labMonitor.userFName = rdr["userFName"].ToString();
                        labMonitor.userLName = rdr["userLName"].ToString();
                        monitors.Add(labMonitor);
                    }
                    con.Close();
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            return monitors;
        }

        public List<User> SearchUsersByFullName(string userFName, string userLName)
        {
            List<User> users = new List<User>();

            using (SqlConnection con = new SqlConnection(GetConnected()))
            {
                String strSQL = "SELECT * FROM users WHERE userFName LIKE @userFName AND userLName LIKE @userLName AND userPrivilege < 2"; // get all users that are not students

                using (SqlCommand command = new SqlCommand(strSQL, con))
                {
                    // perform wildcard search on both first name and last name with %
                    command.Parameters.AddWithValue("@userFName", "%" + userFName + "%");
                    command.Parameters.AddWithValue("@userLName", "%" + userLName + "%");

                    con.Open();

                    using (SqlDataReader rdr = command.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            User user = new User();
                            user.userID = Convert.ToInt32(rdr["userID"]);
                            user.userFName = rdr["userFName"].ToString();
                            user.userLName = rdr["userLName"].ToString();

                            users.Add(user);
                        }
                    }
                }
            }

            return users;
        }

        public List<User> SearchTeachersByFullName(string userFName, string userLName)
        {
            List<User> users = new List<User>();

            using (SqlConnection con = new SqlConnection(GetConnected()))
            {
                String strSQL = "SELECT * FROM users WHERE userFName LIKE @userFName AND userLName LIKE @userLName AND userPrivilege > 2"; // get all users that are students

                using (SqlCommand command = new SqlCommand(strSQL, con))
                {
                    // perform wildcard search on both first name and last name with %
                    command.Parameters.AddWithValue("@userFName", "%" + userFName + "%");
                    command.Parameters.AddWithValue("@userLName", "%" + userLName + "%");

                    con.Open();

                    using (SqlDataReader rdr = command.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            User user = new User();
                            user.userID = Convert.ToInt32(rdr["userID"]);
                            user.userFName = rdr["userFName"].ToString();
                            user.userLName = rdr["userLName"].ToString();

                            users.Add(user);
                        }
                    }
                }
            }

            return users;
        }

        /*
         * This function will change their department id and change their permission level to 1
         */
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

        /*
         * This function will change their department id
         */
        public void ChangeDept(User user, int dept)
        {
            User tUser = new User();
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnected()))
                {
                    string strSQL = "UPDATE users SET userDept = @userDept WHERE userID = @userID";
                    SqlCommand cmd = new SqlCommand(strSQL, con);
                    cmd.Parameters.AddWithValue("@userID", user.userID);
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

        public void ChangeToStudent(int userID)
        {
            User tUser = new User();
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnected()))
                {
                    string strSQL = "UPDATE users SET userPrivilege = 0 WHERE userID = @userID";
                    SqlCommand cmd = new SqlCommand(strSQL, con);
                    cmd.Parameters.AddWithValue("@userID", userID);
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



        public void ChangePassword(string userName, string userPassword)
        {

        }

        public void SetPicture(string UserId, string newPassword)
        {

        }

        public string GetPicture(int UserId)
        {
            string defaultPath = (@"images\avatars\avatar.jpg");

            String path = @"images\avatars\" + UserId.ToString() + ".jpg";
            if (File.Exists(System.Web.HttpContext.Current.Server.MapPath(path)))
            {
                return path;
            }
            else
            {
                return defaultPath;
            }
        }


    }
    
}