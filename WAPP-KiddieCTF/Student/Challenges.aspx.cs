using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace WAPP_KiddieCTF.Student
{
    public partial class Challenges : Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        private string ActiveCategory
        {
            get { return ViewState["ActiveCategory"] as string ?? "All"; }
            set { ViewState["ActiveCategory"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCategories();
                LoadChallenges("All");
                ActiveCategory = "All";
            }
            else
            {
                LoadCategories();
            }
        }

        private void LoadCategories()
        {
            try
            {
                string query = "SELECT Category_ID, Category_Name FROM Category ORDER BY Category_Name";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    categoryTabs.Controls.Clear();

                    var allTab = new LinkButton
                    {
                        ID = "tabAll",
                        Text = "All",
                        CommandArgument = "All",
                        CssClass = ActiveCategory == "All" ? "tab active" : "tab"
                    };
                    allTab.Click += Tab_Click;
                    categoryTabs.Controls.Add(allTab);

                    while (reader.Read())
                    {
                        string id = reader["Category_ID"].ToString();
                        string name = reader["Category_Name"].ToString();

                        var tab = new LinkButton
                        {
                            ID = $"tab{id}",
                            Text = name,
                            CommandArgument = id,
                            CssClass = ActiveCategory == id ? "tab active" : "tab"
                        };
                        tab.Click += Tab_Click;
                        categoryTabs.Controls.Add(tab);
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                categoryTabs.Controls.Clear();
                categoryTabs.Controls.Add(new LiteralControl(
                    $"<div class='error'>Error loading categories: {ex.Message}</div>"));
            }
        }

        protected void Tab_Click(object sender, EventArgs e)
        {
            if (sender is LinkButton clickedTab)
            {
                string categoryId = clickedTab.CommandArgument;
                ActiveCategory = categoryId;

                LoadCategories();
                LoadChallenges(categoryId);
            }
        }

        private void LoadChallenges(string categoryId)
        {
            try
            {
                string query = categoryId == "All"
                    ? @"SELECT C.Challenge_ID, C.Challenge_Name, C.Category_ID, C.Challenge_Difficulty, 
                   CAT.Category_Name,
                   CASE WHEN EXISTS (SELECT 1 FROM Challenge_Solved WHERE Student_ID = @Student_ID AND Challenge_ID = C.Challenge_ID) THEN 1 ELSE 0 END AS IsSolved
            FROM Challenge C
            INNER JOIN Category CAT ON C.Category_ID = CAT.Category_ID
            ORDER BY 
                CASE 
                    WHEN C.Challenge_Difficulty = 'Hard' THEN 1
                    WHEN C.Challenge_Difficulty = 'Medium' THEN 2
                    WHEN C.Challenge_Difficulty = 'Easy' THEN 3
                    ELSE 4
                END,
                C.Challenge_Name"
                    : @"SELECT C.Challenge_ID, C.Challenge_Name, C.Category_ID, C.Challenge_Difficulty, 
                   CAT.Category_Name,
                   CASE WHEN EXISTS (SELECT 1 FROM Challenge_Solved WHERE Student_ID = @Student_ID AND Challenge_ID = C.Challenge_ID) THEN 1 ELSE 0 END AS IsSolved
            FROM Challenge C
            INNER JOIN Category CAT ON C.Category_ID = CAT.Category_ID
            WHERE C.Category_ID = @Category_ID
            ORDER BY 
                CASE 
                    WHEN C.Challenge_Difficulty = 'Hard' THEN 1
                    WHEN C.Challenge_Difficulty = 'Medium' THEN 2
                    WHEN C.Challenge_Difficulty = 'Easy' THEN 3
                    ELSE 4
                END,
                C.Challenge_Name";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Student_ID", Session["StudentID"].ToString());
                    if (categoryId != "All")
                    {
                        cmd.Parameters.AddWithValue("@Category_ID", categoryId);
                    }

                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    challengeGrid.Controls.Clear();

                    if (dt.Rows.Count == 0)
                    {
                        var noDataMessage = new HtmlGenericControl("div")
                        {
                            InnerText = $"No challenges found for {(categoryId == "All" ? "any category" : "this category")}!",
                            Attributes = { ["class"] = "no-challenges" }
                        };
                        challengeGrid.Parent.Controls.Add(noDataMessage);
                    }
                    else
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            string challengeId = row["Challenge_ID"].ToString();
                            string challengeName = row["Challenge_Name"].ToString();
                            string difficulty = row["Challenge_Difficulty"].ToString();
                            string categoryName = row["Category_Name"].ToString();
                            bool isSolved = Convert.ToBoolean(row["IsSolved"]);

                            var item = new HtmlGenericControl("div")
                            {
                                Attributes = {
                            ["class"] = "challenge-item" + (isSolved ? " solved-challenge" : ""),
                            ["onclick"] = $"redirectToChallenge('{challengeId}')"
                        }
                            };

                            item.Controls.Add(new HtmlGenericControl("div")
                            {
                                InnerText = categoryName,
                                Attributes = { ["class"] = "category" }
                            });

                            item.Controls.Add(new HtmlGenericControl("div")
                            {
                                InnerText = challengeName,
                                Attributes = { ["class"] = "c_name" }
                            });

                            item.Controls.Add(new HtmlGenericControl("div")
                            {
                                InnerText = difficulty,
                                Attributes = { ["class"] = $"difficulty {difficulty.ToLower()}" }
                            });

                            challengeGrid.Controls.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                challengeGrid.Controls.Clear();
                challengeGrid.Controls.Add(new LiteralControl(
                    $"<div class='error'>Error loading challenges: {ex.Message}</div>"));
            }
        }
    }
}