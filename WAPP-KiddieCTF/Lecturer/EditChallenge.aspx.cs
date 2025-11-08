using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WAPP_KiddieCTF.Lecturer
{
    public partial class EditChallenge : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        protected string ChallengeID;

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
                ChallengeID = Request.QueryString["id"];
                if (string.IsNullOrEmpty(ChallengeID))
                    Response.Redirect("Challenges.aspx");

                ViewState["ChallengeID"] = ChallengeID;

                LoadCategories();
                LoadChallenge();
            }
            else
            {
                ChallengeID = ViewState["ChallengeID"]?.ToString();
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
            ddlCategory.Items.Insert(0, new ListItem("- Select -", ""));
        }

        private void LoadChallenge()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"SELECT Challenge_Name, Challenge_Description, Challenge_File,
                                 Challenge_Difficulty, Challenge_Answer, Category_ID
                                 FROM Challenge WHERE Challenge_ID = @ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", ChallengeID);
                conn.Open();

                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    if (r.Read())
                    {
                        lblChallengeID.Text = ChallengeID;
                        txtName.Text = r["Challenge_Name"].ToString();
                        txtDescription.Text = r["Challenge_Description"].ToString();
                        lblFileName.Text = r["Challenge_File"].ToString();
                        ddlDifficulty.SelectedValue = r["Challenge_Difficulty"].ToString();
                        ddlCategory.SelectedValue = r["Category_ID"].ToString();
                        txtAnswer.Text = r["Challenge_Answer"].ToString();
                    }
                    else
                    {
                        Response.Redirect("Challenges.aspx");
                    }
                }
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string answer = txtAnswer.Text.Trim();
            string description = txtDescription.Text.Trim();
            string difficulty = ddlDifficulty.SelectedValue;
            string category = ddlCategory.SelectedValue;

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(answer) || string.IsNullOrEmpty(category) || string.IsNullOrEmpty(difficulty))
            {
                ShowAlert("error", "Please fill in all fields!");
                return;
            }

            string fileName = lblFileName.Text;
            if (fuFile.HasFile)
            {
                string ext = Path.GetExtension(fuFile.FileName).ToLower();
                if (ext != ".zip" && ext != ".pdf" && ext != ".docx")
                {
                    ShowAlert("error", "Only ZIP, PDF, DOCX allowed!");
                    return;
                }

                fileName = ChallengeID + ext;
                string path = Server.MapPath("~/Uploads/Challenges/") + fileName;
                fuFile.SaveAs(path);
            }

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"UPDATE Challenge SET
                                Challenge_Name = @Name,
                                Challenge_Answer = @Answer,
                                Challenge_Description = @Desc,
                                Challenge_Difficulty = @Diff,
                                Challenge_File = @File,
                                Category_ID = @Category
                                WHERE Challenge_ID = @ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Answer", answer);
                cmd.Parameters.AddWithValue("@Desc", description);
                cmd.Parameters.AddWithValue("@Diff", difficulty);
                cmd.Parameters.AddWithValue("@File", fileName);
                cmd.Parameters.AddWithValue("@Category", category);
                cmd.Parameters.AddWithValue("@ID", ChallengeID);
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            ShowSuccess("Challenge updated!", "Challenges.aspx");
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string filePath = Server.MapPath("~/Uploads/Challenges/") + lblFileName.Text;
            if (File.Exists(filePath)) File.Delete(filePath);

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "DELETE FROM Challenge WHERE Challenge_ID = @ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID", ChallengeID);
                conn.Open();
                cmd.ExecuteNonQuery();
            }

            ShowSuccess("Challenge deleted!", "Challenges.aspx");
        }

        private void ShowAlert(string icon, string title)
        {
            string script = $"Swal.fire({{ icon: '{icon}', title: '{title}', confirmButtonColor: '#3085d6' }});";
            ScriptManager.RegisterStartupScript(this, GetType(), icon, script, true);
        }

        private void ShowSuccess(string title, string redirect)
        {
            string script = $@"
                Swal.fire({{
                    icon: 'success',
                    title: '{title}',
                    showConfirmButton: false,
                    timer: 1500
                }}).then(() => {{
                    window.location = '{redirect}';
                }});";
            ScriptManager.RegisterStartupScript(this, GetType(), "success", script, true);
        }

    }
}