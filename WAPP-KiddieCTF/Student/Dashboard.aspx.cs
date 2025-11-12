using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;

namespace WAPP_KiddieCTF.Student
{
    public partial class Dashboard : System.Web.UI.Page
    {
        private readonly string connStr =
            ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string studentId = Session["StudentID"] as string;
                if (string.IsNullOrEmpty(studentId))
                {
                    Response.Redirect("~/Login.aspx");
                    return;
                }

                LoadRecentAccess();
                LoadCounts();
                LoadUpcomingDeadlines();
            }
        }

        // ===========================================================
        // 1. RECENTLY ACCESSED COURSES / CHALLENGES
        // ===========================================================
        private void LoadRecentAccess()
        {
            string studentId = Session["StudentID"] as string;

            if (string.IsNullOrEmpty(studentId))
            {
                rptRecent.DataSource = null;
                rptRecent.DataBind();
                return;
            }

            DataTable all = new DataTable();
            all.Columns.Add("ItemType");
            all.Columns.Add("ItemID");
            all.Columns.Add("ItemName");
            all.Columns.Add("LecturerID");
            all.Columns.Add("AccessDate", typeof(DateTime));

            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand(@"
                    SELECT TOP 5 acr.Course_ID, acr.ACouR_Date, c.Course_Name, c.Lecturer_ID
                    FROM Access_Course_Record acr
                    INNER JOIN Course c ON acr.Course_ID = c.Course_ID
                    WHERE acr.Student_ID = @StudentID
                    ORDER BY acr.ACouR_Date DESC, acr.ACouR_ID DESC", con))
                {
                    cmd.Parameters.AddWithValue("@StudentID", studentId);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            DateTime accessDate = dr["ACouR_Date"] != DBNull.Value ? Convert.ToDateTime(dr["ACouR_Date"]) : DateTime.MinValue;
                            all.Rows.Add("COURSE", dr["Course_ID"], dr["Course_Name"], dr["Lecturer_ID"], accessDate);
                        }
                    }
                }

                using (SqlCommand cmd = new SqlCommand(@"
                    SELECT TOP 5 acr.Challenge_ID, acr.AChaR_Date, ch.Challenge_Name, ch.Lecturer_ID
                    FROM Access_Challenge_Record acr
                    INNER JOIN Challenge ch ON acr.Challenge_ID = ch.Challenge_ID
                    WHERE acr.Student_ID = @StudentID
                    ORDER BY acr.AChaR_Date DESC, acr.AChaR_ID DESC", con))
                {
                    cmd.Parameters.AddWithValue("@StudentID", studentId);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            DateTime accessDate = dr["AChaR_Date"] != DBNull.Value ? Convert.ToDateTime(dr["AChaR_Date"]) : DateTime.MinValue;
                            all.Rows.Add("CHALLENGE", dr["Challenge_ID"], dr["Challenge_Name"], dr["Lecturer_ID"], accessDate);
                        }
                    }
                }
            }

            var ordered = all.AsEnumerable().OrderByDescending(r => r.Field<DateTime>("AccessDate")).Take(10);
            if (ordered.Any())
            {
                rptRecent.DataSource = ordered.CopyToDataTable();
                rptRecent.DataBind();
            }
            else
            {
                rptRecent.DataSource = null;
                rptRecent.DataBind();
            }
        }

        // ===========================================================
        // 2. BOTTOM CARDS COUNTS
        // ===========================================================
        private void LoadCounts()
        {
            string studentId = Session["StudentID"] as string;

            if (string.IsNullOrEmpty(studentId))
            {
                lblCompletedFA.Text = "0";
                lblCertificates.Text = "0";
                lblChallengeProgress.Text = "Completed 0/0 Challenges";
                return;
            }

            int doneFA = GetCompletedFACount(studentId);
            lblCompletedFA.Text = doneFA.ToString();

            int certs = GetCertificateCount(studentId);
            lblCertificates.Text = certs.ToString();

            var challengeData = GetChallengeProgress(studentId);
            lblChallengeProgress.Text = $"Completed {challengeData.solved}/{challengeData.total} Challenges";

            hiddenSolved.Value = challengeData.solved.ToString();
            hiddenTotal.Value = challengeData.total.ToString();
        }

        // ===========================================================
        // 3. UPCOMING DEADLINES
        // ===========================================================
        private void LoadUpcomingDeadlines()
        {
            string studentId = Session["StudentID"] as string;

            if (string.IsNullOrEmpty(studentId))
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            DataTable deadlines = GetUpcomingDeadlines(studentId);

            if (deadlines.Rows.Count > 0)
            {
                rptDeadlines.DataSource = deadlines;
                rptDeadlines.DataBind();
                pnlNoDeadlines.Visible = false;
            }
            else
            {
                rptDeadlines.DataSource = null;
                rptDeadlines.DataBind();
                pnlNoDeadlines.Visible = true;
            }
        }

        private int GetCompletedFACount(string studentId)
        {
            int count = 0;
            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(@"
                    SELECT COUNT(DISTINCT FA_ID)
                    FROM Answer_Table
                    WHERE Student_ID = @StudentID", con))
                {
                    cmd.Parameters.AddWithValue("@StudentID", studentId);
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                        count = Convert.ToInt32(result);
                }
            }
            return count;
        }

        private int GetCertificateCount(string studentId)
        {
            int count = 0;
            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(@"
                    SELECT COUNT(*)
                    FROM Certificate
                    WHERE Student_ID = @StudentID", con))
                {
                    cmd.Parameters.AddWithValue("@StudentID", studentId);
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                        count = Convert.ToInt32(result);
                }
            }
            return count;
        }

        private (int solved, int total) GetChallengeProgress(string studentId)
        {
            int solved = 0;
            int total = 0;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();

                using (SqlCommand cmdTotal = new SqlCommand(@"
                    SELECT COUNT(*)
                    FROM Challenge
                    WHERE Challenge_ID IS NOT NULL", con))
                {
                    object result = cmdTotal.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                        total = Convert.ToInt32(result);
                }

                using (SqlCommand cmdSolved = new SqlCommand(@"
                    SELECT COUNT(DISTINCT Challenge_ID)
                    FROM Challenge_Solved
                    WHERE Student_ID = @StudentID", con))
                {
                    cmdSolved.Parameters.AddWithValue("@StudentID", studentId);
                    object result = cmdSolved.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                        solved = Convert.ToInt32(result);
                }
            }

            return (solved, total);
        }

        private DataTable GetUpcomingDeadlines(string studentId)
        {
            DataTable deadlines = new DataTable();

            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(@"
                    SELECT TOP 3 
                    FA.FA_Name,            
                    FA.FA_Deadline,      
                    C.Course_Name,
                    C.Course_ID,
                    FORMAT(TRY_CONVERT(DATETIME, FA.FA_Deadline, 103), 'MM/dd/yyyy') AS FormattedDate
                    FROM Final_Assignment FA
                    INNER JOIN Course C ON FA.Course_ID = C.Course_ID  
                    INNER JOIN Assigned_Course AC ON C.Course_ID = AC.Course_ID 
                    WHERE AC.Student_ID = @StudentID
                        AND TRY_CONVERT(DATETIME, FA.FA_Deadline, 103) >= CAST(GETDATE() AS DATE)       
                    ORDER BY TRY_CONVERT(DATETIME, FA.FA_Deadline, 103) ASC", con))
                {
                    cmd.Parameters.AddWithValue("@StudentID", studentId);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(deadlines);
                    }
                }
            }

            return deadlines;
        }
    }
}