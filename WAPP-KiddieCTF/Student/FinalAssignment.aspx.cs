using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace WAPP_KiddieCTF.Student
{
    public partial class FinalAssignment : Page
    {
        private string courseId;

        protected string FaId
        {
            get { return ViewState["FaId"] as string; }
            set { ViewState["FaId"] = value; }
        }

        protected bool HasSubmitted
        {
            get { return ViewState["HasSubmitted"] as bool? ?? false; }
            set { ViewState["HasSubmitted"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string studentId = Session["StudentID"]?.ToString();
            courseId = Request.QueryString["courseId"];

            if (string.IsNullOrEmpty(studentId) || string.IsNullOrEmpty(courseId))
            {
                Response.Redirect("/Default.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadFinalAssignment(courseId);
                CheckExistingSubmission(studentId);
            }

            if (HasSubmitted)
            {
                // If already submitted, disable upload
                DisableUpload("submitted");
            }

            string script = $@"
                document.getElementById('backButton').href = 'Chapter_Assignment.aspx?courseId={courseId}';
            ";
            ScriptManager.RegisterStartupScript(this, GetType(), "setBackButtonHref", script, true);

            // Load marking status
            LoadMarkingStatus(courseId, studentId);
        }

        private void LoadFinalAssignment(string courseId)
        {
            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connStr))
            {
                const string sql = @"
                    SELECT FA_ID, FA_Name, FA_File, FA_Deadline
                    FROM Final_Assignment
                    WHERE Course_ID = @Course_ID";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Course_ID", courseId);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        FaId = reader["FA_ID"].ToString();
                        FinalAssignmentLabel.Text = reader["FA_Name"].ToString();
                        DeadlineLabel.Text = "Due Date: " + Convert.ToDateTime(reader["FA_Deadline"]).ToString("MM/dd/yyyy");

                        string fileName = reader["FA_File"].ToString();
                        FileDownloadLink.CommandArgument = fileName;
                        FileDownloadLink.Text = $"<i class='fas fa-download'></i> <span>{fileName}</span>";
                        FileDownloadLink.Visible = true;
                    }
                    else
                    {
                        FinalAssignmentLabel.Text = "No final assignment found for this course.";
                        FileDownloadLink.Visible = false;
                        FaId = null;
                    }
                }
            }
        }

        private void CheckExistingSubmission(string studentId)
        {
            if (string.IsNullOrEmpty(FaId)) return;

            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connStr))
            {
                const string sql = @"
                    SELECT Answer_File
                    FROM Answer_Table 
                    WHERE Student_ID = @Student_ID AND FA_ID = @FA_ID";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Student_ID", studentId);
                    cmd.Parameters.AddWithValue("@FA_ID", FaId);
                    con.Open();
                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        HasSubmitted = true;
                        string uploadedFile = result.ToString();
                        UploadedFileLabel.Text = $"Submitted for grading!";
                        UploadedFileLabel.ForeColor = System.Drawing.Color.Gray;
                        DisableUpload("submitted");
                    }
                }
            }
        }

        private void DisableUpload(string status)
        {
            FileUpload1.Enabled = false;
            FileUpload1.Attributes["style"] = "display: none;";
            FileUpload1.Attributes["disabled"] = "disabled";

            SubmitAssignmentButton.Enabled = false;
            SubmitAssignmentButton.CssClass = "submit-button disabled";

            if (status == "graded")
            {
                SubmitAssignmentButton.Text = "Graded";
            }
            else if (status == "submitted")
            {
                SubmitAssignmentButton.Text = "Submitted";
            }

            string script = @"
                document.getElementById('fileLabel').innerHTML = '<i class=""fas fa-check upload-icon""></i> Assignment Already Submitted';
                    document.getElementById('fileLabel').style.cursor = 'not-allowed';
                    document.getElementById('fileLabel').style.opacity = '0.6'; ";

            ScriptManager.RegisterStartupScript(this, GetType(), "disableUpload", script, true);
         }

        protected void DownloadFile(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            string fileName = btn.CommandArgument;

            if (string.IsNullOrEmpty(fileName))
            {
                ShowMessage("No file specified.", "error");
                return;
            }

            string filePath = Server.MapPath("~/Uploads/Assignments/") + fileName;

            if (File.Exists(filePath))
            {
                Response.Clear();
                Response.ContentType = GetMimeType(fileName);
                Response.AppendHeader("Content-Disposition", $"attachment; filename=\"{fileName}\"");
                Response.TransmitFile(filePath);
                Response.End();
            }
            else
            {
                ShowMessage("File not found.", "error");
            }
        }

        private string GetMimeType(string fileName)
        {
            string ext = Path.GetExtension(fileName).ToLower();
            switch (ext)
            {
                case ".pdf": return "application/pdf";
                case ".doc": return "application/msword";
                case ".docx": return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                case ".zip": return "application/zip";
                case ".txt": return "text/plain";
                case ".pptx": return "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                default: return "application/octet-stream";
            }
        }

        protected void SubmitAssignmentButton_Click(object sender, EventArgs e)
        {
            string studentId = Session["StudentID"]?.ToString();

            if (string.IsNullOrEmpty(studentId) || string.IsNullOrEmpty(FaId))
            {
                ShowMessage("Session expired or assignment not found.", "error");
                return;
            }

            if (HasSubmitted)
            {
                ShowMessage("You have already submitted this assignment.", "error");
                return;
            }

            if (!FileUpload1.HasFile)
            {
                ShowMessage("Please select a file to upload.", "error");
                return;
            }

            HttpPostedFile file = FileUpload1.PostedFile;

            if (file.ContentLength > 10 * 1024 * 1024)
            {
                ShowMessage("File size must be less than 10MB.", "error");
                return;
            }

            string ext = Path.GetExtension(file.FileName).ToLower();
            string[] allowedExtensions = { ".pdf", ".doc", ".docx", ".zip", ".txt", ".pptx" };

            if (Array.IndexOf(allowedExtensions, ext) == -1)
            {
                ShowMessage("Invalid file type. Allowed: PDF, DOC, DOCX, ZIP, TXT, PPTX", "error");
                return;
            }

            try
            {
                string answerId = GenerateNewAnswerId();
                string fileName = answerId + ext;
                string uploadDir = Server.MapPath("~/Uploads/Answers/");

                if (!Directory.Exists(uploadDir))
                    Directory.CreateDirectory(uploadDir);

                string filePath = Path.Combine(uploadDir, fileName);
                file.SaveAs(filePath);

                if (InsertIntoDatabase(answerId, fileName, studentId))
                {
                    HasSubmitted = true;
                    ShowMessage("Assignment submitted successfully!", "success");
                    CheckExistingSubmission(studentId); // This will call DisableUpload()
                }
                else
                {
                    ShowMessage("Failed to save submission record.", "error");
                    if (File.Exists(filePath))
                        File.Delete(filePath);
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Upload failed: " + ex.Message, "error");
            }
        }

        private bool InsertIntoDatabase(string answerId, string fileName, string studentId)
        {
            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            const string sql = @"
                INSERT INTO Answer_Table (Answer_ID, Student_ID, FA_ID, Answer_File)
                VALUES (@Answer_ID, @Student_ID, @FA_ID, @Answer_File)";

            using (SqlConnection con = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@Answer_ID", answerId);
                cmd.Parameters.AddWithValue("@Student_ID", studentId);
                cmd.Parameters.AddWithValue("@FA_ID", FaId);
                cmd.Parameters.AddWithValue("@Answer_File", fileName);

                con.Open();
                int result = cmd.ExecuteNonQuery();
                return result > 0;
            }
        }

        private string GenerateNewAnswerId()
        {
            string nextId = "AS001";
            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string query = "SELECT MAX(Answer_ID) FROM Answer_Table WHERE Answer_ID LIKE 'AS%'";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    conn.Open();
                    object result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        string maxId = result.ToString();
                        if (maxId.StartsWith("AS") && int.TryParse(maxId.Substring(2), out int num))
                        {
                            nextId = $"AS{(num + 1):D3}";
                        }
                    }
                }
            }
            catch
            {

            }

            return nextId;
        }

        private void ShowMessage(string text, string type)
        {
            UploadedFileLabel.Text = text;
            UploadedFileLabel.ForeColor = type == "success"
                ? System.Drawing.Color.LightGreen
                : System.Drawing.Color.Red;

            if (type == "success")
            {
                string script = $@"
                    setTimeout(function() {{
                        var el = document.getElementById('{UploadedFileLabel.ClientID}');
                        if (el) el.style.display = 'none';
                    }}, 2000);";

                ScriptManager.RegisterStartupScript(this, GetType(),
                    "msg" + Guid.NewGuid(), script, true);
            }
        }

        private void LoadMarkingStatus(string courseId, string studentId)
        {
            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connStr))
            {
                const string sql = @"
                    SELECT Marking_Grades, Marking_Status
                    FROM Marking_Table
                    WHERE Answer_ID = (SELECT Answer_ID FROM Answer_Table WHERE Student_ID = @Student_ID AND FA_ID = @FA_ID)";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Student_ID", studentId);
                    cmd.Parameters.AddWithValue("@FA_ID", FaId);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        string markingStatus = reader["Marking_Status"].ToString();
                        string markingGrade = reader["Marking_Grades"].ToString();

                        UploadedFileLabel.Text = $"Graded! Score: {markingGrade}";

                        if (markingStatus == "Pass")
                        {
                            UploadedFileLabel.ForeColor = System.Drawing.Color.LightGreen;
                        }
                        else if (markingStatus == "Fail")
                        {
                            UploadedFileLabel.ForeColor = System.Drawing.Color.Red;
                        }

                        DisableUpload("graded");
                    }
                    else
                    {
                        UploadedFileLabel.Text = "Submitted for grading!";
                        UploadedFileLabel.ForeColor = System.Drawing.Color.Gray;
                    }
                }
            }
        }
    }
}