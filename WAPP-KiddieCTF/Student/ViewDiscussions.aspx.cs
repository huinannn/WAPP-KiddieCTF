using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace WAPP_KiddieCTF.Student
{
    public partial class ViewDiscussions : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["StudentID"] == null)
            {
                Response.Redirect("~/Default.aspx");
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

                    // Handle discussion message visibility
                    string discussionMessage = dr["Discussion_Message"]?.ToString();
                    if (!string.IsNullOrWhiteSpace(discussionMessage))
                    {
                        lblMessage.Text = discussionMessage;
                        lblMessage.Visible = true;
                    }
                    else
                    {
                        lblMessage.Visible = false;
                    }

                    // Handle image visibility
                    string discussionPost = dr["Discussion_Post"]?.ToString();
                    if (!string.IsNullOrWhiteSpace(discussionPost))
                    {
                        imgPost.ImageUrl = discussionPost;
                        imgPost.Visible = true;
                    }
                    else
                    {
                        imgPost.Visible = false;
                    }
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
                         WHERE c.Discussion_ID = @Discussion_ID";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Discussion_ID", discussionID);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                rptComments.DataSource = dt;
                rptComments.DataBind();
            }
        }

        protected void rptComments_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string commentID = DataBinder.Eval(e.Item.DataItem, "Comment_ID").ToString();
                Repeater rptReplies = (Repeater)e.Item.FindControl("rptReplies");

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
                {
                    string query = @"SELECT r.Reply_Message, r.Reply_DateTime, s.Student_Name
                             FROM Reply r
                             JOIN Student s ON r.Student_ID = s.Student_ID
                             WHERE r.Comment_ID = @Comment_ID";
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

        protected void btnAddComment_Click(object sender, EventArgs e)
        {
            // Check if user is logged in
            if (Session["StudentID"] == null)
            {
                Response.Redirect("~/Default.aspx");
                return;
            }

            string discussionID = Request.QueryString["id"];
            string studentID = Session["StudentID"].ToString();
            string commentMessage = txtComment.Text.Trim();

            if (string.IsNullOrWhiteSpace(commentMessage))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Comment cannot be empty!');", true);
                return;
            }

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                string newCommentID = "C" + DateTime.Now.Ticks; // simple unique ID
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
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Comment posted successfully!');", true);
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Reply cannot be empty!');", true);
                return;
            }

            string studentID = Session["StudentID"].ToString();

            // Generate sequential Reply_ID
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
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Reply posted successfully!');", true);

            Repeater rptReplies = (Repeater)item.FindControl("rptReplies");
            LoadReplies(commentID, rptReplies);
        }

        // New helper method to load replies for a specific comment
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
