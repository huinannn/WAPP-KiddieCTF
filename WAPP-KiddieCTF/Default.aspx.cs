using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace WAPP_KiddieCTF
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                LoadCourses();
            }
        }

        private void LoadCourses()
        {
            // Replace with your actual connection string (from Web.config ideally)
            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                string query = "SELECT Course_Name FROM Course";
                SqlCommand cmd = new SqlCommand(query, con);

                try
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    rptCourses.DataSource = reader;
                    rptCourses.DataBind();
                    con.Close();
                }
                catch (Exception ex)
                {
                    // Optional: log or show error for debugging
                    Response.Write("<script>alert('Error loading courses: " + ex.Message + "');</script>");
                }
            }
        }
    }
}
