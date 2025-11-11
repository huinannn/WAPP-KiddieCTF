using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace WAPP_KiddieCTF.Admin.InnerFunction
{
    public partial class CourseDetails : System.Web.UI.Page
    {
        private readonly string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        protected string CurrentCourseID;

        protected void Page_Load(object sender, EventArgs e)
        {
            // admin version: no session check
            CurrentCourseID = Request.QueryString["id"];

            if (!IsPostBack)
            {
                if (string.IsNullOrEmpty(CurrentCourseID))
                {
                    Response.Redirect("../Courses.aspx");
                    return;
                }

                LoadCourseDetails();
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

        private void LoadChapters()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT Chapter_ID, Chapter_Name FROM Chapter WHERE Course_ID = @CourseID ORDER BY Chapter_ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CourseID", CurrentCourseID);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                rptChapters.DataSource = dt;
                rptChapters.DataBind();

                litNoChapters.Visible = dt.Rows.Count == 0;
            }
        }

        private void LoadFinalAssignment()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT FA_ID, FA_Name FROM Final_Assignment WHERE Course_ID = @CourseID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CourseID", CurrentCourseID);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                rptAssignment.DataSource = dt;
                rptAssignment.DataBind();

                litNoAssignment.Visible = dt.Rows.Count == 0;
            }
        }

        protected void btnAddChapter_Click(object sender, EventArgs e)
        {
            Response.Redirect("AddChapter.aspx?courseid=" + CurrentCourseID);
        }

        protected void btnAddAssignment_Click(object sender, EventArgs e)
        {
            Response.Redirect("AddAssignment.aspx?courseid=" + CurrentCourseID);
        }

        protected void btnViewProgress_Click(object sender, EventArgs e)
        {
            Response.Redirect("ChapterProgress.aspx?courseid=" + CurrentCourseID);
        }

        protected void btnViewAssignProgress_Click(object sender, EventArgs e)
        {
            Response.Redirect("AssignmentProgress.aspx?courseid=" + CurrentCourseID);
        }
    }
}
