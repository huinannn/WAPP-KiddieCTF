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
                Response.Redirect("../LogIn.aspx");
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
            string courseId = lblCourseID.Text.Trim();
            string courseName = txtCourseName.Text.Trim();

            if (string.IsNullOrEmpty(courseName))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "EmptyCourseName",
                    "Swal.fire({ icon: 'error', title: 'Course name cannot be empty!', confirmButtonColor: '#3085d6' });", true);
                return;
            }

            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                // Update course name
                string updateQuery = "UPDATE Course SET Course_Name = @Name WHERE Course_ID = @CID";
                SqlCommand updateCmd = new SqlCommand(updateQuery, conn);
                updateCmd.Parameters.AddWithValue("@Name", courseName);
                updateCmd.Parameters.AddWithValue("@CID", courseId);
                updateCmd.ExecuteNonQuery();

                // Insert ONLY new students (avoid duplicates)
                List<string> tempEdit = Session["TempEditStudents"] as List<string>;
                if (tempEdit != null && tempEdit.Count > 0)
                {
                    // Get existing student IDs for this course
                    List<string> existing = new List<string>();
                    string selectExisting = "SELECT Student_ID FROM Assigned_Course WHERE Course_ID = @CID";
                    SqlCommand existingCmd = new SqlCommand(selectExisting, conn);
                    existingCmd.Parameters.AddWithValue("@CID", courseId);
                    SqlDataReader reader = existingCmd.ExecuteReader();
                    while (reader.Read())
                        existing.Add(reader["Student_ID"].ToString());
                    reader.Close();

                    // Insert only those not already assigned
                    foreach (string studentId in tempEdit)
                    {
                        if (!existing.Contains(studentId))
                        {
                            string maxQuery = "SELECT ISNULL(MAX(CAST(SUBSTRING(AC_ID, 3, 10) AS INT)), 0) FROM Assigned_Course";
                            SqlCommand maxCmd = new SqlCommand(maxQuery, conn);
                            int nextId = (int)maxCmd.ExecuteScalar() + 1;
                            string newAcId = "AC" + nextId.ToString("D3");

                            string insertQuery = "INSERT INTO Assigned_Course (AC_ID, Course_ID, Student_ID) VALUES (@ACID, @CID, @SID)";
                            SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                            insertCmd.Parameters.AddWithValue("@ACID", newAcId);
                            insertCmd.Parameters.AddWithValue("@CID", courseId);
                            insertCmd.Parameters.AddWithValue("@SID", studentId);
                            insertCmd.ExecuteNonQuery();
                        }
                    }
                }
            }

            // Clear session after saving
            Session.Remove("TempEditStudents");

            // Success message
            ScriptManager.RegisterStartupScript(this, GetType(), "Updated",
                "Swal.fire({ icon: 'success', title: 'Course updated successfully!', showConfirmButton:false, timer:1500 }).then(()=>{window.location='Courses.aspx';});", true);
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
            string courseId = lblCourseID.Text.Trim();

            // Load currently assigned students into session if not done yet
            if (Session["TempEditStudents"] == null)
            {
                List<string> assigned = new List<string>();
                string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string query = "SELECT Student_ID FROM Assigned_Course WHERE Course_ID = @CID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@CID", courseId);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                        assigned.Add(reader["Student_ID"].ToString());
                }

                Session["TempEditStudents"] = assigned;
            }

            // Redirect to AddStudent.aspx in “edit mode”
            Response.Redirect($"AddStudent.aspx?course={lblCourseID.Text}&from=edit");
        }

        protected void btnViewStudents_Click(object sender, EventArgs e)
        {
            Response.Redirect($"StudentList.aspx?course={lblCourseID.Text}&from=edit"); 
        }

    }
}