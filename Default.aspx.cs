using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace labMonitor
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["User"] != null)
            {
                var user = Session["User"] as labMonitor.Models.User;
                lblUserPrivilege.Text = "Welcome, " + user.userFName;
            }
            else
            {
                lblUserPrivilege.Text = "No privilege information available.";
            }
        }
    }
}