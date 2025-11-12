using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WAPP_KiddieCTF.Lecturer
{
    public partial class Profile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LecturerID"] == null)
            {
                Response.Redirect("../LogIn.aspx");
                return;
            }

            lblLecturerID.Text = Session["LecturerID"].ToString();
            lblLecturerName.Text = Session["LecturerName"].ToString();

            if (Session["UpdateMessage"] != null)
            {
                lblMessage.Text = Session["UpdateMessage"].ToString();
                lblMessage.Visible = true;
                Session["UpdateMessage"] = null;

                ScriptManager.RegisterStartupScript(this, GetType(), "HideMsg",
                    $"setTimeout(function() {{ document.getElementById('{lblMessage.ClientID}').style.display='none'; }}, 2000);", true);
            }
        }
    }
}