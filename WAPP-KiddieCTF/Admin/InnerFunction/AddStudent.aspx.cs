using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WAPP_KiddieCTF.Admin.InnerFunction
{
    public partial class AddStudent : System.Web.UI.Page
    {
        // /Admin/InnerFunction/AddStudent.aspx?course=CR001&from=add
        private string CourseID => Request.QueryString["course"];

        protected void Page_Load(object sender, EventArgs e)
        {
            // ADMIN VERSION: no lecturer session check

            if (string.IsNullOrEmpty(CourseID))
            {
                // no course passed, go back to admin courses
                Response.Redirect("../Courses.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadStudents();
            }
        }

        private void LoadStudents(string search = "")
        {
            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                    SELECT s.Student_ID, s.Student_Name, s.Intake_Code
                    FROM Student s
                    WHERE (s.Student_ID   LIKE @Search
                        OR s.Student_Name LIKE @Search
                        OR s.Intake_Code  LIKE @Search)
                    ORDER BY s.Student_ID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Search", "%" + search + "%");

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }

            // remove students already in temp session
            List<string> tempStudents = Session["TempStudents"] as List<string> ?? new List<string>();
            IEnumerable<DataRow> filtered = dt.AsEnumerable()
                                              .Where(r => !tempStudents.Contains(r["Student_ID"].ToString()));

            DataTable finalTable;
            if (filtered.Any())
            {
                finalTable = filtered.CopyToDataTable();
            }
            else
            {
                finalTable = dt.Clone(); // empty table with same schema
            }

            StudentRepeater.DataSource = finalTable;
            StudentRepeater.DataBind();

            pnlNoResults.Visible = finalTable.Rows.Count == 0;
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadStudents(txtSearch.Text.Trim());
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string studentId = btn.CommandArgument;

            // store in temp session list (same behaviour as your AddNewCourse flow)
            List<string> tempStudents = Session["TempStudents"] as List<string> ?? new List<string>();
            if (!tempStudents.Contains(studentId))
            {
                tempStudents.Add(studentId);
            }
            Session["TempStudents"] = tempStudents;

            // reload so newly-added student disappears from list
            LoadStudents(txtSearch.Text.Trim());

            // alert
            ScriptManager.RegisterStartupScript(this, GetType(), "AddTemp",
                "Swal.fire({ icon: 'success', title: 'Student added temporarily!', background:'#1B263B', color:'#fff' });",
                true);
        }
    }
}
