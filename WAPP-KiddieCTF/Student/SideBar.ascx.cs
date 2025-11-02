using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WAPP_Assignment.Student
{
    public partial class SideBar : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["StudentName"] == null)
            {
                Response.Redirect("/Default.aspx");
                return;
            }

            if (!IsPostBack)
            {
                if (Session["StudentName"] != null && Session["StudentID"] != null)
                {
                    lblName.Text = Session["StudentName"].ToString();
                    lblID.Text = Session["StudentID"].ToString();
                }
                else
                {
                    lblName.Text = "Guest";
                    lblID.Text = "Not logged in";
                }
            }
        }
    }
}