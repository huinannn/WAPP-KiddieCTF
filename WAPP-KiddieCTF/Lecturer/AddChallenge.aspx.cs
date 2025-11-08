using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace WAPP_KiddieCTF.Lecturer
{
    public partial class AddChallenge : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LecturerID"] == null || Session["LecturerName"] == null)
            {
                Response.Redirect("../LogIn.aspx");
                return;
            }

            lblLecturerID.Text = Session["LecturerID"].ToString();
            lblLecturerName.Text = Session["LecturerName"].ToString();

            if (!IsPostBack)
            {
                LoadCategories();
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

        protected void btnDone_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string flag = txtFlag.Text.Trim();
            string description = txtDescription.Text.Trim();
            string difficulty = ddlDifficulty.SelectedValue;
            string category = ddlCategory.SelectedValue;

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(flag) || string.IsNullOrEmpty(category) || string.IsNullOrEmpty(difficulty))
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
            string path = Server.MapPath("~/Uploads/Challenges/") + fileName;
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            fuFile.SaveAs(path);

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"INSERT INTO Challenge 
    (Challenge_ID, Challenge_Name, Challenge_Answer, Challenge_Description, Challenge_Difficulty, Challenge_File, Category_ID, Lecturer_ID)
    VALUES (@ID, @Name, @Answer, @Desc, @Diff, @File, @Category, @Lecturer)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", lblChallengeID.Text);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Answer", flag);
                cmd.Parameters.AddWithValue("@Desc", description);
                cmd.Parameters.AddWithValue("@Diff", difficulty);
                cmd.Parameters.AddWithValue("@File", fileName);
                cmd.Parameters.AddWithValue("@Category", category);
                cmd.Parameters.AddWithValue("@Lecturer", Session["LecturerID"].ToString());

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
                    window.location = 'Challenges.aspx';
                });";
            ScriptManager.RegisterStartupScript(this, GetType(), "success", script, true);
        }

        private void ShowAlert(string icon, string title)
        {
            string script = $"Swal.fire({{ icon: '{icon}', title: '{title}', confirmButtonColor: '#3085d6' }});";
            ScriptManager.RegisterStartupScript(this, GetType(), icon, script, true);
        }

    }
}