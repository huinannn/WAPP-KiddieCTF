using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WAPP_KiddieCTF.Lecturer
{
    public partial class Dashboard : System.Web.UI.Page
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
                LoadDashboardData();
            }

        }

        private void LoadDashboardData()
        {
            string lecturerId = Session["LecturerID"].ToString();
            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                // Recent Courses (latest 2)
                string queryRecent = @"
                    SELECT TOP 2 Course_Name, Course_ID
                    FROM Course 
                    WHERE Lecturer_ID = @Lecturer_ID 
                    ORDER BY Course_ID DESC";
                SqlDataAdapter daRecent = new SqlDataAdapter(queryRecent, conn);
                daRecent.SelectCommand.Parameters.AddWithValue("@Lecturer_ID", lecturerId);
                DataTable dtRecent = new DataTable();
                daRecent.Fill(dtRecent);
                rptRecentCourses.DataSource = dtRecent;
                rptRecentCourses.DataBind();

                // Popular Courses (most accessed)
                string queryPopular = @"
                    SELECT TOP 3 C.Course_Name, COUNT(A.Course_ID) AS AccessCount
                    FROM Access_Course_Record A
                    INNER JOIN Course C ON A.Course_ID = C.Course_ID
                    GROUP BY C.Course_Name
                    ORDER BY COUNT(A.Course_ID) DESC";
                SqlDataAdapter daPopular = new SqlDataAdapter(queryPopular, conn);
                DataTable dtPopular = new DataTable();
                daPopular.Fill(dtPopular);
                rptPopularCourses.DataSource = dtPopular;
                rptPopularCourses.DataBind();

                // Total Courses
                string queryTotalCourses = "SELECT COUNT(*) FROM Course WHERE Lecturer_ID = @Lecturer_ID";
                SqlCommand cmdCourse = new SqlCommand(queryTotalCourses, conn);
                cmdCourse.Parameters.AddWithValue("@Lecturer_ID", lecturerId);
                lblTotalCourses.Text = cmdCourse.ExecuteScalar().ToString();

                // Total Challenges
                string queryTotalChallenges = "SELECT COUNT(*) FROM Challenge WHERE Lecturer_ID = @Lecturer_ID";
                SqlCommand cmdChallenge = new SqlCommand(queryTotalChallenges, conn);
                cmdChallenge.Parameters.AddWithValue("@Lecturer_ID", lecturerId);
                lblTotalChallenges.Text = cmdChallenge.ExecuteScalar().ToString();
            }
        }

    }
}