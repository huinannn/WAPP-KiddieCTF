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
        // your web.config should have this
        private readonly string connStr =
            ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadRecentAccess();
                LoadCounts();
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

            // we'll merge courses + challenges into 1 table
            DataTable all = new DataTable();
            all.Columns.Add("ItemType");     // COURSE / CHALLENGE
            all.Columns.Add("ItemID");
            all.Columns.Add("ItemName");
            all.Columns.Add("LecturerID");
            all.Columns.Add("AccessDate", typeof(DateTime));

            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();

                // ------------------- COURSES -------------------
                // Access_Course_Record: ACouR_ID, Student_ID, ACouR_Date, Course_ID
                using (SqlCommand cmd = new SqlCommand(@"
                    SELECT TOP 20 acr.Course_ID,
                           acr.ACouR_Date,
                           c.Course_Name,
                           c.Lecturer_ID
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
                            DateTime accessDate = DateTime.MinValue;
                            if (dr["ACouR_Date"] != DBNull.Value)
                                accessDate = Convert.ToDateTime(dr["ACouR_Date"]);

                            all.Rows.Add(
                                "COURSE",
                                dr["Course_ID"].ToString(),
                                dr["Course_Name"].ToString(),
                                dr["Lecturer_ID"] == DBNull.Value ? "" : dr["Lecturer_ID"].ToString(),
                                accessDate
                            );
                        }
                    }
                }

                // ------------------- CHALLENGES -------------------
                // Access_Challenge_Record: AChaR_ID, Student_ID, AChaR_Date, Challenge_ID
                // Challenge: Challenge_ID, Challenge_Name, Lecturer_ID, ...
                using (SqlCommand cmd = new SqlCommand(@"
                    SELECT TOP 20 acr.Challenge_ID,
                           acr.AChaR_Date,
                           ch.Challenge_Name,
                           ch.Lecturer_ID
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
                            DateTime accessDate = DateTime.MinValue;
                            if (dr["AChaR_Date"] != DBNull.Value)
                                accessDate = Convert.ToDateTime(dr["AChaR_Date"]);

                            all.Rows.Add(
                                "CHALLENGE",
                                dr["Challenge_ID"].ToString(),
                                dr["Challenge_Name"].ToString(),
                                dr["Lecturer_ID"] == DBNull.Value ? "" : dr["Lecturer_ID"].ToString(),
                                accessDate
                            );
                        }
                    }
                }
            }

            // newest first, only 10
            var ordered = all.AsEnumerable()
                             .OrderByDescending(r => r.Field<DateTime>("AccessDate"))
                             .Take(10);

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

            // total final assignments answered
            int doneFA = GetCompletedFACount(studentId);
            lblCompletedFA.Text = doneFA.ToString();

            // total certificates
            int certs = GetCertificateCount(studentId);
            lblCertificates.Text = certs.ToString();

            // challenge progress
            var challengeData = GetChallengeProgress(studentId);
            lblChallengeProgress.Text = $"Completed {challengeData.solved}/{challengeData.total} Challenges";
        }

        // Answer_Table: count DISTINCT FA_ID for this student
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

        // Certificate: count rows for this student
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

        // Challenge progress: solved / total
        // solved from Challenge_Solved, total from Challenge
        private (int solved, int total) GetChallengeProgress(string studentId)
        {
            int solved = 0;
            int total = 0;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();

                // total challenges (ignore the NULL row)
                using (SqlCommand cmdTotal = new SqlCommand(@"
                    SELECT COUNT(*)
                    FROM Challenge
                    WHERE Challenge_ID IS NOT NULL", con))
                {
                    object result = cmdTotal.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                        total = Convert.ToInt32(result);
                }

                // solved by this student
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
    }
}
