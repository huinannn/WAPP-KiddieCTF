using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace WAPP_KiddieCTF.Admin.InnerFunction
{
    public partial class AddNewCourse : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // ADMIN: no lecturer session check
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
                lblCourseID.Text = "CR" + nextId.ToString("D3");
            }
        }

        protected void btnDone_Click(object sender, EventArgs e)
        {
            string courseId = lblCourseID.Text.Trim();
            string courseName = txtCourseName.Text.Trim();

            if (string.IsNullOrEmpty(courseName))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "EmptyCourse",
                    "Swal.fire({ icon: 'error', title: 'Please enter course name!', confirmButtonColor: '#3085d6' });", true);
                return;
            }

            // we still use Session to temporarily hold student IDs
            List<string> tempStudents = Session["TempStudents"] as List<string>;

            if (tempStudents == null || tempStudents.Count == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "NoStudents",
                    "Swal.fire({ icon: 'error', title: 'Please add at least one student before completing!', confirmButtonColor: '#3085d6' });", true);
                return;
            }

            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                // insert course
                // NOTE: original table had Lecturer_ID.
                // If your Course table requires Lecturer_ID (NOT NULL), add a default or pick one.
                string insertCourse = "INSERT INTO Course (Course_ID, Course_Name) VALUES (@ID, @Name)";
                SqlCommand cmd = new SqlCommand(insertCourse, conn);
                cmd.Parameters.AddWithValue("@ID", courseId);
                cmd.Parameters.AddWithValue("@Name", courseName);
                cmd.ExecuteNonQuery();

                // insert students
                foreach (string studentId in tempStudents)
                {
                    string maxQuery = "SELECT ISNULL(MAX(CAST(SUBSTRING(AC_ID, 3, 10) AS INT)), 0) FROM Assigned_Course";
                    SqlCommand maxCmd = new SqlCommand(maxQuery, conn);
                    int nextId = (int)maxCmd.ExecuteScalar() + 1;
                    string newAcId = "AC" + nextId.ToString("D3");

                    string insertAssigned = "INSERT INTO Assigned_Course (AC_ID, Course_ID, Student_ID) VALUES (@ACID, @CID, @SID)";
                    SqlCommand insertCmd = new SqlCommand(insertAssigned, conn);
                    insertCmd.Parameters.AddWithValue("@ACID", newAcId);
                    insertCmd.Parameters.AddWithValue("@CID", courseId);
                    insertCmd.Parameters.AddWithValue("@SID", studentId);
                    insertCmd.ExecuteNonQuery();
                }
            }

            // clear temp
            Session.Remove("TempCourseID");
            Session.Remove("TempCourseName");
            Session.Remove("TempStudents");

            // success
            ScriptManager.RegisterStartupScript(this, GetType(), "Success",
                "Swal.fire({ icon: 'success', title: 'Course created successfully!', showConfirmButton:false, timer:1500 }).then(()=>{window.location='../Courses.aspx';});", true);
        }

        protected void btnAddStudents_Click(object sender, EventArgs e)
        {
            string courseId = lblCourseID.Text.Trim();
            string courseName = txtCourseName.Text.Trim();

            if (string.IsNullOrEmpty(courseName))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "MissingCourse",
                    "Swal.fire({ icon: 'error', title: 'Please enter the course name first!', confirmButtonColor: '#3085d6' });", true);
                return;
            }

            // store temp course info
            Session["TempCourseID"] = courseId;
            Session["TempCourseName"] = courseName;
            Session["TempStudents"] = new List<string>(); // reset

            Response.Redirect("AddStudent.aspx?course=" + courseId + "&from=add");
        }

        protected void btnViewStudents_Click(object sender, EventArgs e)
        {
            Response.Redirect("StudentList.aspx?course=" + lblCourseID.Text);
        }
    }
}
