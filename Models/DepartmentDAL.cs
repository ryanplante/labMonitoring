using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace labMonitor.Models
{
    public class DepartmentDAL
    {


        public Department GetDeptByID(int? id)
        {
            if (id == null)
            {
                Helpers.LogError(new ArgumentNullException(nameof(id), "Department ID cannot be null."));
            }

            Department dept = new Department();
            try
            {
                using (SqlConnection con = new SqlConnection(Helpers.GetConnected()))
                {
                    string strSQL = "SELECT * FROM department WHERE deptID = @deptID";
                    SqlCommand cmd = new SqlCommand(strSQL, con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@deptID", id);
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();

                    if (rdr.Read()) // Assuming deptID is a primary key, there should be at most one record.
                    {
                        dept.deptID = Convert.ToInt32(rdr["deptID"]);
                        dept.deptName = rdr["deptName"].ToString();
                    }
                    else // No department was found
                    {
                        Helpers.LogError(new KeyNotFoundException($"No department found with ID {id}."));
                    }
                }
            }
            catch (SqlException e)
            {
                // Log SQL-specific exceptions, possibly with more details or specific actions.
                Helpers.LogError(new ApplicationException($"A database error occurred while retrieving the department with ID {id}.", e));
            }
            catch (Exception e)
            {
                // Log generic exceptions.
                Helpers.LogError(new ApplicationException($"An error occurred while retrieving the department with ID {id}.", e));
            }
            return dept;
        }

        public List<Department> GetAllDepartments()
        {
            List<Department> deptList = new List<Department>();

            try
            {
                using (SqlConnection con = new SqlConnection(Helpers.GetConnected()))
                {
                    string strSQL = "SELECT * FROM department";
                    SqlCommand cmd = new SqlCommand(strSQL, con);
                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        Department dept = new Department
                        {
                            deptID = Convert.ToInt32(rdr["deptID"]),
                            deptName = rdr["deptName"].ToString()
                        };
                        deptList.Add(dept);
                    }
                }
            }
            catch (SqlException e)
            {
                // Log SQL-specific exceptions
                Helpers.LogError(new ApplicationException("A database error occurred while retrieving all departments.", e));
            }
            catch (Exception e)
            {
                // Log generic exceptions.
                Helpers.LogError(new ApplicationException("An error occurred while retrieving all departments.", e));
            }

            return deptList;
        }

    }
}