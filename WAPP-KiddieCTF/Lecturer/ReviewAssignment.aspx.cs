using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WAPP_KiddieCTF.Lecturer
{
    public partial class ReviewAssignment : System.Web.UI.Page
    {

        private readonly string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        private string AnswerID => Request.QueryString["answerid"];
        private string StudentID => Request.QueryString["studentid"];

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LecturerID"] == null || Session["LecturerName"] == null)
            {
                Response.Redirect("../Default.aspx");
                return;
            }

            lblLecturerName.Text = Session["LecturerName"].ToString();
            lblLecturerID.Text = Session["LecturerID"].ToString();

            if (string.IsNullOrEmpty(AnswerID) || string.IsNullOrEmpty(StudentID))
                Response.Redirect("Courses.aspx");

            if (!IsPostBack)
                LoadSubmission();
        }

        private void LoadSubmission()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
            SELECT 
                fa.FA_ID, fa.FA_Name,
                s.Student_ID, s.Student_Name,
                a.Answer_File,
                m.Marking_Grades, m.Marking_Status
            FROM Answer_Table a
            INNER JOIN Final_Assignment fa ON a.FA_ID = fa.FA_ID
            INNER JOIN Student s ON a.Student_ID = s.Student_ID
            LEFT JOIN Marking_Table m ON a.Answer_ID = m.Answer_ID
            WHERE a.Answer_ID = @AnswerID AND a.Student_ID = @StudentID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@AnswerID", AnswerID);
                cmd.Parameters.AddWithValue("@StudentID", StudentID);

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    // === Basic Info ===
                    lblFAID.Text = dr["FA_ID"].ToString();
                    lblFAName.Text = dr["FA_Name"].ToString();
                    lblStudentID.Text = dr["Student_ID"].ToString();
                    lblStudentName.Text = dr["Student_Name"].ToString();

                    string fileName = dr["Answer_File"].ToString();
                    string grade = dr["Marking_Grades"]?.ToString();
                    string status = dr["Marking_Status"]?.ToString();

                    // === Case 1: Student has not submitted ===
                    if (string.IsNullOrEmpty(fileName))
                    {
                        DisableForm();
                        ShowAlert("info", "Student has not submitted their work yet!");
                        return;
                    }

                    // === Case 2: File Submitted ===
                    string filePath = Server.MapPath("~/Uploads/Answers/" + fileName);
                    if (System.IO.File.Exists(filePath))
                    {
                        lblFileName.Text = fileName;

                        lnkDownload.NavigateUrl = ResolveUrl("~/Uploads/Answers/" + fileName);
                        lnkDownload.Target = "_blank";
                    }
                    else
                    {
                        lblFileName.Text = "File not found on server!";
                        lnkDownload.NavigateUrl = "#";
                    }

                    // === Case 3: Already Graded (Pass/Fail) ===
                    if (status == "Pass" || status == "Fail")
                    {
                        txtMark.Text = $"{grade} ({status})";
                        txtMark.Enabled = false;
                        btnDone.Enabled = true;
                        btnDone.CssClass = "done-btn";
                    }
                    else
                    {
                        // === Case 4: Submitted but not yet graded ===
                        txtMark.Enabled = true;
                        btnDone.Enabled = true;
                    }
                }
                else
                {
                    DisableForm();
                    ShowAlert("error", "Submission not found!");
                }
            }
        }

        protected void btnDone_Click(object sender, EventArgs e)
        {
            string courseID = Request.QueryString["courseid"];

            // Case 1️: If mark input is disabled (Graded student) → just go back
            if (!txtMark.Enabled)
            {
                Response.Redirect("AssignmentProgress.aspx?courseid=" + courseID);
                return;
            }

            // Case 2️: Validate input
            if (string.IsNullOrWhiteSpace(txtMark.Text))
            {
                ShowAlert("error", "Please enter a mark!");
                return;
            }

            if (!int.TryParse(txtMark.Text, out int mark) || mark < 0 || mark > 100)
            {
                ShowAlert("error", "Mark must be between 0 and 100!");
                return;
            }

            // Case 3️: Save to Marking_Table
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                // Generate next Marking_ID (MK001, MK002, ...)
                string getIdQuery = "SELECT ISNULL(MAX(CAST(SUBSTRING(Marking_ID, 3, 10) AS INT)), 0) + 1 FROM Marking_Table WHERE Marking_ID LIKE 'MK%'";
                SqlCommand getIdCmd = new SqlCommand(getIdQuery, conn);
                int nextId = Convert.ToInt32(getIdCmd.ExecuteScalar());
                string newMarkingID = "MK" + nextId.ToString("D3");
                string lecturerId = Session["LecturerID"].ToString();

                // Determine Pass/Fail
                string resultStatus = mark >= 50 ? "Pass" : "Fail";

                // Insert new record into Marking_Table
                string insertQuery = @"
            INSERT INTO Marking_Table (Marking_ID, Lecturer_ID, Answer_ID, Marking_Grades, Marking_Status)
            VALUES (@ID, @LecturerID, @AnswerID, @Grade, @Status)";
                SqlCommand cmd = new SqlCommand(insertQuery, conn);
                cmd.Parameters.AddWithValue("@ID", newMarkingID);
                cmd.Parameters.AddWithValue("@LecturerID", lecturerId);
                cmd.Parameters.AddWithValue("@AnswerID", AnswerID);
                cmd.Parameters.AddWithValue("@Grade", mark);
                cmd.Parameters.AddWithValue("@Status", resultStatus);

                cmd.ExecuteNonQuery();
            }

            // Case 4️: Notify & redirect back
            string script = $@"
        Swal.fire({{
            icon: 'success',
            title: 'Grade saved successfully!',
            showConfirmButton: false,
            timer: 1500
        }}).then(() => {{
            window.location = 'AssignmentProgress.aspx?courseid={courseID}';
        }});
    ";
            ScriptManager.RegisterStartupScript(this, GetType(), "GradeSaved", script, true);
        }

        private void DisableForm()
        {
            txtMark.Enabled = false;
            btnDone.Enabled = false;
            btnDone.CssClass = "done-btn disabled";
            lblFileName.Text = "No file submitted";
            lnkDownload.NavigateUrl = "#";
        }

        private void ShowAlert(string icon, string message)
        {
            string script = $@"Swal.fire({{ icon: '{icon}', title: '{message}', confirmButtonColor: '#3085d6' }});";
            ScriptManager.RegisterStartupScript(this, GetType(), "Alert", script, true);
        }

    }
}