using labMonitor.Models;
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
                    List<User> labMonitors = (List<User>)userFactory.GetMonitorsByDept(user.userDept);

                    DGLabMonitors.DataSource = labMonitors.Select(o => new User()
                    { userID = o.userID, userFName = o.userFName + " " + o.userLName }).ToList();
                    //DGLabMonitors.DataSource = userFactory.GetMonitorsByDept(user.userDept);
                    DGLabMonitors.DataBind();
                }
            }
        }

        protected void Unnamed_Click(object sender, EventArgs e)
        {

        }

        protected void Search_Users(object sender, EventArgs e)
        {
            UserDAL userFactory = new UserDAL();
            DepartmentDAL factory = new DepartmentDAL();
            //List<User> labMonitors = (List<User>)userFactory.GetMonitorsByDept(user.userDept);
        }
    }
}