using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WAPP_KiddieCTF.Lecturer
{
    public partial class Challenges : System.Web.UI.Page
    {
        private string currentFilter = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LecturerID"] == null || Session["LecturerName"] == null)
            {
                Response.Redirect("../Default.aspx");
                return;
            }

            lblLecturerID.Text = Session["LecturerID"].ToString();
            lblLecturerName.Text = Session["LecturerName"].ToString();

            if (!IsPostBack)
            {
                BindChallenges("");
                SetActiveTab("All");
            }
        }

        private void BindChallenges(string categoryId)
        {
            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string lecturerId = Session["LecturerID"]?.ToString() ?? "LC001";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                    SELECT c.Challenge_ID, c.Challenge_Name, c.Category_ID, c.Challenge_Difficulty
                    FROM Challenge c
                    WHERE c.Lecturer_ID = @LecturerID
                    AND (@CategoryID = '' OR c.Category_ID = @CategoryID)
                    ORDER BY c.Challenge_ID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@LecturerID", lecturerId);
                    cmd.Parameters.AddWithValue("@CategoryID", categoryId);

                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    conn.Open();
                    sda.Fill(dt);

                    ChallengeRepeater.DataSource = dt;
                    ChallengeRepeater.DataBind();
                }
            }

            currentFilter = categoryId;
            UpdatePanelChallenges.Update();
        }

        protected void Filter_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            string categoryId = btn.CommandArgument;

            BindChallenges(categoryId);
            SetActiveTab(btn.Text);
        }

        private void SetActiveTab(string tabText)
        {
            lnkAll.CssClass = "tab-btn";
            lnkOSINT.CssClass = "tab-btn";
            lnkCrypto.CssClass = "tab-btn";
            lnkStegano.CssClass = "tab-btn";
            lnkReverse.CssClass = "tab-btn";

            switch (tabText)
            {
                case "All": lnkAll.CssClass += " active"; break;
                case "OSINT": lnkOSINT.CssClass += " active"; break;
                case "Cryptography": lnkCrypto.CssClass += " active"; break;
                case "Steganography": lnkStegano.CssClass += " active"; break;
                case "Reverse Engineering": lnkReverse.CssClass += " active"; break;
            }
        }

        protected void lnkEdit_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            string challengeId = btn.CommandArgument;
            Response.Redirect($"EditChallenge.aspx?id={challengeId}");
        }

        protected string GetCategoryName(object categoryIdObj)
        {
            if (categoryIdObj == null) return "";
            string categoryId = categoryIdObj.ToString();

            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT Category_Name FROM Category WHERE Category_ID = @CategoryID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CategoryID", categoryId);
                    conn.Open();
                    return cmd.ExecuteScalar()?.ToString() ?? "";
                }
            }
        }

        protected string GetDifficultyClass(object difficultyObj)
        {
            string difficulty = difficultyObj?.ToString().ToLower() ?? "";
            switch (difficulty)
            {
                case "easy": return "diff-easy";
                case "medium": return "diff-medium";
                case "hard": return "diff-hard";
                default: return "diff-easy";
            }
        }

        protected void lnkViewDetails_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            string challengeId = btn.CommandArgument;
            Response.Redirect($"ChallengeDetails.aspx?id={challengeId}");
        }

    }
}