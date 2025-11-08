using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WAPP_KiddieCTF.Lecturer
{
    public partial class ChallengeDetails : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        string challengeID = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LecturerID"] == null || Session["LecturerName"] == null)
            {
                Response.Redirect("../LogIn.aspx");
                return;
            }

            lblLecturerID.Text = Session["LecturerID"].ToString();
            lblLecturerName.Text = Session["LecturerName"].ToString();

            challengeID = Request.QueryString["id"];
            if (string.IsNullOrEmpty(challengeID))
            {
                Response.Redirect("Challenges.aspx");
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
                    WHERE c.Challenge_ID = @ChallengeID AND c.Lecturer_ID = @LecturerID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ChallengeID", challengeID);
                cmd.Parameters.AddWithValue("@LecturerID", Session["LecturerID"].ToString());

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
            difficulty = difficulty.ToLower();
            switch (difficulty)
            {
                case "easy": return "difficulty-box difficulty-easy";
                case "medium": return "difficulty-box difficulty-medium";
                case "hard": return "difficulty-box difficulty-hard";
                default: return "difficulty-box difficulty-easy";
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            Response.Redirect($"EditChallenge.aspx?id={challengeID}");
        }


    }
}