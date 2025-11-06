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
    public partial class EditCourse : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LecturerID"] == null || Session["LecturerName"] == null)
            {
                Response.Redirect("../Default.aspx");
                return;
            }

            lblLecturerID.Text = Session["LecturerID"].ToString();
            lblLecturerName.Text = Session["LecturerName"].ToString();

            if (!IsPostBack)
            {
                string courseId = Request.QueryString["id"];
                if (string.IsNullOrEmpty(courseId))
                    Response.Redirect("Courses.aspx");

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
                    Response.Redirect("Courses.aspx");
                }
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            string courseName = txtCourseName.Text.Trim();
            if (string.IsNullOrEmpty(courseName))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "EmptyCourseName",
                    "Swal.fire({ icon: 'error', title: 'Course name cannot be empty!', confirmButtonColor: '#3085d6' });", true);
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
            window.location = 'Courses.aspx';
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

                // 1️⃣ Delete assigned students first
                string deleteAssigned = "DELETE FROM Assigned_Course WHERE Course_ID = @CourseID";
                SqlCommand cmd1 = new SqlCommand(deleteAssigned, conn);
                cmd1.Parameters.AddWithValue("@CourseID", courseId);
                cmd1.ExecuteNonQuery();

                // 2️⃣ Delete the course itself
                string deleteCourse = "DELETE FROM Course WHERE Course_ID = @CourseID";
                SqlCommand cmd2 = new SqlCommand(deleteCourse, conn);
                cmd2.Parameters.AddWithValue("@CourseID", courseId);
                cmd2.ExecuteNonQuery();
            }

            //Show success message
            string script = @"
        Swal.fire({
            icon: 'success',
            title: 'Course and assigned students deleted successfully!',
            showConfirmButton: false,
            timer: 1500
        }).then(() => {
            window.location = 'Courses.aspx';
        });
    ";
            ScriptManager.RegisterStartupScript(this, GetType(), "DeleteCascade", script, true);
        }

        protected void btnAddStudents_Click(object sender, EventArgs e)
        {
            Response.Redirect($"AddStudent.aspx?course={lblCourseID.Text}&from=edit");

        }

        protected void btnViewStudents_Click(object sender, EventArgs e)
        {
            Response.Redirect($"StudentList.aspx?course={lblCourseID.Text}");
        }

    }
}