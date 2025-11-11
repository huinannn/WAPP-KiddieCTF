using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace WAPP_KiddieCTF.Admin.InnerFunction
{
    public partial class EditCourse : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // ADMIN version: no lecturer session check

            if (!IsPostBack)
            {
                string courseId = Request.QueryString["id"];
                if (string.IsNullOrEmpty(courseId))
                {
                    Response.Redirect("../Courses.aspx");
                    return;
                }

                LoadCourse(courseId);
            }
        }

        private void LoadCourse(string courseId)
        {
            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT Course_Name FROM Course WHERE Course_ID = @CourseID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CourseID", courseId);

                conn.Open();
                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    lblCourseID.Text = courseId;
                    txtCourseName.Text = result.ToString();
                }
                else
                {
                    Response.Redirect("../Courses.aspx");
                }
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            string courseName = txtCourseName.Text.Trim();
            if (string.IsNullOrEmpty(courseName))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "EmptyCourseName",
                    "Swal.fire({ icon: 'error', title: 'Course name cannot be empty!', confirmButtonColor: '#3085d6' });",
                    true);
                return;
            }

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                conn.Open();
                string query = "UPDATE Course SET Course_Name = @CourseName WHERE Course_ID = @CourseID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CourseName", courseName);
                cmd.Parameters.AddWithValue("@CourseID", lblCourseID.Text);
                cmd.ExecuteNonQuery();
            }

            string script = @"
                Swal.fire({
                    icon: 'success',
                    title: 'Course updated successfully!',
                    showConfirmButton: false,
                    timer: 1500
                }).then(() => {
                    window.location = '../Courses.aspx';
                });
            ";
            ScriptManager.RegisterStartupScript(this, GetType(), "UpdateSuccess", script, true);
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string courseId = lblCourseID.Text;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                conn.Open();

                // delete assigned students for this course
                string deleteAssigned = "DELETE FROM Assigned_Course WHERE Course_ID = @CourseID";
                using (SqlCommand cmd1 = new SqlCommand(deleteAssigned, conn))
                {
                    cmd1.Parameters.AddWithValue("@CourseID", courseId);
                    cmd1.ExecuteNonQuery();
                }

                // delete the course
                string deleteCourse = "DELETE FROM Course WHERE Course_ID = @CourseID";
                using (SqlCommand cmd2 = new SqlCommand(deleteCourse, conn))
                {
                    cmd2.Parameters.AddWithValue("@CourseID", courseId);
                    cmd2.ExecuteNonQuery();
                }
            }

            string script = @"
                Swal.fire({
                    icon: 'success',
                    title: 'Course and assigned students deleted successfully!',
                    showConfirmButton: false,
                    timer: 1500
                }).then(() => {
                    window.location = '../Courses.aspx';
                });
            ";
            ScriptManager.RegisterStartupScript(this, GetType(), "DeleteCascade", script, true);
        }

        protected void btnAddStudents_Click(object sender, EventArgs e)
        {
            // goes to innerfunction AddStudent
            Response.Redirect("AddStudent.aspx?course=" + lblCourseID.Text + "&from=edit");
        }

        protected void btnViewStudents_Click(object sender, EventArgs e)
        {
            // goes to innerfunction StudentList
            Response.Redirect("StudentList.aspx?course=" + lblCourseID.Text);
        }
    }
}
