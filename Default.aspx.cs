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
        LabDAL labFactory = new LabDAL();
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
                    if (!IsPostBack)
                    {
                        studentview.Visible = true;
                        List<Lab> labs = labFactory.GetAllLabs();

                        foreach (Lab lab in labs)
                        {
                            scheduleLiteral.Text += "<div class='labcard'>";
                            scheduleLiteral.Text += "<div class='htags'>\n <h3 class='lbn'>" + lab.labName + "</h3><h3 class='rn'>" + lab.labRoom + "</h3></div>";
                            scheduleLiteral.Text += "<div class='cardcontent'>";
                            scheduleLiteral.Text += "<div class='schcard'>";
                            string[] operatingHours = scheduleFactory.GetDeptSchedule(lab.deptID).Split(',');
                            string[] daysOfWeek = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
                            for (int i = 0; i < daysOfWeek.Length; i++)
                            {
                                scheduleLiteral.Text += "<p>" + daysOfWeek[i] + ": " + operatingHours[i] + "</p>";
                            }
                            scheduleLiteral.Text += "</div>";
                            scheduleLiteral.Text += "<div class='imgbk'> <img src='images/image 39.png' /></div>";
                            scheduleLiteral.Text += "</div></div>";
                        }
                    }

                }
            }
            else
            {
                Response.Redirect("Login");
            }


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