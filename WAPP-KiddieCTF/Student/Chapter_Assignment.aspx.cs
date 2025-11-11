using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;
using System.Net.Http;
using System.Text;

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
                Response.Redirect("/LogIn.aspx");
            }
            else
            {
                if (!IsPostBack)
                {
                    LoadCourseDetails(courseId);
                    LoadChapters(courseId, studentId);
                    LoadFinalAssignment(courseId);
                    CheckCourseCompletionStatus(studentId, courseId);
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

                    ViewState["CourseName"] = courseName;
                    ViewState["CourseId"] = courseId;
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
            catch (Exception)
            {
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

        private void CheckCourseCompletionStatus(string studentId, string courseId)
        {
            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            const string totalChaptersSql = @"
            SELECT COUNT(*) 
            FROM Chapter 
            WHERE Course_ID = @Course_ID";

            const string completedChaptersSql = @"
            SELECT COUNT(DISTINCT Chapter_ID)
            FROM Progress_Checker 
            WHERE Student_ID = @Student_ID 
            AND Chapter_ID IN (SELECT Chapter_ID FROM Chapter WHERE Course_ID = @Course_ID)";

            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();

                int totalChapters = 0;
                using (SqlCommand cmd = new SqlCommand(totalChaptersSql, con))
                {
                    cmd.Parameters.AddWithValue("@Course_ID", courseId);
                    totalChapters = Convert.ToInt32(cmd.ExecuteScalar());
                }

                int completedChapters = 0;
                using (SqlCommand cmd = new SqlCommand(completedChaptersSql, con))
                {
                    cmd.Parameters.AddWithValue("@Student_ID", studentId);
                    cmd.Parameters.AddWithValue("@Course_ID", courseId);
                    completedChapters = Convert.ToInt32(cmd.ExecuteScalar());
                }

                if (completedChapters == totalChapters)
                {
                    CheckFinalAssignmentStatus(studentId, courseId);
                }
                else
                {
                    GenerateCertificateButton.Visible = false;
                }
            }
        }

        private void CheckFinalAssignmentStatus(string studentId, string courseId)
        {
            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            const string faIdSql = @"
            SELECT FA_ID
            FROM Final_Assignment
            WHERE Course_ID = @Course_ID";

            string faId = string.Empty;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(faIdSql, con))
                {
                    cmd.Parameters.AddWithValue("@Course_ID", courseId);
                    faId = Convert.ToString(cmd.ExecuteScalar());
                }
            }

            if (string.IsNullOrEmpty(faId))
            {
                GenerateCertificateButton.Visible = false;
                return;
            }

            const string answerSql = @"
            SELECT Answer_ID
            FROM Answer_Table 
            WHERE Student_ID = @Student_ID AND FA_ID = @FA_ID";

            string answerId = string.Empty;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(answerSql, con))
                {
                    cmd.Parameters.AddWithValue("@Student_ID", studentId);
                    cmd.Parameters.AddWithValue("@FA_ID", faId);
                    answerId = Convert.ToString(cmd.ExecuteScalar());
                }
            }

            if (!string.IsNullOrEmpty(answerId))
            {
                const string markingSql = @"
                SELECT Marking_Status
                FROM Marking_Table 
                WHERE Answer_ID = @Answer_ID";

                string markingStatus = string.Empty;

                using (SqlConnection con = new SqlConnection(connStr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(markingSql, con))
                    {
                        cmd.Parameters.AddWithValue("@Answer_ID", answerId);
                        markingStatus = Convert.ToString(cmd.ExecuteScalar());
                    }
                }

                if (markingStatus == "Pass")
                {
                    GenerateCertificateButton.Visible = true;
                }
                else
                {
                    GenerateCertificateButton.Visible = false;
                }
            }
            else
            {
                GenerateCertificateButton.Visible = false;
            }
        }

        private async Task<string> GenerateCertificatePdfAsync(string studentId, string courseId, string studentName, string courseName)
        {
            try
            {
                string existingCertificateId = GetExistingCertificateId(studentId, courseId);
                string certificateId = !string.IsNullOrEmpty(existingCertificateId) ? existingCertificateId : GenerateNewCertificateId();
                string fileName = $"{certificateId}_{studentName.Replace(" ", "_")}_{courseName.Replace(" ", "_")}.pdf";
                string filePath = Server.MapPath($"~/Uploads/Certificates/{fileName}");

                if (File.Exists(filePath))
                {
                    return $"~/Uploads/Certificates/{fileName}";
                }

                string apiKey = ConfigurationManager.AppSettings["PdfCoKey"]?.Trim();

                if (string.IsNullOrEmpty(apiKey))
                {
                    return string.Empty;
                }

                string certificateDir = Server.MapPath("~/Uploads/Certificates/");
                if (!Directory.Exists(certificateDir))
                {
                    Directory.CreateDirectory(certificateDir);
                }

                string lecturerName = GetLecturerNameByCourseId(courseId);
                string htmlContent = CreateCertificateHtml(studentName, courseName, certificateId, lecturerName);

                return await GenerateWithPdfCo(apiKey, htmlContent, filePath, fileName, certificateId, studentId, courseId, existingCertificateId);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        private async Task<string> GenerateWithPdfCo(string apiKey, string htmlContent, string filePath, string fileName, string certificateId, string studentId, string courseId, string existingCertificateId)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.Timeout = TimeSpan.FromSeconds(30);

                    var requestData = new
                    {
                        html = htmlContent,
                        name = fileName,
                        landscape = false,
                        media = "print"
                    };

                    string json = Newtonsoft.Json.JsonConvert.SerializeObject(requestData);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    httpClient.DefaultRequestHeaders.Add("x-api-key", apiKey);

                    string apiUrl = "https://api.pdf.co/v1/pdf/convert/from/html";

                    var response = await httpClient.PostAsync(apiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var result = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(response.Content.ReadAsStringAsync().Result);
                        if (result != null && result.url != null)
                        {
                            string pdfUrl = result.url.ToString();

                            var pdfResponse = await httpClient.GetAsync(pdfUrl);
                            if (pdfResponse.IsSuccessStatusCode)
                            {
                                var pdfBytes = await pdfResponse.Content.ReadAsByteArrayAsync();

                                if (pdfBytes != null && pdfBytes.Length > 0)
                                {
                                    File.WriteAllBytes(filePath, pdfBytes);

                                    // Save to database only if it's a new certificate
                                    if (string.IsNullOrEmpty(existingCertificateId))
                                    {
                                        SaveCertificateToDatabase(certificateId, studentId, courseId);
                                    }

                                    return $"~/Uploads/Certificates/{fileName}";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Silent fail
            }

            return string.Empty;
        }

        private string GetExistingCertificateId(string studentId, string courseId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT Certificate_ID 
                    FROM Certificate 
                    WHERE Student_ID = @Student_ID AND Course_ID = @Course_ID";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Student_ID", studentId);
                cmd.Parameters.AddWithValue("@Course_ID", courseId);

                con.Open();
                var result = cmd.ExecuteScalar();

                return result?.ToString() ?? string.Empty;
            }
        }

        private string GenerateNewCertificateId()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT TOP 1 Certificate_ID FROM Certificate ORDER BY Certificate_ID DESC";

                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    string lastId = reader["Certificate_ID"].ToString();
                    int nextId = int.Parse(lastId.Substring(2)) + 1;
                    return "CF" + nextId.ToString("D3");
                }
                else
                {
                    return "CF001";
                }
            }
        }

        private string GetLecturerNameByCourseId(string courseId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = @"
                        SELECT L.Lecturer_Name 
                        FROM Course C 
                        INNER JOIN Lecturer L ON C.Lecturer_ID = L.Lecturer_ID 
                        WHERE C.Course_ID = @Course_ID";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Course_ID", courseId);

                    con.Open();
                    var result = cmd.ExecuteScalar();

                    return result?.ToString() ?? "Course Lecturer";
                }
            }
            catch (Exception)
            {
                return "Course Lecturer";
            }
        }

        private bool SaveCertificateToDatabase(string certificateId, string studentId, string courseId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = @"
                    INSERT INTO Certificate (Certificate_ID, Student_ID, Course_ID)
                    VALUES (@Certificate_ID, @Student_ID, @Course_ID)";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Certificate_ID", certificateId);
                    cmd.Parameters.AddWithValue("@Student_ID", studentId);
                    cmd.Parameters.AddWithValue("@Course_ID", courseId);

                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        private string CreateCertificateHtml(string studentName, string courseName, string certificateId, string lecturerName)
        {
            return $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <style>
                        @import url('https://fonts.googleapis.com/css2?family=Playfair+Display:wght@400;700&family=Montserrat:wght@300;400;600&family=Dancing+Script:wght@700&display=swap');
    
                        body {{ 
                            font-family: 'Montserrat', sans-serif;
                            margin: 0; 
                            padding: 0;
                            text-align: center;
                            min-height: 100vh;
                            display: flex;
                            align-items: center;
                            justify-content: center;
                        }}
                        .certificate-wrapper {{
                            padding: 10px;
                            margin: 20px;
                        }}
                        .certificate-container {{
                            border: 15px double #2c3e50;
                            padding: 50px 40px;
                            position: relative;
                            border-radius: 10px;
                        }}
                        .certificate-badge {{
                            position: absolute;
                            top: -25px;
                            left: 50%;
                            transform: translateX(-50%);
                            background: #e74c3c;
                            color: white;
                            padding: 10px 30px;
                            border-radius: 25px;
                            font-weight: 600;
                            font-size: 14px;
                            letter-spacing: 2px;
                        }}
                        .certificate-header {{
                            color: #2c3e50;
                            font-size: 42px;
                            font-weight: 700;
                            margin-bottom: 20px;
                            text-transform: uppercase;
                            letter-spacing: 3px;
                        }}
                        .certificate-subtitle {{
                            color: #7f8c8d;
                            font-size: 18px;
                            margin-bottom: 20px;
                            font-weight: 300;
                            letter-spacing: 1px;
                        }}
                        .student-name {{
                            font-size: 25px;
                            color: #e74c3c;
                            font-weight: 600;
                            margin: 40px 0;
                            padding: 15px;
                            border-bottom: 3px solid #bdc3c7;
                            display: inline-block;
                            text-shadow: 1px 1px 2px rgba(0,0,0,0.1);
                        }}
                        .course-name {{
                            font-size: 25px;
                            color: #2c3e50;
                            margin: 30px 0;
                            font-weight: 600;
                        }}
                        .completion-text {{
                            font-size: 16px;
                            color: #5d6d7e;
                            margin: 30px 0;
                            line-height: 1.8;
                            max-width: 600px;
                            margin-left: auto;
                            margin-right: auto;
                        }}
                        .date {{
                            font-size: 14px;
                            color: #95a5a6;
                            margin-top: 50px;
                            font-style: italic;
                        }}
                        .certificate-id {{
                            font-size: 14px;
                            color: #7f8c8d;
                            margin-top: 10px;
                            font-family: monospace;
                        }}
                        .signature-section {{
                            margin-top: 80px;
                            text-align: center;
                        }}
                        .signature {{
                            display: inline-block;
                            text-align: center;
                        }}
                        .signature-line {{
                            border-top: 2px solid #2c3e50;
                            width: 300px;
                            margin: 10px auto;
                        }}
                        .electronic-signature {{
                            font-family: 'Dancing Script', cursive;
                            font-size: 25px;
                            color: #2c3e50;
                            margin: 15px 0;
                            font-weight: bold;
                        }}
                        .signature-name {{
                            font-weight: 600;
                            color: #2c3e50;
                            margin-top: 5px;
                            font-size: 18px;
                        }}
                        .signature-title {{
                            font-size: 12px;
                            color: #7f8c8d;
                            font-style: italic;
                        }}
                    </style>
                </head>
                <body>
                    <div class='certificate-wrapper'>
                        <div class='certificate-container'>
                            <div class='certificate-badge'>CERTIFICATE</div>

                            <div class='certificate-header'>Certificate of Completion</div>
                            <div class='certificate-subtitle'>This is to certify that</div>

                            <div class='student-name'>{System.Web.HttpUtility.HtmlEncode(studentName)}</div>

                            <div class='completion-text'>
                                has successfully completed the course requirements and demonstrated outstanding dedication and proficiency in
                            </div>

                            <div class='course-name'>{System.Web.HttpUtility.HtmlEncode(courseName)}</div>

                            <div class='completion-text'>
                                This achievement recognizes the commitment to excellence and the successful 
                                mastery of the course material.
                            </div>

                            <div class='date'>Awarded on {DateTime.Now:MMMM dd, yyyy}</div>
                            <div class='certificate-id'>Certificate ID: {certificateId}</div>

                            <div class='signature-section'>
                                <div class='signature'>
                                    <div class='electronic-signature'>{System.Web.HttpUtility.HtmlEncode(lecturerName)}</div>
                                    <div class='signature-line'></div>
                                    <div class='signature-name'>{System.Web.HttpUtility.HtmlEncode(lecturerName)}</div>
                                    <div class='signature-title'>Course Lecturer</div>
                                </div>
                            </div>
                        </div>
                    </div>
                </body>
                </html>";
        }

        protected async void GenerateCertificateButton_Click(object sender, EventArgs e)
        {
            string studentId = Session["StudentID"]?.ToString();
            string courseId = Request.QueryString["courseId"];

            if (!string.IsNullOrEmpty(studentId) && !string.IsNullOrEmpty(courseId))
            {
                string courseName = ViewState["CourseName"]?.ToString() ?? "Course";
                string studentName = Session["StudentName"]?.ToString() ?? $"Student {studentId}";

                string pdfVirtualPath = await GenerateCertificatePdfAsync(studentId, courseId, studentName, courseName);

                if (!string.IsNullOrEmpty(pdfVirtualPath))
                {
                    string physicalPath = Server.MapPath(pdfVirtualPath);

                    if (File.Exists(physicalPath))
                    {
                        try
                        {
                            Response.Clear();
                            Response.ClearHeaders();
                            Response.ClearContent();

                            Response.ContentType = "application/pdf";
                            Response.AppendHeader("Content-Disposition", $"attachment; filename=Certificate_{studentName.Replace(" ", "_")}.pdf");

                            Response.WriteFile(physicalPath);

                            Response.Flush();
                            HttpContext.Current.ApplicationInstance.CompleteRequest();
                            return;
                        }
                        catch (System.Threading.ThreadAbortException)
                        {
                            return;
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
            }
        }
    }
}