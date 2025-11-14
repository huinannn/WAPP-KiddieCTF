using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace WAPP_KiddieCTF.Admin.InnerFunction
{
    public partial class AssignmentProgress : System.Web.UI.Page
    {
        private string Course_ID => Request.QueryString["Course_ID"]; // Using Course_ID query parameter
        private string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Admin version: No session check needed for Admin

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
                string query = @"
                    SELECT 
                        s.Student_ID,
                        s.Student_Name,
                        a.Answer_ID,
                        CASE 
                            WHEN m.Marking_ID IS NOT NULL THEN 'Graded'
                            WHEN a.Answer_ID IS NOT NULL THEN 'Submitted'
                            ELSE 'No Submission'
                        END AS Status
                    FROM Student s
                    INNER JOIN Assigned_Course ac ON s.Student_ID = ac.Student_ID
                    LEFT JOIN Answer_Table a ON s.Student_ID = a.Student_ID 
                        AND a.FA_ID = (SELECT TOP 1 FA_ID FROM Final_Assignment WHERE Course_ID = @Course_ID)
                    LEFT JOIN Marking_Table m ON a.Answer_ID = m.Answer_ID
                    WHERE ac.Course_ID = @Course_ID
                      AND (s.Student_ID LIKE @Search OR s.Student_Name LIKE @Search)
                    ORDER BY s.Student_ID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Course_ID", Course_ID); // Use Course_ID here
                cmd.Parameters.AddWithValue("@Search", $"%{search}%");

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

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
            UpdatePanelProgress.Update();
        }
    }
}
