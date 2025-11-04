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
    public partial class Courses : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCourses("");
            }
        }

        private void BindCourses(string searchTerm)
        {
            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                SELECT Course_ID, Course_Name 
                FROM Course 
                WHERE Lecturer_ID = @LecturerID 
                AND (@SearchTerm IS NULL OR @SearchTerm = '' OR Course_Name LIKE '%' + @SearchTerm + '%')
                ORDER BY Course_ID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    string lecturerId = Session["LecturerID"]?.ToString() ?? "LC001"; // From login session
                    cmd.Parameters.AddWithValue("@LecturerID", lecturerId);
                    cmd.Parameters.AddWithValue("@SearchTerm", searchTerm);

                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);

                    conn.Open();
                    sda.Fill(dt);
                    CourseRepeater.DataSource = dt;
                    CourseRepeater.DataBind();
                }
            }
        }

        // Search Event (Triggers on every keystroke)
        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();
            BindCourses(searchTerm);

            UpdatePanelSearch.Update();
            UpdatePanelCourses.Update();

            // FORCE placeholder update after postback
            string script = @"
                <script type='text/javascript'>
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
                                label.style.fontSize = '14ography, px';
                            }
                        }
                    }, 150);
                </script>";

            ScriptManager.RegisterStartupScript(this, GetType(), "ForcePlaceholder", script, false);
        }

        protected void lnkEdit_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            string courseId = btn.CommandArgument;
            Response.Redirect($"EditCourse.aspx?id={courseId}");
        }

        protected void lnkCourseCard_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            string courseId = btn.CommandArgument;
            Response.Redirect($"CourseDetails.aspx?id={courseId}");
        }

    }
}