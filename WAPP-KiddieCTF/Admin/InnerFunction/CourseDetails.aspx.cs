using System;
using System.Data;
using System.Data.SqlClient;

namespace WAPP_KiddieCTF.Admin.InnerFunction
{
    public partial class CourseDetails : System.Web.UI.Page
    {
        string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        protected string CurrentCourseID;

        protected void Page_Load(object sender, EventArgs e)
        {
            CurrentCourseID = Request.QueryString["Course_ID"];

            if (!IsPostBack)
            {
                if (string.IsNullOrEmpty(CurrentCourseID))
                {
                    Response.Redirect("../Courses.aspx");
                }

                LoadCourseDetails();
                LoadAssignedLecturer();
                LoadChapters();
                LoadFinalAssignment();
            }
        }

        private void LoadCourseDetails()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT Course_Name FROM Course WHERE Course_ID = @CourseID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CourseID", CurrentCourseID);

                conn.Open();
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    lblCourseName.Text = result.ToString() + " (" + CurrentCourseID + ")";
                }
                else
                {
                    lblCourseName.Text = "Unknown Course";
                }
            }
        }

        private void LoadAssignedLecturer()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT L.Lecturer_Name FROM Lecturer L " +
                               "INNER JOIN Course C ON L.Lecturer_ID = C.Lecturer_ID " +
                               "WHERE C.Course_ID = @CourseID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CourseID", CurrentCourseID);

                conn.Open();
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    lblAssignedLecturer.Text = result.ToString();
                }
                else
                {
                    lblAssignedLecturer.Text = "No lecturer assigned";
                }
            }
        }

        private void LoadChapters()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT Chapter_ID, Chapter_Name FROM Chapter WHERE Course_ID = @CourseID ORDER BY Chapter_ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CourseID", CurrentCourseID);

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                rptChapters.DataSource = dt;
                rptChapters.DataBind();

                bool hasChapters = dt.Rows.Count > 0;
                litNoChapters.Visible = !hasChapters;
                btnAddChapter.Visible = true;
            }
        }

        private void LoadFinalAssignment()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT FA_ID, FA_Name FROM Final_Assignment WHERE Course_ID = @CourseID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CourseID", CurrentCourseID);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                rptAssignment.DataSource = dt;
                rptAssignment.DataBind();

                litNoAssignment.Visible = dt.Rows.Count == 0;
            }
        }

        protected void btnAddChapter_Click(object sender, EventArgs e)
        {
            Response.Redirect($"AddChapter.aspx?Course_ID={CurrentCourseID}");
        }

        protected void btnAddAssignment_Click(object sender, EventArgs e)
        {
            Response.Redirect($"AddAssignment.aspx?Course_ID={CurrentCourseID}");
        }

        protected void btnViewProgress_Click(object sender, EventArgs e)
        {
            Response.Redirect($"ChapterProgress.aspx?Course_ID={CurrentCourseID}");
        }

        protected void btnViewAssignProgress_Click(object sender, EventArgs e)
        {
            Response.Redirect($"AssignmentProgress.aspx?Course_ID={CurrentCourseID}");
        }
    }
}
