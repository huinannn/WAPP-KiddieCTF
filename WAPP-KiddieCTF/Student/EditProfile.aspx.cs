using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace WAPP_Assignment.Student
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
                string newStudentName = Request.Form["nameField"];
                string studentId = Session["StudentID"]?.ToString();

                if (string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(newStudentName))
                {
                    lblError.Text = "Student Name and Password cannot be empty.";
                    lblError.Visible = true;
                    return;
                }

                string currentPassword = string.Empty;
                string currentStudentName = string.Empty;

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
                {
                    con.Open();

                    string selectQuery = "SELECT Student_Password, Student_Name FROM Student WHERE Student_ID = @StudentID";
                    SqlCommand cmd = new SqlCommand(selectQuery, con);
                    cmd.Parameters.AddWithValue("@StudentID", studentId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            currentPassword = reader["Student_Password"].ToString();
                            currentStudentName = reader["Student_Name"].ToString();
                        }
                    }
                }

                bool isPasswordSame = newPassword == currentPassword;
                bool isNameSame = newStudentName == currentStudentName;

                if (isPasswordSame && isNameSame)
                {
                    lblError.Text = "Nothing updated! The new values are the same as the current ones.";
                    lblError.Visible = true;
                    return;
                }

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
                {
                    con.Open();
                    string updateQuery = @"
                        UPDATE Student 
                        SET Student_Password = @NewPassword, Student_Name = @NewName 
                        WHERE Student_ID = @StudentID";
                    SqlCommand cmd = new SqlCommand(updateQuery, con);
                    cmd.Parameters.AddWithValue("@NewPassword", newPassword);
                    cmd.Parameters.AddWithValue("@NewName", newStudentName);
                    cmd.Parameters.AddWithValue("@StudentID", studentId);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Session["StudentPassword"] = newPassword;
                        Session["StudentName"] = newStudentName;
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