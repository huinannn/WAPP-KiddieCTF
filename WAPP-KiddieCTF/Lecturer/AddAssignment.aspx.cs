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
    public partial class AddAssignment : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        protected string CourseID;

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
                CourseID = Request.QueryString["courseid"];
                if (string.IsNullOrEmpty(CourseID))
                    Response.Redirect("Courses.aspx");

                GenerateNextFAID();
                Session["ReturnCourseID"] = CourseID;
            }
        }

            private void GenerateNextFAID()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT MAX(CAST(SUBSTRING(FA_ID, 3, LEN(FA_ID)) AS INT)) FROM Final_Assignment WHERE FA_ID LIKE 'FA%'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                object result = cmd.ExecuteScalar();
                int next = (result == DBNull.Value) ? 1 : Convert.ToInt32(result) + 1;
                lblFAID.Text = "FA" + next.ToString("D3"); // FA001, FA002...
            }
        }

        protected void btnDone_Click(object sender, EventArgs e)
        {
            string name = txtFAName.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                ShowAlert("error", "Assignment name cannot be empty!");
                return;
            }

            if (!fuFile.HasFile)
            {
                ShowAlert("error", "Please attach a file!");
                return;
            }

            string ext = Path.GetExtension(fuFile.FileName).ToLower();
            if (ext != ".pdf" && ext != ".docx" && ext != ".zip")
            {
                ShowAlert("error", "Only PDF, DOCX, ZIP allowed!");
                return;
            }

            if (string.IsNullOrEmpty(txtDeadline.Text))
            {
                ShowAlert("error", "Please select a deadline!");
                return;
            }

            DateTime deadline;
            if (!DateTime.TryParse(txtDeadline.Text, out deadline))
            {
                ShowAlert("error", "Invalid deadline!");
                return;
            }

            // Save file
            string fileName = lblFAID.Text + ext;
            string path = Server.MapPath("~/Uploads/Assignments/") + fileName;
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            fuFile.SaveAs(path);

            // Insert DB
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"INSERT INTO Final_Assignment 
                    (FA_ID, FA_Name, FA_File, FA_Deadline, Course_ID) 
                    VALUES (@ID, @Name, @File, @Deadline, @CourseID)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", lblFAID.Text);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@File", fileName);
                cmd.Parameters.AddWithValue("@Deadline", deadline);
                cmd.Parameters.AddWithValue("@CourseID", Session["ReturnCourseID"]);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            // Success
            string script = $@"
                Swal.fire({{
                    icon: 'success',
                    title: 'Assignment added!',
                    showConfirmButton: false,
                    timer: 1500
                }}).then(() => {{
                    window.location = 'CourseDetails.aspx?id={Session["ReturnCourseID"]}';
                }});
            ";
            ScriptManager.RegisterStartupScript(this, GetType(), "success", script, true);
        }

        private void ShowAlert(string icon, string title)
        {
            string script = $"Swal.fire({{ icon: '{icon}', title: '{title}', confirmButtonColor: '#3085d6' }});";
            ScriptManager.RegisterStartupScript(this, GetType(), icon, script, true);
        }

    }
    }
