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

namespace labMonitor
{
    public partial class _Default : Page
    {
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
            }
            else
            {
                Response.Redirect("Login");
            }
        }

        [WebMethod]
        public static List<string> getStudentNames(string id)
        {
            UserDAL factory = new UserDAL();
            List<User> users = (List<User>)factory.GetNamesByID(int.Parse(id));
            List<String> names = users.Select(z => z.userFName + " " + z.userLName).ToList();
            return names;
        }
    }
}