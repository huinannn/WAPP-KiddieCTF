using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WAPP_KiddieCTF.Admin
{
    public partial class Challenges : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindChallenges("");
                SetActiveTab("All");
            }
        }

        private void BindChallenges(string categoryId)
        {
            string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                    SELECT ch.Challenge_ID,
                           ch.Challenge_Name,
                           ch.Category_ID,
                           ch.Challenge_Difficulty,
                           c.Category_Name
                    FROM Challenge ch
                    LEFT JOIN Category c ON ch.Category_ID = c.Category_ID
                    WHERE (@CategoryID = '' OR ch.Category_ID = @CategoryID)
                    ORDER BY ch.Challenge_ID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CategoryID", categoryId ?? string.Empty);

                    DataTable dt = new DataTable();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    conn.Open();
                    sda.Fill(dt);

                    ChallengeRepeater.DataSource = dt;
                    ChallengeRepeater.DataBind();
                }
            }

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
            var btn = (LinkButton)sender;
            Response.Redirect($"InnerFunction/EditChallenge.aspx?id={btn.CommandArgument}");
        }

        protected string GetDifficultyClass(object difficultyObj)
        {
            string difficulty = difficultyObj == null ? "" : difficultyObj.ToString().ToLower();
            switch (difficulty)
            {
                case "easy": return "diff-easy";
                case "medium": return "diff-medium";
                case "hard": return "diff-hard";
                default: return "diff-easy";
            }
        }
    }
}
