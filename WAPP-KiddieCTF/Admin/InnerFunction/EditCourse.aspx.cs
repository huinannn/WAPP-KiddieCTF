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
            Session.Clear();

            if (!IsPostBack)
            {
                string courseId = Request.QueryString["id"];

                if (String.IsNullOrEmpty(courseId))
                    Response.Redirect("../Courses.aspx");

                LoadLecturers();
                LoadCourse(courseId);
            }
        }

        private void LoadLecturers()
        {
            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT Lecturer_ID, Lecturer_Name FROM Lecturer ORDER BY Lecturer_Name";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                ddlLecturer.DataSource = cmd.ExecuteReader();
                ddlLecturer.DataTextField = "Lecturer_Name";
                ddlLecturer.DataValueField = "Lecturer_ID";
                ddlLecturer.DataBind();
            }

            ddlLecturer.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Lecturer --", ""));
        }

        private void LoadCourse(string courseId)
        {
            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query =
                    "SELECT Course_Name, Lecturer_ID FROM Course WHERE Course_ID = @CourseID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CourseID", courseId);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    lblCourseID.Text = courseId;
                    txtCourseName.Text = reader["Course_Name"].ToString();

                    string lecturerId = reader["Lecturer_ID"].ToString();

                    if (!String.IsNullOrEmpty(lecturerId) && ddlLecturer.Items.FindByValue(lecturerId) != null)
                        ddlLecturer.SelectedValue = lecturerId;
                }
                else
                {
                    Response.Redirect("../Courses.aspx");
                }
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            string courseId = lblCourseID.Text;
            string courseName = txtCourseName.Text.Trim();
            string lecturerId = ddlLecturer.SelectedValue;

            if (courseName == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "EmptyName",
                    "Swal.fire({ icon: 'error', title: 'Course name cannot be empty!' });", true);
                return;
            }

            if (lecturerId == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "NoLecturer",
                    "Swal.fire({ icon: 'error', title: 'Please select a lecturer!' });", true);
                return;
            }

            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string updateQuery =
                    "UPDATE Course SET Course_Name = @Name, Lecturer_ID = @Lecturer WHERE Course_ID = @CID";

                SqlCommand cmd = new SqlCommand(updateQuery, conn);
                cmd.Parameters.AddWithValue("@Name", courseName);
                cmd.Parameters.AddWithValue("@Lecturer", lecturerId);
                cmd.Parameters.AddWithValue("@CID", courseId);

                cmd.ExecuteNonQuery();
            }

            ScriptManager.RegisterStartupScript(this, GetType(), "Updated",
                "Swal.fire({ icon: 'success', title: 'Course updated!', showConfirmButton:false, timer:1400 })" +
                ".then(()=>{ window.location='../Courses.aspx'; });", true);
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string courseId = lblCourseID.Text;

            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                // Delete student assignments first
                SqlCommand c1 = new SqlCommand(
                    "DELETE FROM Assigned_Course WHERE Course_ID = @CID", conn);
                c1.Parameters.AddWithValue("@CID", courseId);
                c1.ExecuteNonQuery();

                // Delete the course
                SqlCommand c2 = new SqlCommand(
                    "DELETE FROM Course WHERE Course_ID = @CID", conn);
                c2.Parameters.AddWithValue("@CID", courseId);
                c2.ExecuteNonQuery();
            }

            ScriptManager.RegisterStartupScript(this, GetType(), "Deleted",
                "Swal.fire({ icon:'success', title:'Course deleted!', timer:1500, showConfirmButton:false })" +
                ".then(()=>{ window.location='../Courses.aspx'; });", true);
        }
    }
}
