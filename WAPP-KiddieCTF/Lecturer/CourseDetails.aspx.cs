using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WAPP_KiddieCTF.Lecturer
{
    public partial class CourseDetails : System.Web.UI.Page
    {
        string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        protected string CurrentCourseID;

        protected void Page_Load(object sender, EventArgs e)
        {
            CurrentCourseID = Request.QueryString["id"];

            if (!IsPostBack)
            {
                if (string.IsNullOrEmpty(CurrentCourseID))
                {
                    Response.Redirect("Courses.aspx");
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
                string query = "SELECT Chapter_Name FROM Chapter WHERE Course_ID = @CourseID ORDER BY Chapter_ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CourseID", CurrentCourseID);

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                rptChapters.DataSource = dt;
                rptChapters.DataBind();

                bool hasChapters = dt.Rows.Count > 0;
                lnkEditChapters.Visible = hasChapters;
                litNoChapters.Visible = !hasChapters;
                btnAddChapter.Visible = true;
            }
        }

        private void LoadFinalAssignment()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT FA_Name FROM Final_Assignment WHERE Course_ID = @CourseID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CourseID", CurrentCourseID);

                conn.Open();
                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    lblAssignment.Text = "Assignment: " + result.ToString();
                    lnkEditAssignment.Visible = true;
                    litNoAssignment.Visible = false;
                }
                else
                {
                    lblAssignment.Text = "";
                    lnkEditAssignment.Visible = false;
                    litNoAssignment.Visible = true;
                }
            }
        }

        protected void btnBack_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Courses.aspx");
        }

        protected void btnAddChapter_Click(object sender, EventArgs e)
        {
            Response.Redirect($"AddChapter.aspx?courseid={CurrentCourseID}");
        }

        protected void btnAddAssignment_Click(object sender, EventArgs e)
        {
            Response.Redirect($"AddFinalAssignment.aspx?courseid={CurrentCourseID}");
        }

        protected void lnkEditChapters_Click(object sender, EventArgs e)
        {
            Response.Redirect($"EditChapters.aspx?courseid={CurrentCourseID}");
        }

        protected void lnkEditAssignment_Click(object sender, EventArgs e)
        {
            Response.Redirect($"EditFinalAssignment.aspx?courseid={CurrentCourseID}");
        }

        protected void btnViewProgress_Click(object sender, EventArgs e)
        {
            Response.Redirect($"StudentChapterProgress.aspx?courseid={CurrentCourseID}");
        }

        protected void btnViewAssignProgress_Click(object sender, EventArgs e)
        {
            Response.Redirect($"StudentAssignmentProgress.aspx?courseid={CurrentCourseID}");
        }

    }
}