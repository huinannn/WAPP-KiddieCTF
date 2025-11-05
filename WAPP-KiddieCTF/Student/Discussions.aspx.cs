using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace WAPP_KiddieCTF.Student
{
    public partial class Discussions : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDiscussions();
            }
        }

        private void LoadDiscussions()
        {
            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                string query = @"
                SELECT 
                    d.*,
                    s.Student_Name,
                    COUNT(c.Comment_ID) AS CommentCount
                FROM Discussion d
                LEFT JOIN Comment c ON d.Discussion_ID = c.Discussion_ID
                LEFT JOIN Student s ON d.Student_ID = s.Student_ID 
                GROUP BY 
                    d.Discussion_ID, d.Discussion_Title, d.Discussion_Message, d.Discussion_Post, 
                    d.Student_ID, s.Student_Name, d.Discussion_DateTime
                ORDER BY d.Discussion_DateTime DESC";

                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                con.Open();
                da.Fill(dt);
                con.Close();

                rptDiscussions.DataSource = dt;
                rptDiscussions.DataBind();
            }
        }

    }
}
