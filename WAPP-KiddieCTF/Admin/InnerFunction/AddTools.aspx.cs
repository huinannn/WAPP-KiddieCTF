using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace WAPP_KiddieCTF.Admin.InnerFunction
{
    public partial class AddTools : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // admin: no session check
            if (!IsPostBack)
            {
                LoadCategories();
                lblToolID.Text = GenerateNextToolID();
            }
        }

        private void LoadCategories()
        {
            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT Category_ID, Category_Name FROM Category ORDER BY Category_Name";
                SqlCommand cmd = new SqlCommand(query, conn);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    ddlCategory.DataSource = reader;
                    ddlCategory.DataTextField = "Category_Name";
                    ddlCategory.DataValueField = "Category_ID";
                    ddlCategory.DataBind();

                    var defaultItem = new System.Web.UI.WebControls.ListItem("- Select -", "");
                    // some browsers ignore disabled selected POST, but okay for display
                    defaultItem.Attributes["disabled"] = "disabled";
                    defaultItem.Selected = true;
                    ddlCategory.Items.Insert(0, defaultItem);
                }
                catch (Exception ex)
                {
                    // simple error display (you can change to label)
                    ScriptManager.RegisterStartupScript(this, GetType(), "catErr",
                        $"Swal.fire('Error', 'Error loading categories: {ex.Message}', 'error');", true);
                }
            }
        }

        // Auto-generate next Tool ID (T001, T002, ...)
        private string GenerateNextToolID()
        {
            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string newToolID = "T001";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT TOP 1 Tool_ID FROM Tool ORDER BY Tool_ID DESC";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    string lastID = result.ToString();   // e.g. T010
                    int numPart = 0;
                    if (lastID.Length > 1 && int.TryParse(lastID.Substring(1), out numPart))
                    {
                        numPart++;
                        newToolID = "T" + numPart.ToString("D3");
                    }
                }
            }

            return newToolID;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string toolId = lblToolID.Text;
            string toolName = txtToolName.Value.Trim();
            string toolDesc = txtToolDescription.Value.Trim();
            string categoryId = ddlCategory.SelectedValue;

            if (string.IsNullOrEmpty(toolName) || string.IsNullOrEmpty(toolDesc) || string.IsNullOrEmpty(categoryId))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "warn",
                    "Swal.fire('Warning', 'Please fill in all fields.', 'warning');", true);
                return;
            }

            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string insertQuery = @"
                    INSERT INTO Tool (Tool_ID, Tool_Name, Tool_Description, Category_ID)
                    VALUES (@id, @name, @desc, @catId)";
                SqlCommand cmd = new SqlCommand(insertQuery, conn);
                cmd.Parameters.AddWithValue("@id", toolId);
                cmd.Parameters.AddWithValue("@name", toolName);
                cmd.Parameters.AddWithValue("@desc", toolDesc);
                cmd.Parameters.AddWithValue("@catId", categoryId);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();

                    string successScript = @"
                        Swal.fire({
                            title: 'Success!',
                            text: 'New tool added successfully!',
                            icon: 'success',
                            confirmButtonText: 'OK'
                        }).then((result) => {
                            if (result.isConfirmed) {
                                window.location.href = '../Tools.aspx';
                            }
                        });";

                    ScriptManager.RegisterStartupScript(this, GetType(), "successAlert", successScript, true);
                }
                catch (Exception ex)
                {
                    string errorScript = $"Swal.fire('Error', 'Error adding tool: {ex.Message}', 'error');";
                    ScriptManager.RegisterStartupScript(this, GetType(), "errorAlert", errorScript, true);
                }
            }
        }
    }
}
