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


        string connectionString; // string that will recieve the connection string from the constructor

        private readonly IConfiguration _configuration; // Instance of IConfiguration class... allows us to read in from config file like appsettings

        // The Razor page that creates this data factory and passes the configuration onject to it.
        public UserDAL(IConfiguration configuration)
        {
            //Via the Configuration onject, we could collect the connection string for this project.
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnection");
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

        public void Create()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "INSERT Into users (Title, Description, Address, Address2, City, State, Zip, Category, Email, Phone, Orig_Date, Expire_Date, Active, Flagged, Rate) VALUES (@Title, @Description, @Address, @Address2, @City, @State, @Zip, @Category, @Email, @Phone, @Orig_Date, @Expire_Date, @Active, @Flagged, @Rate);";

                try
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@Title", listing.Title);
                        command.Parameters.AddWithValue("@Description", listing.Description);
                        command.Parameters.AddWithValue("@Address", listing.Address);
                        command.Parameters.AddWithValue("@Address2", listing.Address2);
                        command.Parameters.AddWithValue("@City", listing.City);
                        command.Parameters.AddWithValue("@State", listing.State);
                        command.Parameters.AddWithValue("@Zip", listing.Zip);
                        command.Parameters.AddWithValue("@Category", listing.Category);
                        command.Parameters.AddWithValue("@Email", listing.Email);
                        command.Parameters.AddWithValue("@Phone", listing.Phone);
                        command.Parameters.AddWithValue("@Orig_Date", DateTime.Now); // filling in current date and time
                        command.Parameters.AddWithValue("@Expire_Date", listing.Expire_Date);
                        command.Parameters.AddWithValue("@Active", true); // default value as only admin can set it
                        command.Parameters.AddWithValue("@Flagged", false); // default value as only admin can set it
                        command.Parameters.AddWithValue("@Rate", listing.Rate);

                        connection.Open();

                        listing.Feedback = command.ExecuteNonQuery().ToString() + " Record Added";
                        connection.Close();
                    }
                }
                catch (Exception e)
                {
                    listing.Feedback = "ERROR: " + e.Message;
                }
            }
        }


        //--------------------------------------------------------------------------------------------------------------------------------------------------------->>>>

        public IEnumerable<User> ValidateCredentials(int userID, string password)
        {
            List<User> lstUsers = new List<User>();

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string strSQL = "SELECT TOP 1 * FROM users  WHERE userID = @userID AND userPassword = @userPassword";
                    SqlCommand cmd = new SqlCommand(strSQL, con);
                    cmd.CommandType = CommandType.Text;

                    // Fill in seatch params with login from data
                    cmd.Parameters.AddWithValue("@userID", userID);
                    cmd.Parameters.AddWithValue("@userPassword", password);

                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader(); // Populate the data reader (rdr) from DB

                    // Loop through each record,
                    // For each record fill a temporary trouble ticket object with current record's data.
                    // Then add this temporary ticket object to the list. List will be available to the CSHTML format.
                    while (rdr.Read())
                    {
                        User user = new User(); // Create temporary object

                        // Fill in the temporary object from DB results
                        user.userID = Convert.ToInt32(rdr["userID"].ToString()); // Needed to convert to Int32
                        user.userPassword = rdr["userID"].ToString();

                        lstUsers.Add(user); // Add themp object to list
                    }
                    con.Close();
                }
            }
            catch (Exception err)
            {
                // Nothing at this moment
            }
            return lstUsers;
        }




        public void ChangePassword(string userName, string userPassword)
        {

        }

        public void SetPicture(string UserId, string newPassword)
        {

        }

        public void GetPicture(int UserId)
        {

        }

        public void GetPrivilege(int UserId)
        {

        }

    }
    
}