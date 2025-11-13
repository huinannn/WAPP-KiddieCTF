using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WAPP_KiddieCTF.Student
{
    public partial class ViewDiscussions : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["StudentID"] == null)
            {
                Response.Redirect("~/LogIn.aspx");
                return;
            }

            if (!IsPostBack)
            {
                string discussionID = Request.QueryString["id"];
                LoadDiscussionDetails(discussionID);
                LoadComments(discussionID);
            }
        }

        private void LoadDiscussionDetails(string discussionID)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                string query = @"SELECT Discussion_Title, Discussion_Message, Discussion_Post, 
                         s.Student_Name, Discussion_DateTime 
                         FROM Discussion d 
                         JOIN Student s ON d.Student_ID = s.Student_ID 
                         WHERE Discussion_ID = @Discussion_ID";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Discussion_ID", discussionID);
                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    lblTitle.Text = dr["Discussion_Title"].ToString();
                    lblStudentName.Text = dr["Student_Name"].ToString();
                    lblDateTime.Text = Convert.ToDateTime(dr["Discussion_DateTime"]).ToString("dd/MM/yyyy hh:mm tt");

                    string discussionMessage = dr["Discussion_Message"]?.ToString();
                    lblMessage.Visible = !string.IsNullOrWhiteSpace(discussionMessage);
                    lblMessage.Text = discussionMessage;

                    string discussionPost = dr["Discussion_Post"]?.ToString();
                    imgPost.Visible = !string.IsNullOrWhiteSpace(discussionPost);
                    imgPost.ImageUrl = discussionPost;
                }
            }
        }

        private void LoadComments(string discussionID)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                string query = @"SELECT c.Comment_ID, c.Comment_Message, c.Comment_DateTime, s.Student_Name
                         FROM Comment c
                         JOIN Student s ON c.Student_ID = s.Student_ID
                         WHERE c.Discussion_ID = @Discussion_ID
                         ORDER BY c.Comment_DateTime ASC";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Discussion_ID", discussionID);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                rptComments.DataSource = dt;
                rptComments.DataBind();

                // ✅ Force loading of replies for each comment after binding
                foreach (RepeaterItem item in rptComments.Items)
                {
                    string commentID = ((HiddenField)item.FindControl("hfCommentID")).Value;
                    Repeater rptReplies = (Repeater)item.FindControl("rptReplies");
                    LoadReplies(commentID, rptReplies);
                }
            }
        }


        protected void rptComments_ItemDataBound(object sender, RepeaterItemEventArgs e)
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    string commentID = DataBinder.Eval(e.Item.DataItem, "Comment_ID").ToString();
                    Repeater rptReplies = (Repeater)e.Item.FindControl("rptReplies");

                    // ✅ Only this line is needed — handles loading replies cleanly
                    LoadReplies(commentID, rptReplies);
                }
            }

        protected void btnAddComment_Click(object sender, EventArgs e)
        {
            if (Session["StudentID"] == null)
            {
                Response.Redirect("~/LogIn.aspx");
                return;
            }

            string discussionID = Request.QueryString["id"];
            string studentID = Session["StudentID"].ToString();
            string commentMessage = txtComment.Text.Trim();

            if (string.IsNullOrWhiteSpace(commentMessage))
            {
                // 🟡 Empty comment warning
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", @"
                    Swal.fire({
                        icon: 'warning',
                        title: 'Oops...',
                        text: 'Comment cannot be empty!',
                    });
                ", true);
                return;
            }

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                string newCommentID = "C" + DateTime.Now.Ticks;
                string query = @"INSERT INTO Comment 
                        (Comment_ID, Comment_Message, Discussion_ID, Student_ID) 
                        VALUES (@Comment_ID, @Comment_Message, @Discussion_ID, @Student_ID)";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Comment_ID", newCommentID);
                cmd.Parameters.AddWithValue("@Comment_Message", commentMessage);
                cmd.Parameters.AddWithValue("@Discussion_ID", discussionID);
                cmd.Parameters.AddWithValue("@Student_ID", studentID);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            txtComment.Text = "";

            // ✅ Success message
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", @"
                Swal.fire({
                    icon: 'success',
                    title: 'Comment Posted!',
                    text: 'Your comment has been added successfully.',
                });
            ", true);

            LoadComments(discussionID);
        }

        protected void btnAddReply_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string commentID = btn.CommandArgument;

            RepeaterItem item = (RepeaterItem)btn.NamingContainer;
            TextBox txtReply = (TextBox)item.FindControl("txtReply");
            string replyMessage = txtReply.Text.Trim();

            if (string.IsNullOrWhiteSpace(replyMessage))
            {
                // 🟡 Empty reply warning
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", @"
                    Swal.fire({
                        icon: 'warning',
                        title: 'Oops...',
                        text: 'Reply cannot be empty!',
                    });
                ", true);
                return;
            }

            string studentID = Session["StudentID"].ToString();
            string newReplyID = "R" + DateTime.Now.Ticks;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                string query = @"INSERT INTO Reply 
                         (Reply_ID, Reply_Message, Comment_ID, Student_ID) 
                         VALUES (@Reply_ID, @Reply_Message, @Comment_ID, @Student_ID)";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Reply_ID", newReplyID);
                cmd.Parameters.AddWithValue("@Reply_Message", replyMessage);
                cmd.Parameters.AddWithValue("@Comment_ID", commentID);
                cmd.Parameters.AddWithValue("@Student_ID", studentID);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            txtReply.Text = "";

            // ✅ Success alert
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", @"
                Swal.fire({
                    icon: 'success',
                    title: 'Reply Added!',
                    text: 'Your reply has been posted successfully.',
                });
            ", true);

            Repeater rptReplies = (Repeater)item.FindControl("rptReplies");
            LoadReplies(commentID, rptReplies);
        }

        private void LoadReplies(string commentID, Repeater rptReplies)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                string query = @"SELECT r.Reply_ID, r.Reply_Message, r.Reply_DateTime, s.Student_Name
                         FROM Reply r
                         JOIN Student s ON r.Student_ID = s.Student_ID
                         WHERE r.Comment_ID = @Comment_ID
                         ORDER BY r.Reply_DateTime ASC";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Comment_ID", commentID);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                rptReplies.DataSource = dt;
                rptReplies.DataBind();
            }
        }
    }
}
