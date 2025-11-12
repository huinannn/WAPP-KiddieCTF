using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WAPP_KiddieCTF.Admin.InnerFunction
{
    public partial class EditChallenge : Page
    {
        private readonly string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCategories();
                LoadLecturers();
                LoadChallenge();
            }
        }

        private void ShowNotice(string html)
        {
            phMessage.Visible = true;
            litMessage.Text = html;
        }

        private void LoadCategories()
        {
            using (var con = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("SELECT Category_ID, Category_Name FROM Category ORDER BY Category_Name", con))
            {
                con.Open();
                ddlCategory.DataSource = cmd.ExecuteReader();
                ddlCategory.DataTextField = "Category_Name";
                ddlCategory.DataValueField = "Category_ID";
                ddlCategory.DataBind();
                ddlCategory.Items.Insert(0, new ListItem("-- Select Category --", ""));
            }
        }

        private void LoadLecturers()
        {
            using (var con = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("SELECT Lecturer_ID, Lecturer_Name FROM Lecturer ORDER BY Lecturer_Name", con))
            {
                con.Open();
                ddlLecturer.DataSource = cmd.ExecuteReader();
                ddlLecturer.DataTextField = "Lecturer_Name";
                ddlLecturer.DataValueField = "Lecturer_ID";
                ddlLecturer.DataBind();
                ddlLecturer.Items.Insert(0, new ListItem("-- Select Lecturer --", ""));
            }
        }

        private void LoadChallenge()
        {
            var id = Request.QueryString["id"];
            if (string.IsNullOrWhiteSpace(id))
            {
                Response.Redirect("../Challenges.aspx");
                return;
            }

            using (var con = new SqlConnection(connStr))
            using (var cmd = new SqlCommand(@"
                SELECT Challenge_ID, Challenge_Name, Category_ID, Challenge_Difficulty,
                       Challenge_Description, Challenge_File, Lecturer_ID
                FROM Challenge
                WHERE Challenge_ID = @id", con))
            {
                cmd.Parameters.AddWithValue("@id", id.Trim());
                con.Open();
                using (var rd = cmd.ExecuteReader())
                {
                    if (!rd.Read())
                    {
                        Response.Redirect("../Challenges.aspx");
                        return;
                    }

                    lblChallengeID.Text = rd["Challenge_ID"].ToString();
                    txtName.Text = rd["Challenge_Name"].ToString();
                    txtDescription.Text = rd["Challenge_Description"]?.ToString();

                    // preselect dropdowns if value exists
                    var cat = rd["Category_ID"] as string;
                    if (!string.IsNullOrEmpty(cat) && ddlCategory.Items.FindByValue(cat) != null)
                        ddlCategory.SelectedValue = cat;

                    var diff = rd["Challenge_Difficulty"]?.ToString();
                    if (!string.IsNullOrEmpty(diff) && ddlDifficulty.Items.FindByValue(diff) != null)
                        ddlDifficulty.SelectedValue = diff;

                    var lec = rd["Lecturer_ID"] as string;
                    if (!string.IsNullOrEmpty(lec) && ddlLecturer.Items.FindByValue(lec) != null)
                        ddlLecturer.SelectedValue = lec;

                    var file = rd["Challenge_File"]?.ToString();
                    hfExistingFile.Value = file ?? "";
                    litExistingFile.Text = string.IsNullOrEmpty(file)
                        ? "<div class='file-hint'>No file uploaded.</div>"
                        : $"<div class='file-hint'>Current file: <span>{Server.HtmlEncode(file)}</span></div>";
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            var id = lblChallengeID.Text;
            if (string.IsNullOrEmpty(id))
            {
                ShowNotice("<b>Invalid Challenge ID.</b>");
                return;
            }

            // basic validation like your linear checks style
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                ShowNotice("<b>Challenge Name</b> is required.");
                return;
            }
            if (string.IsNullOrWhiteSpace(ddlDifficulty.SelectedValue))
            {
                ShowNotice("Please select a <b>Difficulty</b>.");
                return;
            }

            string newFileName = null;
            string uploads = Server.MapPath("~/Uploads/Challenges/");
            if (fuAttachment.HasFile)
            {
                if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);
                var safeName = Path.GetFileName(fuAttachment.FileName);
                newFileName = id + "_" + safeName;
                var savePath = Path.Combine(uploads, newFileName);
                fuAttachment.SaveAs(savePath);

                // delete old file if replaced
                var old = hfExistingFile.Value;
                if (!string.IsNullOrEmpty(old))
                {
                    var oldPath = Path.Combine(uploads, old);
                    if (File.Exists(oldPath))
                        File.Delete(oldPath);
                }
            }

            using (var con = new SqlConnection(connStr))
            using (var cmd = new SqlCommand(@"
                UPDATE Challenge
                   SET Challenge_Name        = @name,
                       Category_ID           = NULLIF(@cat, ''),
                       Challenge_Difficulty  = @diff,
                       Challenge_Description = @desc,
                       Lecturer_ID           = NULLIF(@lec, ''),
                       Challenge_File        = COALESCE(@file, Challenge_File)
                 WHERE Challenge_ID          = @id", con))
            {
                cmd.Parameters.AddWithValue("@name", txtName.Text.Trim());
                cmd.Parameters.AddWithValue("@cat", ddlCategory.SelectedValue ?? "");
                cmd.Parameters.AddWithValue("@diff", ddlDifficulty.SelectedValue);
                cmd.Parameters.AddWithValue("@desc", (txtDescription.Text ?? "").Trim());
                cmd.Parameters.AddWithValue("@lec", ddlLecturer.SelectedValue ?? "");
                if (newFileName == null)
                    cmd.Parameters.AddWithValue("@file", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@file", newFileName);
                cmd.Parameters.AddWithValue("@id", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            Response.Redirect("../Challenges.aspx");
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            var id = lblChallengeID.Text;
            if (string.IsNullOrEmpty(id)) return;

            // optional: block delete if solved exists (safer UX)
            using (var con = new SqlConnection(connStr))
            using (var check = new SqlCommand("SELECT COUNT(1) FROM Challenge_Solved WHERE Challenge_ID=@id", con))
            {
                check.Parameters.AddWithValue("@id", id);
                con.Open();
                int used = Convert.ToInt32(check.ExecuteScalar());
                if (used > 0)
                {
                    ShowNotice("This challenge has solve records and cannot be deleted.");
                    return;
                }
            }

            using (var con = new SqlConnection(connStr))
            using (var cmd = new SqlCommand("DELETE FROM Challenge WHERE Challenge_ID=@id", con))
            {
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }

            // remove file if existed
            var file = hfExistingFile.Value;
            if (!string.IsNullOrEmpty(file))
            {
                var uploads = Server.MapPath("~/Uploads/Challenges/");
                var path = Path.Combine(uploads, file);
                if (File.Exists(path)) File.Delete(path);
            }

            Response.Redirect("../Challenges.aspx");
        }
    }
}
