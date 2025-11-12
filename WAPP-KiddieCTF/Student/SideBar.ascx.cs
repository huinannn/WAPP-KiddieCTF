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
                Response.Redirect("/LogIn.aspx");
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
                    Response.Redirect("/LogIn.aspx");
                    return;
                }
            }
        }
    }
}