using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace WAPP_KiddieCTF.Student
{
    public partial class ChallengeDetails : Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string challengeId = Request.QueryString["challengeId"];
                if (!string.IsNullOrEmpty(challengeId))
                {
                    LoadChallengeDetails(challengeId);
                    CheckIfChallengeSolved();
                }
                else
                {
                    errorMessage.Visible = true;
                    errorMessage.Text = "No challenge selected!";
                }
            }
        }

        private void LoadChallengeDetails(string challengeId)
        {
            try
            {
                string query = @"
            SELECT 
                C.Challenge_Name, 
                C.Challenge_Description,
                C.Challenge_Difficulty,
                CAT.Category_Name,
                C.Challenge_File,
                L.Lecturer_Name
            FROM Challenge C
            INNER JOIN Category CAT ON C.Category_ID = CAT.Category_ID
            INNER JOIN Lecturer L ON C.Lecturer_ID = L.Lecturer_ID
            WHERE C.Challenge_ID = @Challenge_ID";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Challenge_ID", challengeId);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        lblChallengeName.Text = reader["Challenge_Name"].ToString();
                        lblCategoryName.Text = reader["Category_Name"].ToString();
                        litDescription.Text = reader["Challenge_Description"].ToString();

                        // Difficulty
                        string difficulty = reader["Challenge_Difficulty"].ToString();
                        difficultyLabel.Text = difficulty;
                        difficultyLabel.CssClass = difficulty.ToLower();

                        // Author
                        lblname.Text = reader["Lecturer_Name"].ToString();

                        // File
                        string challengeFile = reader["Challenge_File"]?.ToString();
                        if (!string.IsNullOrEmpty(challengeFile))
                        {
                            string filePath = "/Uploads/Challenges/" + challengeFile;
                            lnkFile.NavigateUrl = filePath;
                            challengeFileSection.Visible = true;
                        }

                        challengeHeader.Visible = true;
                        challengeDescription.Visible = true;
                        submitSection.Visible = true;
                    }
                    else
                    {
                        errorMessage.Visible = true;
                        errorMessage.Text = "Challenge not found!";
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                errorMessage.Visible = true;
                errorMessage.Text = "Error loading challenge: " + ex.Message;
            }
        }

        private void CheckIfChallengeSolved()
        {
            string studentId = Session["StudentID"]?.ToString();
            string challengeId = Request.QueryString["challengeId"];

            if (string.IsNullOrEmpty(studentId) || string.IsNullOrEmpty(challengeId))
            {
                return;
            }

            if (HasAlreadySolvedChallenge(studentId, challengeId))
            {
                submitButton.Enabled = false;
                submitButton.CssClass = "submit-btn disabled";
                flagTextBox.CssClass = "flag-input correct";
                flagTextBox.Text = string.Empty;
                flagTextBox.Attributes["placeholder"] = "This question has been solved!";
                flagTextBox.Attributes["readonly"] = "true";
                lnkFile.Enabled = false;
                lnkFile.CssClass = "download-btn disabled";
            }
        }

        private bool HasAlreadySolvedChallenge(string studentId, string challengeId)
        {
            bool hasSolved = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT COUNT(*) FROM Challenge_Solved WHERE Student_ID = @Student_ID AND Challenge_ID = @Challenge_ID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Student_ID", studentId);
                    cmd.Parameters.AddWithValue("@Challenge_ID", challengeId);

                    conn.Open();
                    int count = (int)cmd.ExecuteScalar();
                    hasSolved = count > 0;
                }
            }
            catch (Exception ex)
            {
                errorMessage.Visible = true;
                errorMessage.Text = "Error checking challenge status: " + ex.Message;
            }

            return hasSolved;
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            string submittedFlag = flagTextBox.Text.Trim();
            bool isFlagValid = ValidateFlag(submittedFlag);

            if (isFlagValid)
            {
                successMessage.Visible = true;
                successMessage.Text = "Congratulation, your flag is correct!";
                submitButton.Enabled = false;
                submitButton.CssClass = "submit-btn disabled";
                flagTextBox.CssClass = "flag-input correct";
                flagTextBox.Text = string.Empty;
                flagTextBox.Attributes["placeholder"] = "This question has been solved!";
                flagTextBox.Attributes["readonly"] = "true";
                lnkFile.Enabled = false;
                lnkFile.CssClass = "download-btn disabled";
                errorMessage.Visible = false;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "HideSuccessMessage", "setTimeout(function() { document.getElementById('" + successMessage.ClientID + "').style.display = 'none'; }, 2000);", true);

                SaveToDatabase();
            }
            else
            {
                errorMessage.Visible = true;
                errorMessage.Text = "Incorrect flag! Please try again.";
                successMessage.Visible = false;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "HideErrorMessage", "setTimeout(function() { document.getElementById('" + errorMessage.ClientID + "').style.display = 'none'; }, 2000);", true);
            }
        }

        private bool ValidateFlag(string flag)
        {
            string challengeId = Request.QueryString["challengeId"];
            if (string.IsNullOrEmpty(challengeId)) return false;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT Challenge_Answer FROM Challenge WHERE Challenge_ID = @Challenge_ID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Challenge_ID", challengeId);

                    conn.Open();
                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        string correctFlag = result.ToString().Trim();
                        return flag.Equals(correctFlag, StringComparison.OrdinalIgnoreCase);
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage.Visible = true;
                errorMessage.Text = "Error validating flag: " + ex.Message;
                return false;
            }
        }

        private void SaveToDatabase()
        {
            string studentId = Session["StudentID"]?.ToString();
            string challengeId = Request.QueryString["challengeId"];
            string currentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            try
            {
                string solveId = GetNextSolveId();
                string aChaRId = GetNextAChaRID();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string checkQuery = "SELECT COUNT(*) FROM Challenge_Solved WHERE Student_ID = @Student_ID AND Challenge_ID = @Challenge_ID";
                    SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                    checkCmd.Parameters.AddWithValue("@Student_ID", studentId);
                    checkCmd.Parameters.AddWithValue("@Challenge_ID", challengeId);

                    conn.Open();
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count == 0)
                    {
                        string insertSolvedQuery = @"
                            INSERT INTO Challenge_Solved (Solve_ID, Student_ID, Challenge_ID)
                            VALUES (@Solve_ID, @Student_ID, @Challenge_ID)";
                        SqlCommand insertSolvedCmd = new SqlCommand(insertSolvedQuery, conn);
                        insertSolvedCmd.Parameters.AddWithValue("@Solve_ID", solveId);
                        insertSolvedCmd.Parameters.AddWithValue("@Student_ID", studentId);
                        insertSolvedCmd.Parameters.AddWithValue("@Challenge_ID", challengeId);
                        insertSolvedCmd.ExecuteNonQuery();

                        string insertAccessQuery = @"
                            INSERT INTO Access_Challenge_Record (AChaR_ID, Student_ID, AChaR_Time, AChaR_Date, Challenge_ID)
                            VALUES (@AChaR_ID, @Student_ID, @AChaR_Time, @AChaR_Date, @Challenge_ID)";
                        SqlCommand insertAccessCmd = new SqlCommand(insertAccessQuery, conn);
                        insertAccessCmd.Parameters.AddWithValue("@AChaR_ID", aChaRId);
                        insertAccessCmd.Parameters.AddWithValue("@Student_ID", studentId);
                        insertAccessCmd.Parameters.AddWithValue("@AChaR_Time", currentTime);
                        insertAccessCmd.Parameters.AddWithValue("@AChaR_Date", DateTime.Now.ToString("yyyy-MM-dd"));
                        insertAccessCmd.Parameters.AddWithValue("@Challenge_ID", challengeId);
                        insertAccessCmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage.Visible = true;
                errorMessage.Text = "Error saving data: " + ex.Message;
            }
        }

        private string GetNextSolveId()
        {
            string nextId = "SV001";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT MAX(CAST(SUBSTRING(Solve_ID, 3, 3) AS INT)) FROM Challenge_Solved";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    conn.Open();
                    object result = cmd.ExecuteScalar();

                    if (result != DBNull.Value)
                    {
                        int maxId = Convert.ToInt32(result);
                        nextId = $"SV{(maxId + 1):D3}";
                    }
                }
            }
            catch
            {

            }
            return nextId;
        }

        private string GetNextAChaRID()
        {
            string nextId = "ACH001";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT MAX(CAST(SUBSTRING(ACHaR_ID, 4, LEN(ACHaR_ID) - 3) AS INT)) FROM Access_Challenge_Record";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    conn.Open();
                    object result = cmd.ExecuteScalar();

                    int maxId = result != DBNull.Value ? Convert.ToInt32(result) : 0;

                    nextId = $"ACH{(maxId + 1):D3}";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return nextId;
        }
    }
}