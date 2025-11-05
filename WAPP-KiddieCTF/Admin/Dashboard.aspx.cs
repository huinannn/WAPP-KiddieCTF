using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace WAPP_KiddieCTF.Admin
{
    public partial class Dashboard : System.Web.UI.Page
    {
        // same style as your other pages
        private readonly string connectionString =
            ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCounts();
                // later you can call methods to load intake / most accessed
            }
        }

        private void LoadCounts()
        {
            // get total students
            int studentCount = GetTableCount("Student");
            // get total lecturers
            int lecturerCount = GetTableCount("Lecturer");

            // put into the labels on the page
            lblStudentsCount.Text = studentCount.ToString();
            lblLecturerCount.Text = lecturerCount.ToString();
        }

        /// <summary>
        /// Returns COUNT(*) from the given table.
        /// Assumes table exists in the same database as your connection string.
        /// </summary>
        private int GetTableCount(string tableName)
        {
            // use [] in case table name is reserved
            string query = $"SELECT COUNT(*) FROM [{tableName}]";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                        return Convert.ToInt32(result);
                }
            }
            catch
            {
                // if something goes wrong, just return 0 so the page still renders
                return 0;
            }

            return 0;
        }
    }
}
