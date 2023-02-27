using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using labMonitor.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Services;
using System.Web.UI.HtmlControls;

namespace labMonitor
{
    public partial class _Default : Page
    {
        DepartmentDAL departmentFactory = new DepartmentDAL();
        ScheduleDAL scheduleFactory = new ScheduleDAL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["User"] != null)
            {
                var user = Session["User"] as labMonitor.Models.User;
                welcome.InnerText = "Welcome, " + user.userFName;
                if (user.userPrivilege < 2)
                {
                    head.Visible = false;
                }
                if (user.userPrivilege == 0) // do this in a switch case once everyone finishes their view
                {
                    studentview.Visible = true;
                }
            }
            else
            {
                Response.Redirect("Login");
            }
            string deptSchedule = scheduleFactory.GetDeptSchedule(2);
            string[] operatingHours = deptSchedule.Split(',');
            string[] daysOfWeek = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

            // Create the schcard div
            Literal schCardDiv = new Literal();
            schCardDiv.Text += "<div class='schcard'>";
            for (int i = 0; i < daysOfWeek.Length; i++)
            {
                schCardDiv.Text += "<p>" + daysOfWeek[i] + ": " + operatingHours[i] + "</p>";
            }
            schCardDiv.Text += "</div>";

            // Add the schcard div to the scheduleDiv literal control
            scheduleDiv.Text = schCardDiv.Text;

        }

        /*
        public static void CreateScheduleDiv(List<string> schedules, HtmlGenericControl parentDiv)
        {
            string[] daysOfWeek = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

            for (int i = 0; i < schedules.Count; i++)
            {
                string operatingHours = GetOperatingHours(schedules[i]);

                // Create the cardcontent div
                HtmlGenericControl cardContentDiv = new HtmlGenericControl("div");
                cardContentDiv.Attributes["class"] = "cardcontent";

                // Create the schcard div
                HtmlGenericControl schCardDiv = new HtmlGenericControl("div");
                schCardDiv.Attributes["class"] = "schcard";

                // Create the p elements for each day of the week
                for (int j = 0; j < daysOfWeek.Length; j++)
                {
                    HtmlGenericControl dayP = new HtmlGenericControl("p");
                    dayP.InnerHtml = daysOfWeek[j] + ": " + operatingHours.Split(',')[j];
                    schCardDiv.Controls.Add(dayP);
                }

                // Add the schcard div to the cardcontent div
                cardContentDiv.Controls.Add(schCardDiv);

                // Add the cardcontent div to the parent div
                parentDiv.Controls.Add(cardContentDiv);
            }

        }        */
    }
}