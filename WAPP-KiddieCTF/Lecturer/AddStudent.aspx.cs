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
    public partial class AddStudent : System.Web.UI.Page
    {
        private string CourseID => Request.QueryString["course"];

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LecturerID"] == null || Session["LecturerName"] == null)
            {
                Response.Redirect("../LogIn.aspx");
                return;
            }

            lblLecturerID.Text = Session["LecturerID"].ToString();
            lblLecturerName.Text = Session["LecturerName"].ToString();

            if (string.IsNullOrEmpty(CourseID))
                Response.Redirect("Courses.aspx");

            if (IsPostBack && Request["__EVENTTARGET"] != null && Request["__EVENTARGUMENT"] != null)
            {
                string eventTarget = Request["__EVENTTARGET"];
                string studentId = Request["__EVENTARGUMENT"];

                // Make sure the eventTarget belongs to your repeater buttons
                if (eventTarget.Contains("btnAdd"))
                {
                    AddStudentToCourse(studentId);
                }
            }

            if (!IsPostBack)
                LoadStudents();
        }

        private void LoadStudents(string search = "")
        {
            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
            SELECT s.Student_ID, s.Student_Name, s.Intake_Code
            FROM Student s
            WHERE 
               (s.Student_ID LIKE @Search 
                OR s.Student_Name LIKE @Search 
                OR s.Intake_Code LIKE @Search)
            ORDER BY s.Student_ID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Search", $"%{search}%");
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Remove those already added (in session)
                List<string> tempStudents = Session["TempStudents"] as List<string> ?? new List<string>();
                dt = dt.AsEnumerable()
                       .Where(r => !tempStudents.Contains(r["Student_ID"].ToString()))
                       .CopyToDataTable<DataRow>();

                StudentRepeater.DataSource = dt;
                StudentRepeater.DataBind();

                pnlNoResults.Visible = dt.Rows.Count == 0;
            }
        }

        private void AddStudentToCourse(string studentId)
        {
            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string checkQuery = "SELECT COUNT(*) FROM Assigned_Course WHERE Course_ID = @CourseID AND Student_ID = @StudentID";
                SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@CourseID", CourseID);
                checkCmd.Parameters.AddWithValue("@StudentID", studentId);

                conn.Open();
                int count = (int)checkCmd.ExecuteScalar();
                if (count > 0)
                {
                    LoadStudents(txtSearch.Text.Trim());
                    return;
                }

                string maxQuery = "SELECT ISNULL(MAX(CAST(SUBSTRING(AC_ID, 3, 50) AS INT)), 0) FROM Assigned_Course";
                SqlCommand maxCmd = new SqlCommand(maxQuery, conn);
                int maxId = (int)maxCmd.ExecuteScalar();
                string newAcId = "AC" + (maxId + 1).ToString("D3");

                string insertQuery = "INSERT INTO Assigned_Course (AC_ID, Course_ID, Student_ID) VALUES (@AC_ID, @CourseID, @StudentID)";
                SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                insertCmd.Parameters.AddWithValue("@AC_ID", newAcId);
                insertCmd.Parameters.AddWithValue("@CourseID", CourseID);
                insertCmd.Parameters.AddWithValue("@StudentID", studentId);
                insertCmd.ExecuteNonQuery();
            }

            LoadStudents(txtSearch.Text.Trim());
            ScriptManager.RegisterStartupScript(this, GetType(), "AddSuccess",
                "Swal.fire({ title: 'Added!', text: 'Student added successfully.', icon: 'success', background:'#1B263B', color:'#fff' });", true);
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadStudents(txtSearch.Text.Trim());

            UpdatePanelSearch.Update();
            UpdatePanelStudents.Update();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string studentId = btn.CommandArgument;

            // Retrieve or create temp student list
            List<string> tempStudents = Session["TempStudents"] as List<string> ?? new List<string>();

            // Avoid duplicates
            if (!tempStudents.Contains(studentId))
                tempStudents.Add(studentId);

            Session["TempStudents"] = tempStudents;

            ScriptManager.RegisterStartupScript(this, GetType(), "AddTemp",
                "Swal.fire({ icon: 'success', title: 'Student added temporarily!', background:'#1B263B', color:'#fff' });", true);

            // Reload list (simulate removal from available students)
            LoadStudents(txtSearch.Text.Trim());
        }

    }
}