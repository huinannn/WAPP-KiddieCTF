using System;
using System.Web.UI;

namespace WAPP_KiddieCTF.Student
{
    public partial class SideBar : System.Web.UI.UserControl
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