using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WAPP_KiddieCTF.Student
{
    public partial class EditProfile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void SaveProfile(object sender, EventArgs e)
        {
            try
            {
                string newPassword = Request.Form["passwordField"];
                string studentId = Session["StudentID"]?.ToString();

                if (string.IsNullOrEmpty(newPassword))
                {
                    lblError.Text = "Password cannot be empty.";
                    lblError.Visible = true;
                    return;
                }

                string currentPassword = string.Empty;
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
                {
                    con.Open();

                    string selectQuery = "SELECT Student_Password FROM Student WHERE Student_ID = @StudentID";
                    SqlCommand cmd = new SqlCommand(selectQuery, con);
                    cmd.Parameters.AddWithValue("@StudentID", studentId);

                    var result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        currentPassword = result.ToString();
                    }
                }

                if (newPassword == currentPassword)
                {
                    lblError.Text = "Nothing updated! The new password is the same as the current one.";
                    lblError.Visible = true;
                    return; 
                }

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
                {
                    con.Open();

                    string updateQuery = "UPDATE Student SET Student_Password = @NewPassword WHERE Student_ID = @StudentID";
                    SqlCommand cmd = new SqlCommand(updateQuery, con);
                    cmd.Parameters.AddWithValue("@NewPassword", newPassword);
                    cmd.Parameters.AddWithValue("@StudentID", studentId);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Session["StudentPassword"] = newPassword;
                        Session["UpdateMessage"] = "Profile updated successfully!";
                        Response.Redirect("Profile.aspx");
                    }
                    else
                    {
                        lblError.Text = "Nothing updated!";
                        lblError.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "General Error: " + ex.Message;
                lblError.Visible = true;
            }
        }
    }
}