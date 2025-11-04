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
    public partial class AddNewCourse : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GenerateNextCourseID();
            }
        }

        private void GenerateNextCourseID()
        {
            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT MAX(CAST(SUBSTRING(Course_ID, 3, LEN(Course_ID)) AS INT)) FROM Course WHERE Course_ID LIKE 'CR%'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                object result = cmd.ExecuteScalar();
                int nextId = (result == DBNull.Value) ? 1 : Convert.ToInt32(result) + 1;
                lblCourseID.Text = "CR" + nextId.ToString("D3"); // CR001, CR002, CR013...
            }
        }

        protected void btnDone_Click(object sender, EventArgs e)
        {
            string courseName = txtCourseName.Text.Trim();
            if (string.IsNullOrEmpty(courseName))
            {
                string errorScript = @"
                    Swal.fire({
                        icon: 'error',
                        title: 'Course name cannot be empty!',
                        showConfirmButton: true,
                        confirmButtonColor: '#3085d6'
                    });
                ";
                ScriptManager.RegisterStartupScript(this, GetType(), "EmptyCourseName", errorScript, true);
                return;
            }

            string connStr = ConfigurationManager.ConnectionStrings["KiddieCTFConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "INSERT INTO Course (Course_ID, Course_Name, Lecturer_ID) VALUES (@CourseID, @CourseName, @LecturerID)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CourseID", lblCourseID.Text);     // e.g. CR013
                cmd.Parameters.AddWithValue("@CourseName", courseName);
                cmd.Parameters.AddWithValue("@LecturerID", Session["LecturerID"]?.ToString() ?? "LC001");

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            // SUCCESS: Show message + redirect
            string successScript = @"
                Swal.fire({
                    icon: 'success',
                    title: 'Course created successfully!',
                    showConfirmButton: false,
                    timer: 1500
                }).then(() => {
                    window.location = 'Courses.aspx';
                });
            ";
            ScriptManager.RegisterStartupScript(this, GetType(), "AddCourseSuccess", successScript, true);
        }

        protected void btnAddStudents_Click(object sender, EventArgs e)
        {
            Session["TempCourseID"] = lblCourseID.Text;
            Session["TempCourseName"] = txtCourseName.Text.Trim();
            Response.Redirect("AddStudent.aspx");
        }

        protected void btnViewStudents_Click(object sender, EventArgs e)
        {
            Response.Redirect($"StudentList.aspx?course={lblCourseID.Text}");
        }

    }
}