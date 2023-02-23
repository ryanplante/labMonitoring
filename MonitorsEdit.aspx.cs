﻿using labMonitor.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace labMonitor
{
    public partial class MonitorsEdit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["User"] == null)
            {
                Response.Redirect("Login");
            }
            else
            {
                var user = Session["User"] as labMonitor.Models.User;
                UserDAL userFactory = new UserDAL();
                DepartmentDAL factory = new DepartmentDAL();
                if (user.userPrivilege < 2)
                {
                    Response.Redirect("Default"); // kick them to the appropiate landing screen
                }
                else
                {
                    welcome.InnerText = factory.GetDeptByID(user.userDept).deptName + " Lab Monitors";
                    UpdateGrid();
                }
            }
        }

        private void UpdateGrid()
        {
            UserDAL userFactory = new UserDAL();
            User user = Session["User"] as User;
            List<User> labMonitors = (List<User>)userFactory.GetMonitorsByDept(user.userDept);

            DGLabMonitors.DataSource = labMonitors.Select(o => new User()
            { userID = o.userID, userFName = o.userFName + " " + o.userLName }).ToList();
            //DGLabMonitors.DataSource = userFactory.GetMonitorsByDept(user.userDept);
            DGLabMonitors.DataBind();
        }

        protected void Remove_User(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "RemoveUser")
            {
                UserDAL userFactory = new UserDAL();
                string[] parameters = e.CommandArgument.ToString().Split(',');
                int userID = Int32.Parse(parameters[0]);
                userFactory.ChangeToStudent(userID);
                UpdateGrid();
            }
        }

        protected void Populate_User(object sender, EventArgs e)
        {
            // Get the values from the selected row in the DataGrid
            string userFName = GridResults.SelectedRow.Cells[2].Text;
            string userLName = GridResults.SelectedRow.Cells[3].Text;
            string userID = GridResults.SelectedRow.Cells[1].Text;

            // Set the values of the fields on the page
            txtStudentLast.Text = userFName;
            txtStudentFirst.Text = userLName;
            txtStudentID.Text = userID;
        }

        protected void Add_Monitor(object sender, EventArgs e)
        {
            UserDAL userFactory = new UserDAL();
            lblWarning.Visible = false;
            User user = Session["User"] as labMonitor.Models.User; // get current logged in user so that they can change the dept of the selected user to their department
            // check if the user exists
            if (userFactory.GetOneUser(int.Parse(txtStudentID.Text)) != null)
            {
                userFactory.ChangeMonitorDept(int.Parse(txtStudentID.Text), user.userDept);
            }
            else
            {
                lblWarning.Text = "User does not exist!";
                lblWarning.Visible = true;
            }
            UpdateGrid();
        }
    }
}