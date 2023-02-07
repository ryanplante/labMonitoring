using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace labMonitor
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string password = txtPassword.Text;
            string confirm = txtConfirm.Text;

            if (password == confirm)
            {
                Session["IsLoggedIn"] = true;
                Response.Redirect("Home.aspx");
            }
            else
            {
                Response.Write("Passwords don't match!");
            }
        }
    }
}