using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace labMonitor
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["User"] == null)
            {
                Response.Redirect("Login");
            }
            else
            {
                avatar.ImageUrl = "images/avatar.png";
                var user = Session["User"] as labMonitor.Models.User;
                if (user.userPrivilege < 2)
                {
                    monitor.Visible = false;
                    reports.Visible = false;
                    admin.Visible = false;
                }
            }
        }

        protected void LogOut(object sender, ImageClickEventArgs e)
        {
            Session.Abandon();
            Response.Redirect("Login");
        }
    }
}