using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace WAPP_KiddieCTF.Admin
{
    public partial class Courses : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // admin: no session needed
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
                // admin sees ALL courses, so no WHERE Lecturer_ID = ...
                string query = @"
                    SELECT Course_ID, Course_Name
                    FROM Course
                    WHERE (@SearchTerm IS NULL OR @SearchTerm = '' OR Course_Name LIKE '%' + @SearchTerm + '%')
                    ORDER BY Course_ID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@SearchTerm", searchTerm ?? string.Empty);

                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);

                    conn.Open();
                    sda.Fill(dt);
                    CourseRepeater.DataSource = dt;
                    CourseRepeater.DataBind();
                }
            }
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();
            BindCourses(searchTerm);

            UpdatePanelSearch.Update();
            UpdatePanelCourses.Update();

            // force placeholder update
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
                }, 150);";

            ScriptManager.RegisterStartupScript(this, GetType(), "ForcePlaceholder", script, true);
        }

        protected void lnkEdit_Click(object sender, EventArgs e)
        {
            var btn = (System.Web.UI.WebControls.LinkButton)sender;
            string courseId = btn.CommandArgument;
            Response.Redirect("InnerFunction/EditCourse.aspx?id=" + courseId);
        }
    }
}
