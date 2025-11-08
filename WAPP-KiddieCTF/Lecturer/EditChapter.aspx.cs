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
    public partial class EditChapter : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;


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
                string chapterID = Request.QueryString["chapterid"];
                string courseID = Request.QueryString["courseid"];

                if (string.IsNullOrEmpty(chapterID) || string.IsNullOrEmpty(courseID))
                    Response.Redirect("Courses.aspx");

                ViewState["ChapterID"] = chapterID;
                ViewState["CourseID"] = courseID;

                LoadChapter(chapterID);
            }
        }

        private void LoadChapter(string chapterID)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT Chapter_Name, Chapter_File FROM Chapter WHERE Chapter_ID = @ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", chapterID);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        lblChapterID.Text = chapterID;
                        txtChapterName.Text = reader["Chapter_Name"].ToString();
                        lblFileName.Text = reader["Chapter_File"].ToString();
                    }
                    else
                    {
                        Response.Redirect("Courses.aspx");
                    }
                }
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            // Retrieve the IDs from ViewState on postback
            string chapterID = ViewState["ChapterID"]?.ToString();
            string courseID = ViewState["CourseID"]?.ToString();

            string newName = txtChapterName.Text.Trim();
            if (string.IsNullOrEmpty(newName))
            {
                ShowAlert("error", "Chapter name cannot be empty!");
                return;
            }

            string fileName = lblFileName.Text;
            if (fuChapterFile.HasFile)
            {
                string ext = Path.GetExtension(fuChapterFile.FileName).ToLower();
                if (ext != ".pdf" && ext != ".docx" && ext != ".pptx")
                {
                    ShowAlert("error", "Only PDF, DOCX, PPTX allowed!");
                    return;
                }

                fileName = chapterID + ext;
                string path = Server.MapPath("~/Uploads/Chapters/") + fileName;
                fuChapterFile.SaveAs(path);
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "UPDATE Chapter SET Chapter_Name = @Name, Chapter_File = @File WHERE Chapter_ID = @ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", newName);
                cmd.Parameters.AddWithValue("@File", fileName);
                cmd.Parameters.AddWithValue("@ID", chapterID); // ✅ now always has value
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            ShowSuccess("Chapter updated!", $"CourseDetails.aspx?id={courseID}");
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string chapterID = ViewState["ChapterID"]?.ToString();
            string courseID = ViewState["CourseID"]?.ToString();

            string oldFile = Server.MapPath("~/Uploads/Chapters/") + lblFileName.Text;
            if (File.Exists(oldFile)) File.Delete(oldFile);

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "DELETE FROM Chapter WHERE Chapter_ID = @ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", chapterID);
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            ShowSuccess("Chapter deleted!", $"CourseDetails.aspx?id={courseID}");
        }

        private void ShowAlert(string icon, string title)
        {
            string script = $"Swal.fire({{ icon: '{icon}', title: '{title}', confirmButtonColor: '#3085d6' }});";
            ScriptManager.RegisterStartupScript(this, GetType(), "alert", script, true);
        }

        private void ShowSuccess(string title, string url)
        {
            string script = $@"
                Swal.fire({{
                    icon: 'success',
                    title: '{title}',
                    showConfirmButton: false,
                    timer: 1500
                }}).then(() => {{ window.location = '{url}'; }});
            ";
            ScriptManager.RegisterStartupScript(this, GetType(), "success", script, true);
        }

    }
}