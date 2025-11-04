using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WAPP_KiddieCTF.Lecturer
{
    public partial class AddChapter : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        protected string CourseID;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CourseID = Request.QueryString["courseid"];
                if (string.IsNullOrEmpty(CourseID))
                    Response.Redirect("Courses.aspx");

                GenerateNextChapterID();
                Session["ReturnCourseID"] = CourseID;
            }
        }

        private void GenerateNextChapterID()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT MAX(CAST(SUBSTRING(Chapter_ID, 3, LEN(Chapter_ID)) AS INT)) FROM Chapter WHERE Chapter_ID LIKE 'CP%'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                object result = cmd.ExecuteScalar();
                int nextNum = (result == DBNull.Value) ? 1 : Convert.ToInt32(result) + 1;
                lblChapterID.Text = "CP" + nextNum.ToString("D3"); // CP001, CP002...
            }
        }

        protected void btnDone_Click(object sender, EventArgs e)
        {
            string chapterName = txtChapterName.Text.Trim();
            if (string.IsNullOrEmpty(chapterName))
            {
                ShowAlert("error", "Chapter name cannot be empty!");
                return;
            }

            if (!fuChapterFile.HasFile)
            {
                ShowAlert("error", "Please attach a file!");
                return;
            }

            // Validate file type
            string ext = Path.GetExtension(fuChapterFile.FileName).ToLower();
            if (ext != ".pdf" && ext != ".docx" && ext != ".pptx")
            {
                ShowAlert("error", "Only PDF, DOCX, PPTX files are allowed!");
                return;
            }

            // Save file
            string fileName = lblChapterID.Text + ext;
            string filePath = Server.MapPath("~/Uploads/Chapters/") + fileName;
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            fuChapterFile.SaveAs(filePath);

            // Insert into DB
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "INSERT INTO Chapter (Chapter_ID, Chapter_Name, Chapter_File, Course_ID) VALUES (@ID, @Name, @File, @CourseID)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", lblChapterID.Text);
                cmd.Parameters.AddWithValue("@Name", chapterName);
                cmd.Parameters.AddWithValue("@File", fileName);
                cmd.Parameters.AddWithValue("@CourseID", Session["ReturnCourseID"]);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            // Success
            string script = @"
                Swal.fire({
                    icon: 'success',
                    title: 'Chapter added successfully!',
                    showConfirmButton: false,
                    timer: 1500
                }).then(() => {
                    window.location.replace('CourseDetails.aspx?id=" + Session["ReturnCourseID"] + @"');
                });
            ";
            ScriptManager.RegisterStartupScript(this, GetType(), "AddChapterSuccess", script, true);
        }

        private void ShowAlert(string icon, string title)
        {
            string script = $@"Swal.fire({{ icon: '{icon}', title: '{title}', confirmButtonColor: '#3085d6' }});";
            ScriptManager.RegisterStartupScript(this, GetType(), icon, script, true);
        }

    }
}