﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace labMonitor.Models
{
    public class DepartmentDAL
    {

        private string GetConnected()
        {
            return "Server= sql.neit.edu\\studentsqlserver,4500; Database=SE133_RPlante; User Id=SE133_RPlante;Password=008016590;";
        }

        public Department GetDeptByID(int? id)
        {
            Department dept = new Department();
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnected()))
                {
                    string strSQL = "SELECT * FROM department WHERE deptID = @deptID";
                    SqlCommand cmd = new SqlCommand(strSQL, con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@deptID", id);
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        dept.deptID = Convert.ToInt32(rdr["deptID"]);
                        dept.deptName = rdr["deptName"].ToString();
                    }
                }
            }
            catch (Exception e)
            {
            }
            return dept;
        }
    }
}