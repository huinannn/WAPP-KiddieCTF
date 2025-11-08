using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace WAPP_KiddieCTF.Student
{
    public partial class AddDiscussions : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["StudentID"] == null)
            {
                Response.Redirect("~/LogIn.aspx");
                return;
            }
        }

        protected void btnPost_Click(object sender, EventArgs e)
        {
            if (Session["StudentID"] == null)
            {
                Response.Redirect("~/LogIn.aspx");
                return;
            }

            string studentId = Session["StudentID"].ToString();
            string title = txtDiscussionName.Text.Trim();
            string description = txtDescription.Text.Trim();
            string imagePath = "";

            // Validation: must have title + (image OR description)
            if (string.IsNullOrEmpty(title) || (string.IsNullOrEmpty(description) && !fileImage.HasFile))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Please add a title and either a description or an image.');", true);
                return;
            }

            // Upload image if exists
            if (fileImage.HasFile)
            {
                // Validate allowed image types
                string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
                string fileExt = Path.GetExtension(fileImage.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExt))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert",
                        "alert('❌ Invalid file type. Only JPG, JPEG, PNG, and GIF images are allowed.');", true);
                    return;
                }

                // Optional: double-check MIME type
                string contentType = fileImage.PostedFile.ContentType.ToLower();
                if (!contentType.StartsWith("image/"))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert",
                        "alert('❌ Invalid file. Please upload an image file only.');", true);
                    return;
                }

                // Save image to /Images/discussion/
                string folderPath = Server.MapPath("~/Images/discussion/");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string newFileName = "DISC_" + Guid.NewGuid().ToString("N").Substring(0, 8) + fileExt;
                string fullPath = Path.Combine(folderPath, newFileName);
                fileImage.SaveAs(fullPath);

                imagePath = "../Images/discussion/" + newFileName;
            }

            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();

                string newDiscussionID = "";
                string getIdQuery = "SELECT TOP 1 Discussion_ID FROM Discussion ORDER BY Discussion_ID DESC";
                SqlCommand getIdCmd = new SqlCommand(getIdQuery, con);
                object result = getIdCmd.ExecuteScalar();

                if (result != null)
                {
                    string lastId = result.ToString();
                    int num = int.Parse(lastId.Substring(1)) + 1;
                    newDiscussionID = "D" + num.ToString("D3");
                }
                else
                {
                    newDiscussionID = "D001";
                }

                string insertQuery = @"INSERT INTO Discussion 
                    (Discussion_ID, Discussion_Title, Discussion_Message, Discussion_Post, Student_ID)
                    VALUES (@DiscussionID, @Title, @Message, @Post, @StudentID)";

                SqlCommand cmd = new SqlCommand(insertQuery, con);
                cmd.Parameters.AddWithValue("@DiscussionID", newDiscussionID);
                cmd.Parameters.AddWithValue("@Title", title);
                cmd.Parameters.AddWithValue("@Message", description);
                cmd.Parameters.AddWithValue("@Post", imagePath);
                cmd.Parameters.AddWithValue("@StudentID", studentId);

                cmd.ExecuteNonQuery();
                con.Close();
            }

            ClientScript.RegisterStartupScript(this.GetType(), "success",
                "alert('✅ Discussion posted successfully!'); window.location='Discussions.aspx';", true);
        }
    }
}
