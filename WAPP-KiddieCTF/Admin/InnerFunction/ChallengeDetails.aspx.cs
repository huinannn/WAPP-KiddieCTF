using System;
using System.Configuration;
using System.Data.SqlClient;

namespace WAPP_KiddieCTF.Admin.InnerFunction
{
    public partial class ChallengeDetails : System.Web.UI.Page
    {
        private readonly string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        private string challengeID = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            // ADMIN VERSION: no lecturer session check

            challengeID = Request.QueryString["id"];
            if (string.IsNullOrEmpty(challengeID))
            {
                Response.Redirect("../Challenges.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadChallengeDetails();
            }
        }

        private void LoadChallengeDetails()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"
                    SELECT c.Challenge_Name, c.Challenge_Difficulty, c.Challenge_Description, 
                           c.Challenge_File, cat.Category_Name
                    FROM Challenge c
                    INNER JOIN Category cat ON c.Category_ID = cat.Category_ID
                    WHERE c.Challenge_ID = @ChallengeID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ChallengeID", challengeID);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    lblChallengeName.Text = reader["Challenge_Name"].ToString();
                    lblCategory.Text = reader["Category_Name"].ToString();
                    lblDescription.Text = reader["Challenge_Description"].ToString();

                    string difficulty = reader["Challenge_Difficulty"].ToString();
                    difficultyBox.InnerText = difficulty.ToUpper();
                    difficultyBox.Attributes["class"] = "difficulty-box " + GetDifficultyCss(difficulty);

                    string fileName = reader["Challenge_File"].ToString();
                    lblFileName.Text = fileName;
                }
                else
                {
                    lblChallengeName.Text = "Unknown Challenge";
                }
                reader.Close();
            }
        }

        private string GetDifficultyCss(string difficulty)
        {
            difficulty = (difficulty ?? "").ToLower();
            switch (difficulty)
            {
                case "easy": return "difficulty-easy";
                case "medium": return "difficulty-medium";
                case "hard": return "difficulty-hard";
                default: return "difficulty-easy";
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            // go to admin edit challenge in innerfunction
            Response.Redirect($"EditChallenge.aspx?id={challengeID}");
        }
    }
}
