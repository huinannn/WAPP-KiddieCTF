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
    public partial class EditAssignment : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        protected string FAID, CourseID;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LecturerID"] == null || Session["LecturerName"] == null)
            {
                Response.Redirect("../Default.aspx");
                return;
            }

            lblLecturerID.Text = Session["LecturerID"].ToString();
            lblLecturerName.Text = Session["LecturerName"].ToString();

            if (!IsPostBack)
            {
                FAID = Request.QueryString["faid"];
                CourseID = Request.QueryString["courseid"];

                if (string.IsNullOrEmpty(FAID) || string.IsNullOrEmpty(CourseID))
                    Response.Redirect("Courses.aspx");

                ViewState["FAID"] = FAID;
                ViewState["CourseID"] = CourseID;

                LoadAssignment();
            }
            else
            {
                FAID = ViewState["FAID"]?.ToString();
                CourseID = ViewState["CourseID"]?.ToString();
            }
        }

        private void LoadAssignment()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT FA_Name, FA_File, FA_Deadline FROM Final_Assignment WHERE FA_ID = @ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", FAID);
                conn.Open();

                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    if (r.Read())
                    {
                        lblFAID.Text = FAID;
                        txtFAName.Text = r["FA_Name"].ToString();
                        lblFileName.Text = r["FA_File"].ToString();
                        txtDeadline.Text = Convert.ToDateTime(r["FA_Deadline"]).ToString("yyyy-MM-dd");
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
            string name = txtFAName.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                ShowAlert("error", "Assignment name required!");
                return;
            }

            if (string.IsNullOrEmpty(txtDeadline.Text))
            {
                ShowAlert("error", "Deadline required!");
                return;
            }

            DateTime deadline;
            if (!DateTime.TryParse(txtDeadline.Text, out deadline))
            {
                ShowAlert("error", "Invalid date!");
                return;
            }

            string fileName = lblFileName.Text;
            if (fuFile.HasFile)
            {
                string ext = Path.GetExtension(fuFile.FileName).ToLower();
                if (ext != ".pdf" && ext != ".docx" && ext != ".zip")
                {
                    ShowAlert("error", "Only PDF, DOCX, ZIP allowed!");
                    return;
                }
                fileName = FAID + ext;
                string path = Server.MapPath("~/Uploads/Assignments/") + fileName;
                fuFile.SaveAs(path);
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "UPDATE Final_Assignment SET FA_Name = @Name, FA_File = @File, FA_Deadline = @Deadline WHERE FA_ID = @ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@File", fileName);
                cmd.Parameters.AddWithValue("@Deadline", deadline);
                cmd.Parameters.AddWithValue("@ID", FAID);
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            ShowSuccess("Assignment updated!", $"CourseDetails.aspx?id={CourseID}");
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string oldFile = Server.MapPath("~/Uploads/Assignments/") + lblFileName.Text;
            if (File.Exists(oldFile)) File.Delete(oldFile);

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "DELETE FROM Final_Assignment WHERE FA_ID = @ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", FAID);
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            ShowSuccess("Assignment deleted!", $"CourseDetails.aspx?id={CourseID}");
        }

        private void ShowAlert(string icon, string title)
        {
            string script = $"Swal.fire({{ icon: '{icon}', title: '{title}', confirmButtonColor: '#3085d6' }});";
            ScriptManager.RegisterStartupScript(this, GetType(), icon, script, true);
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