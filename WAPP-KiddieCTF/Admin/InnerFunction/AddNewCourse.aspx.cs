using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace WAPP_KiddieCTF.Admin.InnerFunction
{
    public partial class AddNewCourse : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GenerateNextCourseID(); // Generate new Course ID upon page load
                LoadLecturers();        // Load lecturers into the dropdown list
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

        private void LoadLecturers()
        {
            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT Lecturer_ID, Lecturer_Name FROM Lecturer";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                ddlLecturer.DataSource = reader;
                ddlLecturer.DataTextField = "Lecturer_Name";
                ddlLecturer.DataValueField = "Lecturer_ID";
                ddlLecturer.DataBind();
                ddlLecturer.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select Lecturer", ""));
            }
        }

        protected void btnDone_Click(object sender, EventArgs e)
        {
            string courseId = lblCourseID.Text.Trim();
            string courseName = txtCourseName.Text.Trim();
            string lecturerId = ddlLecturer.SelectedValue.Trim();

            if (string.IsNullOrEmpty(courseName))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "EmptyCourse",
                    "Swal.fire({ icon: 'error', title: 'Please enter course name!', confirmButtonColor: '#3085d6' });", true);
                return;
            }

            if (string.IsNullOrEmpty(lecturerId))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "EmptyLecturer",
                    "Swal.fire({ icon: 'error', title: 'Please assign a lecturer!', confirmButtonColor: '#3085d6' });", true);
                return;
            }

            // Insert the course and lecturer assignment
            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                string insertCourse = "INSERT INTO Course (Course_ID, Course_Name, Lecturer_ID) " +
                                      "VALUES (@ID, @Name, @Lecturer)";
                SqlCommand cmd = new SqlCommand(insertCourse, conn);
                cmd.Parameters.AddWithValue("@ID", courseId);
                cmd.Parameters.AddWithValue("@Name", courseName);
                cmd.Parameters.AddWithValue("@Lecturer", lecturerId);
                cmd.ExecuteNonQuery();
            }

            // Success message
            ScriptManager.RegisterStartupScript(this, GetType(), "Success",
                "Swal.fire({ icon: 'success', title: 'Course created successfully!', showConfirmButton:false, timer:1500 }).then(()=>{window.location='../Courses.aspx';});",
                true);
        }
    }
}
