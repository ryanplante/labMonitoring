using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace labMonitor.Models
{
    public class LabDAL
    {
        public Lab GetLabByID(int? id)
        {
            if (id == null)
            {
                Helpers.LogError(new ArgumentNullException(nameof(id), "Lab ID cannot be null."));
                return null;
            }

            Lab lab = null; // Initialize to null to indicate it may not be found.

            try
            {
                using (SqlConnection con = new SqlConnection(Helpers.GetConnected()))
                {
                    string strSQL = "SELECT * FROM Lab WHERE labID = @labID";
                    SqlCommand cmd = new SqlCommand(strSQL, con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@labID", id);
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();

                    if (rdr.Read()) // Assuming labID is a primary key, there should be at most one record.
                    {
                        lab = new Lab
                        {
                            labID = Convert.ToInt32(rdr["labID"]),
                            labName = rdr["labName"].ToString(),
                            labRoom = rdr["labRoom"].ToString(),
                            deptHead = Convert.ToInt32(rdr["deptHead"]),
                            deptID = Convert.ToInt32(rdr["deptID"])
                        };
                    }
                    else // No lab was found with the given ID.
                    {
                        Helpers.LogError(new KeyNotFoundException($"No lab found with ID {id}."));
                    }
                }
            }
            catch (SqlException e)
            {
                // Log SQL-specific exceptions.
                Helpers.LogError(new ApplicationException($"A database error occurred while retrieving the lab with ID {id}.", e));
            }
            catch (Exception e)
            {
                // Log generic exceptions.
                Helpers.LogError(new ApplicationException($"An error occurred while retrieving the lab with ID {id}.", e));
            }

            return lab; // This could be null if no lab was found or there was an exception.
        }

        // id: the employee's id
        public Lab GetEmployeeLab(int? id)
        {
            if (id == null)
            {
                Helpers.LogError(new ArgumentNullException(nameof(id), "Employee ID (as deptHead) cannot be null."));
                return null; // Or however you wish to handle this case.
            }

            Lab lab = null; // Initialize to null to indicate it may not be found.

            try
            {
                using (SqlConnection con = new SqlConnection(Helpers.GetConnected()))
                {
                    string strSQL = "SELECT * FROM Lab WHERE deptHead = @deptHead";
                    SqlCommand cmd = new SqlCommand(strSQL, con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@deptHead", id);
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();

                    if (rdr.Read()) // Assuming deptHead uniquely identifies a lab, there should be at most one record.
                    {
                        lab = new Lab
                        {
                            labID = Convert.ToInt32(rdr["labID"]),
                            labName = rdr["labName"].ToString(),
                            labRoom = rdr["labRoom"].ToString(),
                            deptHead = Convert.ToInt32(rdr["deptHead"]),
                            deptID = Convert.ToInt32(rdr["deptID"])
                        };
                    }
                    else // No lab was found with the given deptHead ID.
                    {
                        Helpers.LogError(new KeyNotFoundException($"No lab found with deptHead ID {id}."));
                    }
                }
            }
            catch (SqlException e)
            {
                // Log SQL-specific exceptions.
                Helpers.LogError(new ApplicationException($"A database error occurred while retrieving the lab with deptHead ID {id}.", e));
            }
            catch (Exception e)
            {
                // Log generic exceptions.
                Helpers.LogError(new ApplicationException($"An error occurred while retrieving the lab with deptHead ID {id}.", e));
            }

            return lab; // This could be null if no lab was found or there was an exception.
        }

        // id: the lab's department id
        public Lab GetLabByDeptID(int? id)
        {
            if (id == null)
            {
                Helpers.LogError(new ArgumentNullException(nameof(id), "Department ID cannot be null."));
                return null; // Or however you wish to handle this case.
            }

            Lab lab = null; // Initialize to null to indicate that the lab may not be found.

            try
            {
                using (SqlConnection con = new SqlConnection(Helpers.GetConnected()))
                {
                    string strSQL = "SELECT * FROM Lab WHERE deptID = @deptID";
                    SqlCommand cmd = new SqlCommand(strSQL, con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@deptID", id);
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();

                    if (rdr.Read()) // Fetches the first lab associated with the department ID, if any.
                    {
                        lab = new Lab
                        {
                            labID = Convert.ToInt32(rdr["labID"]),
                            labName = rdr["labName"].ToString(),
                            labRoom = rdr["labRoom"].ToString(),
                            deptHead = Convert.ToInt32(rdr["deptHead"]),
                            deptID = Convert.ToInt32(rdr["deptID"])
                        };
                        con.Close();
                    }
                    else // No lab found with the given department ID.
                    {
                        Helpers.LogError(new KeyNotFoundException($"No lab found with department ID {id}."));
                    }
                }
            }
            catch (SqlException e)
            {
                // Log SQL-specific exceptions.
                Helpers.LogError(new ApplicationException($"A database error occurred while retrieving the lab with department ID {id}.", e));
            }
            catch (Exception e)
            {
                // Log generic exceptions.
                Helpers.LogError(new ApplicationException($"An error occurred while retrieving the lab with department ID {id}.", e));
            }

            return lab; // This could be null if no lab was found or there was an exception.
        }


        public List<Lab> GetAllLabs()
        {
            List<Lab> labs = new List<Lab>();

            try
            {
                using (SqlConnection con = new SqlConnection(Helpers.GetConnected()))
                {
                    string strSQL = "SELECT * FROM Lab";
                    SqlCommand cmd = new SqlCommand(strSQL, con);
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        Lab lab = new Lab
                        {
                            labID = Convert.ToInt32(rdr["labID"]),
                            labName = rdr["labName"].ToString(),
                            labRoom = rdr["labRoom"].ToString(),
                            deptHead = Convert.ToInt32(rdr["deptHead"]),
                            deptID = Convert.ToInt32(rdr["deptID"])
                        };

                        labs.Add(lab);
                    }
                    // Close the connection
                    con.Close();
                }
            }
            catch (SqlException e)
            {
                // Log SQL-specific exceptions.
                Helpers.LogError(new ApplicationException("A database error occurred while retrieving all labs.", e));
            }
            catch (Exception e)
            {
                // Log generic exceptions.
                Helpers.LogError(new ApplicationException("An error occurred while retrieving all labs.", e));
            }

            return labs;
        }


        public void AddLab(Lab lab)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Helpers.GetConnected()))
                {
                    // Open the database connection
                    connection.Open();

                    // Define the SQL query to insert a new lab record
                    string insertSql = "INSERT INTO Lab (labName, labRoom, deptHead, deptID) VALUES (@labName, @labRoom, @deptHead, @deptID)";

                    // Create a new SqlCommand object to execute the insert query
                    using (SqlCommand command = new SqlCommand(insertSql, connection))
                    {
                        // Set the parameter values for the insert query
                        command.Parameters.AddWithValue("@labName", lab.labName);
                        command.Parameters.AddWithValue("@labRoom", lab.labRoom);
                        command.Parameters.AddWithValue("@deptHead", lab.deptHead);
                        command.Parameters.AddWithValue("@deptID", lab.deptID);

                        // Execute the insert query
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException e)
            {
                // Log SQL-specific exceptions
                Helpers.LogError(new ApplicationException($"A database error occurred while adding a new lab.", e));
            }
            catch (Exception e)
            {
                // Log generic exceptions
                Helpers.LogError(new ApplicationException($"An error occurred while adding a new lab.", e));
            }
        }


        public void UpdateLab(Lab lab)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Helpers.GetConnected()))
                {
                    // Open the database connection
                    connection.Open();

                    // Define the SQL query to update an existing lab record
                    string updateSql = "UPDATE Lab SET labName = @labName, labRoom = @labRoom, deptHead = @deptHead, deptID = @deptID WHERE labID = @labID";

                    // Create a new SqlCommand object to execute the update query
                    using (SqlCommand command = new SqlCommand(updateSql, connection))
                    {
                        // Set the parameter values for the update query
                        command.Parameters.AddWithValue("@labID", lab.labID);
                        command.Parameters.AddWithValue("@labName", lab.labName);
                        command.Parameters.AddWithValue("@labRoom", lab.labRoom);
                        command.Parameters.AddWithValue("@deptHead", lab.deptHead);
                        command.Parameters.AddWithValue("@deptID", lab.deptID);

                        // Execute the update query
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
            catch (SqlException e)
            {
                // Log SQL-specific exceptions
                Helpers.LogError(new ApplicationException($"A database error occurred while updating lab with ID {lab.labID}.", e));
            }
            catch (Exception e)
            {
                // Log generic exceptions
                Helpers.LogError(new ApplicationException($"An error occurred while updating lab with ID {lab.labID}.", e));
            }
        }


        public void RemoveLab(int labID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Helpers.GetConnected()))
                {
                    connection.Open();
                    string deleteSql = "DELETE FROM Lab WHERE labID = @labID";

                    // Create a new SqlCommand object to execute the delete query
                    using (SqlCommand command = new SqlCommand(deleteSql, connection))
                    {
                        // Set the parameter value for the labID to delete
                        command.Parameters.AddWithValue("@labID", labID);

                        // Execute the delete query
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException e)
            {
                // Log SQL-specific exceptions
                Helpers.LogError(new ApplicationException($"A database error occurred while removing lab with ID {labID}.", e));
            }
            catch (Exception e)
            {
                // Log generic exceptions
                Helpers.LogError(new ApplicationException($"An error occurred while removing lab with ID {labID}.", e));
            }
        }


        public bool DepartmentExists(int deptID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Helpers.GetConnected()))
                {
                    // Open the database connection
                    connection.Open();

                    // Create a SQL query to check if the record has a "deptID" key
                    string query = "SELECT COUNT(*) FROM Department WHERE deptID=@deptID"; // Ensure this targets the correct table, assuming "Department"

                    // Create a command object with the query and connection
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add a parameter for the record ID
                        command.Parameters.AddWithValue("@deptID", deptID);

                        // Execute the query and get the result
                        int count = Convert.ToInt32(command.ExecuteScalar()); // Safely cast the result to int

                        // If the count is greater than zero, the record exists
                        return count > 0;
                    }
                } 
            }
            catch (SqlException e)
            {
                // Log SQL-specific exceptions
                Helpers.LogError(new ApplicationException($"A database error occurred while checking if department with ID {deptID} exists.", e));
                return false; // Optionally return false or re-throw the exception based on your error handling policy
            }
            catch (Exception e)
            {
                // Log generic exceptions
                Helpers.LogError(new ApplicationException($"An error occurred while checking if department with ID {deptID} exists.", e));
                return false;
            }
        }


        public bool UserExists(int deptHead)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Helpers.GetConnected()))
                {
                    // Open the database connection
                    connection.Open();

                    // Create a SQL query to check if the record has a "deptHead" key
                    string query = "SELECT COUNT(*) FROM Lab WHERE deptHead=@deptHead";

                    // Create a command object with the query and connection
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add a parameter for the department head ID
                        command.Parameters.AddWithValue("@deptHead", deptHead);

                        // Execute the query and get the result
                        int count = Convert.ToInt32(command.ExecuteScalar()); // Safely cast the result to int

                        // If the count is greater than zero, the record exists
                        return count > 0;
                    }
                }
            }
            catch (SqlException e)
            {
                // Log SQL-specific exceptions
                Helpers.LogError(new ApplicationException($"A database error occurred while checking if user with deptHead {deptHead} exists.", e));
                return false; 
            }
            catch (Exception e)
            {
                // Log generic exceptions
                Helpers.LogError(new ApplicationException($"An error occurred while checking if user with deptHead {deptHead} exists.", e));
                return false;
            }
        }

    }
}