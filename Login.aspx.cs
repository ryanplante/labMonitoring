using labMonitor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace labMonitor
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        private readonly IConfiguration _configuration;
        public User tUser { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            UserDAL user = new UserDAL(_configuration);
            int userID = Int32.Parse(txtUsername.Text);
            string password = txtPassword.Text;

            if (user.ValidateCredentials(userID, password) != null)
            {
                Session["IsLoggedIn"] = true;
                Response.Redirect("Default.aspx");
            }
            else
            {
                Response.Write("Invalid username or password.");
            }
        }
    }
}