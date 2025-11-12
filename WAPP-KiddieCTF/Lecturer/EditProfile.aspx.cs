using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WAPP_KiddieCTF.Lecturer
{
    public partial class EditProfile : System.Web.UI.Page
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
        }

        protected void SaveProfile(object sender, EventArgs e)
        {
            string lecturerId = Session["LecturerID"].ToString();
            string newName = Request.Form["nameField"];
            string newPassword = Request.Form["passwordField"];

            if (string.IsNullOrEmpty(newName) || string.IsNullOrEmpty(newPassword))
            {
                lblError.Text = "Fields cannot be empty.";
                lblError.Visible = true;
                return;
            }

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                con.Open();
                string query = "UPDATE Lecturer SET Lecturer_Name = @Name, Lecturer_Password = @Password WHERE Lecturer_ID = @ID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Name", newName);
                cmd.Parameters.AddWithValue("@Password", newPassword);
                cmd.Parameters.AddWithValue("@ID", lecturerId);

                int rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    Session["LecturerName"] = newName;
                    Session["LecturerPassword"] = newPassword;
                    Session["UpdateMessage"] = "Profile updated successfully!";
                    Response.Redirect("Profile.aspx");
                }
                else
                {
                    lblError.Text = "No changes were made.";
                    lblError.Visible = true;
                }
            }
        }

    }
}