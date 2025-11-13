using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;

namespace WAPP_KiddieCTF.Admin.InnerFunction
{
    public partial class AddChallenge : System.Web.UI.Page
    {
        private readonly string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCategories();
                LoadLecturers();
                GenerateNextChallengeID();
            }
        }

        private void LoadCategories()
        {
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
                reader.Close();
            }
            ddlCategory.Items.Insert(0, new System.Web.UI.WebControls.ListItem("- Select -", ""));
        }

        private void LoadLecturers()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT Lecturer_ID, Lecturer_Name FROM Lecturer";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                ddlLecturer.DataSource = reader;
                ddlLecturer.DataTextField = "Lecturer_Name";
                ddlLecturer.DataValueField = "Lecturer_ID";
                ddlLecturer.DataBind();
                reader.Close();
            }
            ddlLecturer.Items.Insert(0, new System.Web.UI.WebControls.ListItem("- Select Lecturer -", ""));
        }

        private void GenerateNextChallengeID()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT MAX(CAST(SUBSTRING(Challenge_ID, 3, LEN(Challenge_ID)) AS INT)) FROM Challenge WHERE Challenge_ID LIKE 'CH%'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                object result = cmd.ExecuteScalar();
                int next = (result == DBNull.Value) ? 1 : Convert.ToInt32(result) + 1;
                lblChallengeID.Text = "CH" + next.ToString("D3");
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string flag = txtFlag.Text.Trim();
            string description = txtDescription.Text.Trim();
            string difficulty = ddlDifficulty.SelectedValue;
            string category = ddlCategory.SelectedValue;
            string lecturer = ddlLecturer.SelectedValue;

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(flag) || string.IsNullOrEmpty(category) ||
                string.IsNullOrEmpty(difficulty) || string.IsNullOrEmpty(lecturer))
            {
                ShowAlert("error", "Please fill in all required fields!");
                return;
            }

            if (!fuFile.HasFile)
            {
                ShowAlert("error", "Please attach a file!");
                return;
            }

            string ext = Path.GetExtension(fuFile.FileName).ToLower();
            if (ext != ".zip" && ext != ".pdf" && ext != ".docx")
            {
                ShowAlert("error", "Only ZIP, PDF, or DOCX files are allowed!");
                return;
            }

            string fileName = lblChallengeID.Text + ext;
            string folder = Server.MapPath("~/Uploads/Challenges/");
            Directory.CreateDirectory(folder);
            string fullPath = Path.Combine(folder, fileName);
            fuFile.SaveAs(fullPath);

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                    INSERT INTO Challenge
                        (Challenge_ID, Challenge_Name, Challenge_Answer, Challenge_Description, Challenge_Difficulty, Challenge_File, Category_ID, Lecturer_ID)
                    VALUES
                        (@ID, @Name, @Answer, @Desc, @Diff, @File, @Category, @Lecturer)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", lblChallengeID.Text);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Answer", flag);
                cmd.Parameters.AddWithValue("@Desc", description);
                cmd.Parameters.AddWithValue("@Diff", difficulty);
                cmd.Parameters.AddWithValue("@File", fileName);
                cmd.Parameters.AddWithValue("@Category", category);
                cmd.Parameters.AddWithValue("@Lecturer", lecturer);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            string script = @"
                Swal.fire({
                    icon: 'success',
                    title: 'Challenge added!',
                    showConfirmButton: false,
                    timer: 1500
                }).then(() => {
                    window.location = '../Challenges.aspx';
                });";
            ScriptManager.RegisterStartupScript(this, GetType(), "success", script, true);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Challenges.aspx");
        }

        private void ShowAlert(string icon, string title)
        {
            string script = $"Swal.fire({{ icon: '{icon}', title: '{title}', confirmButtonColor: '#3085d6' }});";
            ScriptManager.RegisterStartupScript(this, GetType(), icon, script, true);
        }
    }
}
