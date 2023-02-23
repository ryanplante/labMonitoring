using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using labMonitor.Models;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Drawing;

namespace labMonitor
{
    public partial class Calendar : System.Web.UI.Page
    {
        List<string> daysOfWeek = new List<string>()
            {
                "Sunday",
                "Monday",
                "Tuesday",
                "Wednesday",
                "Thursday",
                "Friday",
                "Saturday"
            };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["User"] != null)
                {
                    var user = Session["User"] as labMonitor.Models.User;
                    if (user.userPrivilege < 1)
                    {
                        Response.Redirect("Login");
                    }
                }
                else
                {
                    Response.Redirect("Login");
                }
                ScheduleGrid.DataSource = GenerateGrid();
                ScheduleGrid.DataBind();
            }
        }

        protected void MakeGray(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string cellValue = e.Row.Cells[1].Text; // index 1 corresponds to "Column2"
                if (cellValue == "off")
                {
                    e.Row.Cells[1].BackColor = Color.Gray;
                }
            }
        }

        private DataTable GenerateGrid()
        {
            UserDAL userFactory = new UserDAL();
            ScheduleDAL scheduleFactory = new ScheduleDAL();
            var user = Session["User"] as labMonitor.Models.User;
            List<User> monitors = (List<User>)userFactory.GetMonitorsByDept(user.userDept); // get all the users for the department to populate them on the datagrid
            /*
             * Format the data columns. This has to be done programatically since we cannot just connect it to a table from a db
             */
            DataTable dt = new DataTable();
            DataColumn dc = new DataColumn("studentName", typeof(string));
            dt.Columns.Add(dc); 
            foreach (String day in daysOfWeek)
            {
                dc = new DataColumn(day, typeof(string));
                dt.Columns.Add(dc);
            }
            foreach (User monitor in monitors)
            {
                DataRow dr = dt.NewRow();
                String schedule = scheduleFactory.GetUserSchedule(monitor);
                dr["studentName"] = monitor.userFName + " " + monitor.userLName;
                String[] splitSchedule = schedule.Split(',');
                for (int i = 0; i < splitSchedule.Length; i++)
                {
                    dr[daysOfWeek[i]] = splitSchedule[i];
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        private void PopulateForm(int row, int col)
        {
            UserDAL userFactory = new UserDAL();
            ScheduleDAL scheduleFactory = new ScheduleDAL();
            var user = Session["User"] as labMonitor.Models.User;
            List<User> monitors = (List<User>)userFactory.GetMonitorsByDept(user.userDept); // get all the users for the department to populate them on the datagrid
            ScheduleForm.Visible = true;
            lblStudent.InnerText = monitors[row].userFName + " " + monitors[row].userLName;
            lblDay.InnerText = "Schedule for:" + daysOfWeek[col];
            coords.Value = String.Format("{0},{0}", row, col);
        }

        protected void DataGrid1_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "GetCellValue")
            {
                // Get the row index and column index from the CommandArgument
                string[] args = e.CommandArgument.ToString().Split(',');
                int rowIndex = int.Parse(args[0]);
                int colIndex = int.Parse(args[1]);
                PopulateForm(rowIndex, colIndex);

            }
        }


        protected void Submit(object sender, EventArgs e)
        {

        }
    }
}