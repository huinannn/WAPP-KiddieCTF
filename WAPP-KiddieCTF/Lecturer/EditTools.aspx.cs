using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace WAPP_KiddieCTF.Lecturer
{
    public partial class EditTools : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LecturerID"] == null)
            {
                Response.Redirect("~/Default.aspx");
                return;
            }

            if (!IsPostBack)
            {
                string toolId = Request.QueryString["Tool_ID"];
                if (!string.IsNullOrEmpty(toolId))
                {
                    LoadCategories();
                    LoadToolDetails(toolId);
                }
                else
                {
                    Response.Redirect("Tools.aspx");
                }
            }
        }

        // 🔹 Load categories for dropdown
        private void LoadCategories()
        {
            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT Category_ID, Category_Name FROM Category";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                ddlCategory.DataSource = reader;
                ddlCategory.DataTextField = "Category_Name";
                ddlCategory.DataValueField = "Category_ID";
                ddlCategory.DataBind();
            }
        }

        // 🔹 Load existing tool details
        private void LoadToolDetails(string toolId)
        {
            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT Tool_ID, Tool_Name, Tool_Description, Category_ID FROM Tool WHERE Tool_ID = @ToolID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ToolID", toolId);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    lblToolID.Text = reader["Tool_ID"].ToString();
                    txtToolName.Value = reader["Tool_Name"].ToString();
                    txtToolDescription.Value = reader["Tool_Description"].ToString();
                    ddlCategory.SelectedValue = reader["Category_ID"].ToString();
                }
            }
        }

        // 🔹 Update tool on submit
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string toolId = lblToolID.Text;
            string toolName = txtToolName.Value.Trim();
            string toolDesc = txtToolDescription.Value.Trim();
            string categoryId = ddlCategory.SelectedValue;

            if (string.IsNullOrEmpty(toolName) || string.IsNullOrEmpty(toolDesc) || string.IsNullOrEmpty(categoryId))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert",
                    "Swal.fire('Warning', 'Please fill in all fields.', 'warning');", true);
                return;
            }

            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string updateQuery = "UPDATE Tool SET Tool_Name=@name, Tool_Description=@desc, Category_ID=@cat WHERE Tool_ID=@id";
                SqlCommand cmd = new SqlCommand(updateQuery, conn);
                cmd.Parameters.AddWithValue("@id", toolId);
                cmd.Parameters.AddWithValue("@name", toolName);
                cmd.Parameters.AddWithValue("@desc", toolDesc);
                cmd.Parameters.AddWithValue("@cat", categoryId);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();

                    // SweetAlert success + redirect back
                    string successScript = @"
                        Swal.fire({
                            title: 'Updated!',
                            text: 'Tool details updated successfully!',
                            icon: 'success',
                            confirmButtonText: 'OK'
                        }).then((result) => {
                            if (result.isConfirmed) {
                                window.location.href = 'Tools.aspx';
                            }
                        });";
                    ScriptManager.RegisterStartupScript(this, GetType(), "successAlert", successScript, true);
                }
                catch (Exception ex)
                {
                    string errorScript = $"Swal.fire('Error', 'Error updating tool: {ex.Message}', 'error');";
                    ScriptManager.RegisterStartupScript(this, GetType(), "errorAlert", errorScript, true);
                }
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string toolId = lblToolID.Text;
            if (string.IsNullOrEmpty(toolId))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert",
                    "Swal.fire('Error', 'No tool selected to delete.', 'error');", true);
                return;
            }

            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string deleteQuery = "DELETE FROM Tool WHERE Tool_ID = @id";
                SqlCommand cmd = new SqlCommand(deleteQuery, conn);
                cmd.Parameters.AddWithValue("@id", toolId);

                try
                {
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        string successScript = @"
                    Swal.fire({
                        title: 'Deleted!',
                        text: 'The tool has been deleted successfully.',
                        icon: 'success',
                        confirmButtonText: 'OK'
                    }).then((result) => {
                        if (result.isConfirmed) {
                            window.location.href = 'Tools.aspx';
                        }
                    });";
                        ScriptManager.RegisterStartupScript(this, GetType(), "deleteSuccess", successScript, true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "deleteFail",
                            "Swal.fire('Error', 'Tool not found or already deleted.', 'error');", true);
                    }
                }
                catch (Exception ex)
                {
                    string errorScript = $"Swal.fire('Error', 'Failed to delete tool: {ex.Message}', 'error');";
                    ScriptManager.RegisterStartupScript(this, GetType(), "deleteError", errorScript, true);
                }
            }
        }
    }
}
