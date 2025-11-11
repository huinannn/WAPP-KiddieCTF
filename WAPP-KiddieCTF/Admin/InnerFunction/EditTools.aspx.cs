using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace WAPP_KiddieCTF.Admin.InnerFunction
{
    public partial class EditTools : System.Web.UI.Page
    {
        private readonly string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCategories();
                LoadTool();
            }
        }

        private void LoadCategories()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                string sql = "SELECT Category_ID, Category_Name FROM Category ORDER BY Category_Name";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    ddlCategory.DataSource = dr;
                    ddlCategory.DataTextField = "Category_Name";
                    ddlCategory.DataValueField = "Category_ID";
                    ddlCategory.DataBind();
                }

                // optional: add a "Select" item
                ddlCategory.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Category --", ""));
            }
        }

        private void LoadTool()
        {
            string toolId = Request.QueryString["Tool_ID"];
            if (string.IsNullOrEmpty(toolId))
            {
                // no id, go back
                Response.Redirect("../Tools.aspx");
                return;
            }

            using (SqlConnection con = new SqlConnection(connStr))
            {
                string sql = @"
                    SELECT Tool_ID, Tool_Name, Tool_Description, Category_ID
                    FROM Tool
                    WHERE Tool_ID = @Tool_ID
                ";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Tool_ID", toolId);
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        lblToolID.Text = dr["Tool_ID"].ToString();
                        // because in .aspx we used <input runat="server" ...> it's HtmlInputControl
                        txtToolName.Value = dr["Tool_Name"].ToString();
                        txtToolDescription.Value = dr["Tool_Description"].ToString();

                        string categoryId = dr["Category_ID"] != DBNull.Value ? dr["Category_ID"].ToString() : "";
                        if (!string.IsNullOrEmpty(categoryId) && ddlCategory.Items.FindByValue(categoryId) != null)
                        {
                            ddlCategory.SelectedValue = categoryId;
                        }
                    }
                    else
                    {
                        // not found
                        Response.Redirect("../Tools.aspx");
                    }
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string toolId = lblToolID.Text;
            if (string.IsNullOrEmpty(toolId))
            {
                return;
            }

            string name = txtToolName.Value.Trim();
            string desc = txtToolDescription.Value.Trim();
            string categoryId = ddlCategory.SelectedValue;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                string sql = @"
                    UPDATE Tool
                    SET Tool_Name = @Tool_Name,
                        Tool_Description = @Tool_Description,
                        Category_ID = CASE WHEN @Category_ID = '' THEN NULL ELSE @Category_ID END
                    WHERE Tool_ID = @Tool_ID
                ";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Tool_Name", name);
                    cmd.Parameters.AddWithValue("@Tool_Description", desc);
                    cmd.Parameters.AddWithValue("@Category_ID", string.IsNullOrEmpty(categoryId) ? (object)DBNull.Value : categoryId);
                    cmd.Parameters.AddWithValue("@Tool_ID", toolId);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            // after update, go back to tools list
            Response.Redirect("../Tools.aspx");
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string toolId = lblToolID.Text;
            if (string.IsNullOrEmpty(toolId))
            {
                return;
            }

            using (SqlConnection con = new SqlConnection(connStr))
            {
                string sql = "DELETE FROM Tool WHERE Tool_ID = @Tool_ID";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@Tool_ID", toolId);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            Response.Redirect("../Tools.aspx");
        }
    }
}
