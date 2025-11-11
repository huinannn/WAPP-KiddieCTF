using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace WAPP_KiddieCTF.Student
{
    public partial class Courses : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string studentId = Session["StudentID"]?.ToString();
            if (string.IsNullOrEmpty(studentId))
            {
                Response.Redirect("/LogIn.aspx");
            }
            else
            {
                if (!IsPostBack)
                {
                    LoadStudentCourses();
                }
            }

            if (Request.Form["searchTextBox"] != null)
            {
                string searchText = Request.Form["searchTextBox"];
                LoadStudentCourses(searchText);
            }
        }

        protected void btnRecordClick_Click(object sender, EventArgs e)
        {
            string courseId = hdnCourseId.Value;
            string studentId = Session["StudentID"]?.ToString();

            if (!string.IsNullOrEmpty(studentId) && !string.IsNullOrEmpty(courseId))
            {
                RecordCourseClickToDatabase(studentId, courseId);

                Response.Redirect($"Chapter_Assignment.aspx?courseId={courseId}");
            }
        }

        private void RecordCourseClickToDatabase(string studentId, string courseId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string newId = GenerateNewAccessCourseId(connectionString);

                string query = @"
                    INSERT INTO Access_Course_Record (ACouR_ID, Student_ID, ACourR_Time, ACouR_Date, Course_ID)
                    VALUES (@ACouR_ID, @Student_ID, @ACourR_Time, @ACouR_Date, @Course_ID)";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ACouR_ID", newId);
                cmd.Parameters.AddWithValue("@Student_ID", studentId);
                cmd.Parameters.AddWithValue("@ACourR_Time", DateTime.Now.ToString("HH:mm:ss"));
                cmd.Parameters.AddWithValue("@ACouR_Date", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@Course_ID", courseId);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private string GenerateNewAccessCourseId(string connectionString)
        {
            string nextId = "AC001";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT MAX(ACouR_ID) FROM Access_Course_Record WHERE ACouR_ID LIKE 'AC%'";
                SqlCommand cmd = new SqlCommand(query, con);

                con.Open();
                object result = cmd.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    string maxId = result.ToString();
                    if (maxId.StartsWith("AC") && int.TryParse(maxId.Substring(2), out int num))
                    {
                        nextId = $"AC{(num + 1):D3}";
                    }
                }
            }

            return nextId;
        }

        private void LoadStudentCourses(string searchText = "")
        {
            string studentId = Session["StudentID"]?.ToString();
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT C.Course_ID, C.Course_Name, L.Lecturer_Name 
                    FROM Assigned_Course AC 
                    INNER JOIN Course C ON AC.Course_ID = C.Course_ID 
                    LEFT JOIN Lecturer L ON C.Lecturer_ID = L.Lecturer_ID 
                    WHERE AC.Student_ID = @Student_ID";

                if (!string.IsNullOrEmpty(searchText))
                {
                    query += " AND (C.Course_Name LIKE @SearchText OR L.Lecturer_Name LIKE @SearchText)";
                }

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Student_ID", studentId);

                if (!string.IsNullOrEmpty(searchText))
                {
                    cmd.Parameters.AddWithValue("@SearchText", "%" + searchText + "%");
                }

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    if (string.IsNullOrEmpty(searchText))
                    {
                        NoCoursesLabel.Visible = true;
                        NoSearchResultsLabel.Visible = false;
                    }
                    else
                    {
                        NoCoursesLabel.Visible = false;
                        NoSearchResultsLabel.Visible = true;
                    }
                    CoursesRepeater.Visible = false;
                }
                else
                {
                    NoCoursesLabel.Visible = false;
                    NoSearchResultsLabel.Visible = false;
                    CoursesRepeater.Visible = true;
                    CoursesRepeater.DataSource = dt;
                    CoursesRepeater.DataBind();
                }
            }
        }
    }
}