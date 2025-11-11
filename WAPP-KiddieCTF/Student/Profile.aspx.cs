using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WAPP_KiddieCTF.Student
{
    public partial class Profile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UpdateMessage"] != null)
            {
                lblMessage.Text = Session["UpdateMessage"].ToString();
                lblMessage.Visible = true;
                Session["UpdateMessage"] = null;

                ScriptManager.RegisterStartupScript(this, this.GetType(), "HideMessage", "setTimeout(function() { document.getElementById('" + lblMessage.ClientID + "').style.display = 'none'; }, 2000);", true);
            }
        }
    }
}