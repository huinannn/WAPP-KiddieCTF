using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WAPP_KiddieCTF.Admin.InnerFunction
{
    public partial class StudentList : System.Web.UI.Page
    {
        private string CourseID => Request.QueryString["course"];

        protected void Page_Load(object sender, EventArgs e)
        {
            // Admin doesn't need session data, just ensure that courseID is passed in the query string
            if (string.IsNullOrEmpty(CourseID))
            {
                Response.Redirect("../Courses.aspx");
                return;
            }

            if (!IsPostBack)
                LoadStudents();
        }

        private void LoadStudents(string search = "")
        {
            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                    SELECT s.Student_ID, s.Student_Name, s.Intake_Code
                    FROM Student s
                    INNER JOIN Assigned_Course ac ON s.Student_ID = ac.Student_ID
                    WHERE ac.Course_ID = @CourseID
                    AND (
                        s.Student_ID LIKE @Search OR 
                        s.Student_Name LIKE @Search OR 
                        s.Intake_Code LIKE @Search
                    )
                    ORDER BY s.Student_ID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CourseID", CourseID);
                cmd.Parameters.AddWithValue("@Search", $"%{search}%");

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                StudentRepeater.DataSource = dt;
                StudentRepeater.DataBind();
            }
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadStudents(txtSearch.Text.Trim());

            // FORCE placeholder update after postback
            string script = @"
                <script type='text/javascript'>
                    setTimeout(function() {
                        var txt = document.getElementById('" + txtSearch.ClientID + @"');
                        var label = txt.parentNode.querySelector('.placeholder-label');
                        if (txt && label) {
                            if (txt.value.trim() === '') {
                                label.style.opacity = '1';
                                label.style.transform = 'translateY(-50%)';
                                label.style.top = '50%';
                                label.style.fontSize = '18px';
                            } else {
                                label.style.opacity = '0';
                                label.style.transform = 'translateY(-50%) scale(0.8)';
                                label.style.top = '10px';
                                label.style.fontSize = '14px';
                            }
                        }
                    }, 150);
                </script>";

            ScriptManager.RegisterStartupScript(this, GetType(), "ForcePlaceholder", script, false);
        }

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string studentId = btn.CommandArgument;

            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "DELETE FROM Assigned_Course WHERE Course_ID = @CourseID AND Student_ID = @StudentID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CourseID", CourseID);
                cmd.Parameters.AddWithValue("@StudentID", studentId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            LoadStudents(txtSearch.Text.Trim());

            string script = @"
                Swal.fire({
                    icon: 'success',
                    title: 'Student removed successfully!',
                    showConfirmButton: false,
                    timer: 1500
                });
            ";

            ScriptManager.RegisterStartupScript(this, GetType(), "RemoveSuccess", script, true);
        }
    }
}
