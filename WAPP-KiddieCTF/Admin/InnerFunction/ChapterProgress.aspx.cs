using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace WAPP_KiddieCTF.Admin.InnerFunction
{
    public partial class ChapterProgress : System.Web.UI.Page
    {
        private string Course_ID => Request.QueryString["Course_ID"];
        private string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Admin version: NO session check

            if (string.IsNullOrEmpty(Course_ID))
            {
                // Redirect back to Courses.aspx if Course_ID is missing
                Response.Redirect("../Courses.aspx");
                return;
            }

            if (!IsPostBack)
                LoadProgress();
        }

        private void LoadProgress(string search = "")
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                // Step 1: Get total chapters for this course
                string totalQuery = "SELECT COUNT(*) FROM Chapter WHERE Course_ID = @Course_ID";
                SqlCommand totalCmd = new SqlCommand(totalQuery, conn);
                totalCmd.Parameters.AddWithValue("@Course_ID", Course_ID);
                conn.Open();
                int totalChapters = (int)totalCmd.ExecuteScalar();

                // Step 2: Get enrolled students + their completed chapters
                string query = @"
                    SELECT 
                        s.Student_ID,
                        s.Student_Name,
                        ISNULL(pc.CompletedCount, 0) AS CompletedCount
                    FROM Student s
                    INNER JOIN Assigned_Course ac ON s.Student_ID = ac.Student_ID
                    LEFT JOIN (
                        SELECT Student_ID, COUNT(DISTINCT Chapter_ID) AS CompletedCount
                        FROM Progress_Checker
                        WHERE Chapter_ID IN (SELECT Chapter_ID FROM Chapter WHERE Course_ID = @Course_ID)
                        GROUP BY Student_ID
                    ) pc ON s.Student_ID = pc.Student_ID
                    WHERE ac.Course_ID = @Course_ID
                      AND (s.Student_ID LIKE @Search OR s.Student_Name LIKE @Search)
                    ORDER BY s.Student_ID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Course_ID", Course_ID);  // Use Course_ID here
                cmd.Parameters.AddWithValue("@Search", $"%{search}%");

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Add computed columns
                dt.Columns.Add("Completed", typeof(string));
                dt.Columns.Add("TotalChapters", typeof(int));

                foreach (DataRow row in dt.Rows)
                {
                    int completed = Convert.ToInt32(row["CompletedCount"]);
                    row["Completed"] = $"{completed}/{totalChapters}";
                    row["TotalChapters"] = totalChapters;
                }

                dt.Columns.Remove("CompletedCount");

                rptProgress.DataSource = dt;
                rptProgress.DataBind();

                litNoData.Visible = dt.Rows.Count == 0;
            }
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadProgress(txtSearch.Text.Trim());

            string script = @"
                setTimeout(function() {
                    var txt = document.getElementById('" + txtSearch.ClientID + @"');
                    var label = txt.parentNode.querySelector('.placeholder-label');
                    if (txt && label) {
                        if (txt.value.trim() === '') {
                            label.style.opacity = '1';
                            label.style.transform = 'translateY(-50%)';
                            label.style.top = '50%';
                            label.style.fontSize = '18px';
                        } else {
                            label.style.opacity = '0';
                            label.style.transform = 'translateY(-50%) scale(0.8)';
                            label.style.top = '10px';
                            label.style.fontSize = '14px';
                        }
                    }
                }, 150);
            ";

            ScriptManager.RegisterStartupScript(this, GetType(), "ForcePlaceholder", script, true);
        }
    }
}
