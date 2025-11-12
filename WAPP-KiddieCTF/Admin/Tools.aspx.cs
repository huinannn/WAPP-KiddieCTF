using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace WAPP_KiddieCTF.Admin
{
    public partial class Tools : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadTools();
            }
        }

        private void LoadTools()
        {
            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                string sql = @"
                    SELECT t.Tool_ID, t.Tool_Name, t.Tool_Description, c.Category_Name
                    FROM Tool t
                    LEFT JOIN Category c ON t.Category_ID = c.Category_ID
                    ORDER BY t.Tool_Name
                ";

                SqlDataAdapter da = new SqlDataAdapter(sql, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                rptTools.DataSource = dt;
                rptTools.DataBind();
            }
        }
    }
}
