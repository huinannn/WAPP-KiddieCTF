using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI.WebControls;

namespace WAPP_KiddieCTF.Student
{
    public partial class Chapter_Assignment : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string studentId = Session["StudentID"]?.ToString();
            string courseId = Request.QueryString["courseId"];

            if (string.IsNullOrEmpty(studentId) || string.IsNullOrEmpty(courseId))
            {
                Response.Redirect("/Default.aspx");
            }
            else
            {
                if (!IsPostBack)
                {
                    LoadCourseDetails(courseId);
                    LoadChapters(courseId, studentId);
                    LoadFinalAssignment(courseId);
                }
            }
        }

        private void LoadCourseDetails(string courseId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT Course_Name, Course_ID FROM Course WHERE Course_ID = @Course_ID";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Course_ID", courseId);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    string courseName = reader["Course_Name"].ToString();
                    string courseID = reader["Course_ID"].ToString();

                    coursesTitle.Text = $"{courseName} ({courseID})";
                }
                else
                {
                    coursesTitle.Text = "Course not found!";
                }

                reader.Close();
                con.Close();
            }
        }

        private void LoadChapters(string courseId, string studentId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"
                SELECT Chapter_ID, Chapter_Name, Chapter_File
                FROM Chapter
                WHERE Course_ID = @Course_ID";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Course_ID", courseId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    ChaptersRepeater.Visible = false;
                    noChaptersMessage.Visible = true;
                }
                else
                {
                    ChaptersRepeater.Visible = true;
                    noChaptersMessage.Visible = false;

                    ChaptersRepeater.DataSource = dt;
                    ChaptersRepeater.DataBind();

                    foreach (RepeaterItem item in ChaptersRepeater.Items)
                    {
                        LinkButton btnMarkDone = (LinkButton)item.FindControl("btnMarkDone");
                        string chapterId = btnMarkDone.CommandArgument;

                        if (IsChapterMarkedAsDone(studentId, chapterId))
                        {
                            btnMarkDone.Text = "Done";
                            btnMarkDone.Enabled = false;
                            btnMarkDone.CssClass = "mark-done-btn done";
                        }
                        else
                        {
                            btnMarkDone.Text = "Mark as Done";
                            btnMarkDone.Enabled = true;
                            btnMarkDone.CssClass = "mark-done-btn";
                        }
                    }
                }
            }
        }

        private bool IsChapterMarkedAsDone(string studentId, string chapterId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT COUNT(*) 
                    FROM Progress_Checker 
                    WHERE Student_ID = @Student_ID 
                    AND Chapter_ID = @Chapter_ID";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Student_ID", studentId);
                cmd.Parameters.AddWithValue("@Chapter_ID", chapterId);

                con.Open();
                int count = (int)cmd.ExecuteScalar();

                return count > 0;
            }
        }

        protected void DownloadChapter(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            string fileName = btn.CommandArgument;

            if (!string.IsNullOrEmpty(fileName))
            {
                string filePath = Server.MapPath("/Uploads/Chapters/") + fileName;

                if (File.Exists(filePath))
                {
                    Response.Clear();
                    Response.ContentType = "application/octet-stream";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
                    Response.TransmitFile(filePath);
                    Response.End();
                }
            }
        }

        private void LoadFinalAssignment(string courseId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"
            SELECT FA_ID, FA_Name, FA_File, FA_Deadline
            FROM Final_Assignment
            WHERE Course_ID = @Course_ID";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Course_ID", courseId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    FinalAssignmentLink.Text = dt.Rows[0]["FA_Name"].ToString();
                    FinalAssignmentLink.Visible = true;
                    noFinalAssignmentMessage.Visible = false;
                }
                else
                {
                    FinalAssignmentLink.Visible = false;
                    noFinalAssignmentMessage.Visible = true;
                }
            }
        }

        protected void RedirectToFinalAssignment(object sender, EventArgs e)
        {
            string courseId = Request.QueryString["courseId"];
            if (!string.IsNullOrEmpty(courseId))
            {
                Response.Redirect($"FinalAssignment.aspx?courseId={courseId}");
            }
        }

        protected void MarkChapterAsDoneButton_Click(object sender, EventArgs e)
        {
            string studentId = Session["StudentID"]?.ToString();
            LinkButton btn = (LinkButton)sender;
            string chapterId = btn.CommandArgument;

            if (!string.IsNullOrEmpty(studentId) && !string.IsNullOrEmpty(chapterId))
            {
                bool result = MarkChapterAsDone(studentId, chapterId);

                if (result)
                {
                    btn.Text = "Done";
                    btn.CssClass = "mark-done-btn done";
                }
            }
        }

        public bool MarkChapterAsDone(string studentId, string chapterId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            try
            {
                string checkId = GenerateNewCheckId();

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = @"
                        INSERT INTO Progress_Checker (Check_ID, Student_ID, Chapter_ID)
                        VALUES (@Check_ID, @Student_ID, @Chapter_ID)";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Check_ID", checkId);
                    cmd.Parameters.AddWithValue("@Student_ID", studentId);
                    cmd.Parameters.AddWithValue("@Chapter_ID", chapterId);

                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error marking chapter as done: " + ex.Message);
                return false;
            }
        }

        private static string GenerateNewCheckId()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT TOP 1 Check_ID FROM Progress_Checker ORDER BY Check_ID DESC";

                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    string lastId = reader["Check_ID"].ToString();
                    int nextId = int.Parse(lastId.Substring(2)) + 1;
                    return "CK" + nextId.ToString("D3");
                }
                else
                {
                    return "CK001";
                }
            }
        }
    }
}