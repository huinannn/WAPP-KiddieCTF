using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.UI;

namespace WAPP_KiddieCTF.Admin
{
    public partial class Dashboard : System.Web.UI.Page
    {
        private readonly string connectionString =
            ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCounts();
                LoadLatestIntakes();
                LoadMostAccessed();
                LoadYears(); // for charts

                // after binding years, load chart data for selected year
                if (ddlYear.Items.Count > 0)
                {
                    int year = int.Parse(ddlYear.SelectedValue);
                    LoadChartData(year);
                }
            }
        }

        // -----------------------------
        // counts
        // -----------------------------
        private void LoadCounts()
        {
            int studentCount = GetTableCount("Student");
            int lecturerCount = GetTableCount("Lecturer");

            lblStudentsCount.Text = studentCount.ToString();
            lblLecturerCount.Text = lecturerCount.ToString();
        }

        private int GetTableCount(string tableName)
        {
            string query = $"SELECT COUNT(*) FROM [{tableName}]";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                        return Convert.ToInt32(result);
                }
            }
            catch
            {
                return 0;
            }

            return 0;
        }

        // -----------------------------
        // latest intake
        // -----------------------------
        private void LoadLatestIntakes()
        {
            DataTable dt = GetLatestIntakes();
            rptLatestIntake.DataSource = dt;
            rptLatestIntake.DataBind();
        }

        private DataTable GetLatestIntakes()
        {
            string query = @"
                SELECT TOP 5 
                    [Intake_Code],
                    [Intake_Name],
                    [Intake_Month],
                    [Intake_Year]
                FROM [Intake]
                ORDER BY [Intake_Year] DESC, [Intake_Month] DESC";

            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    dt.Load(rdr);
                }
            }

            return dt;
        }

        // -----------------------------
        // most accessed
        // -----------------------------
        private void LoadMostAccessed()
        {
            DataTable dt = GetMostAccessed();
            rptMostAccessed.DataSource = dt;
            rptMostAccessed.DataBind();
        }

        private DataTable GetMostAccessed()
        {
            string query = @"
        SELECT TOP 6 *
        FROM
        (
            -- Courses
            SELECT 
                c.[Course_Name] AS [Name],
                'Course' AS [Category],
                COUNT(*) AS [TotalAccessed]      -- ⬅️ changed here
            FROM [Access_Course_Record] acr
            INNER JOIN [Course] c ON acr.[Course_ID] = c.[Course_ID]
            GROUP BY c.[Course_Name]

            UNION ALL

            -- Challenges
            SELECT 
                ch.[Challenge_Name] AS [Name],
                'Challenge' AS [Category],
                COUNT(*) AS [TotalAccessed]      -- ⬅️ and here
            FROM [Access_Challenge_Record] achr
            INNER JOIN [Challenge] ch ON achr.[Challenge_ID] = ch.[Challenge_ID]
            GROUP BY ch.[Challenge_Name]
        ) AS Combined
        ORDER BY Combined.[TotalAccessed] DESC;";

            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    dt.Load(rdr);
                }
            }

            return dt;
        }


        // -----------------------------
        // year dropdown for charts
        // -----------------------------
        private void LoadYears()
        {
            string query = @"
                SELECT DISTINCT YEAR(LecLogin_Date) AS Y FROM Lecturer_Login WHERE LecLogin_Date IS NOT NULL
                UNION
                SELECT DISTINCT YEAR(StdLogin_Date) AS Y FROM Student_Login WHERE StdLogin_Date IS NOT NULL
                ORDER BY Y DESC";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    ddlYear.Items.Clear();
                    while (rdr.Read())
                    {
                        int y = rdr.GetInt32(0);
                        ddlYear.Items.Add(y.ToString());
                    }
                }
            }
        }

        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            int year = int.Parse(ddlYear.SelectedValue);
            LoadChartData(year);
        }

        // -----------------------------
        // chart data
        // -----------------------------
        private void LoadChartData(int year)
        {
            // 12 months, default 0
            int[] lecLogin = new int[12];
            int[] stdLogin = new int[12];
            int[] lecLogout = new int[12];
            int[] stdLogout = new int[12];

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // lecturer login
                using (SqlCommand cmd = new SqlCommand(@"
                    SELECT MONTH(LecLogin_Date) AS M, COUNT(*) AS C
                    FROM Lecturer_Login
                    WHERE LecLogin_Date IS NOT NULL AND YEAR(LecLogin_Date) = @y
                    GROUP BY MONTH(LecLogin_Date)", conn))
                {
                    cmd.Parameters.AddWithValue("@y", year);
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            int m = rdr.GetInt32(0);
                            int c = rdr.GetInt32(1);
                            lecLogin[m - 1] = c;
                        }
                    }
                }

                // student login
                using (SqlCommand cmd = new SqlCommand(@"
                    SELECT MONTH(StdLogin_Date) AS M, COUNT(*) AS C
                    FROM Student_Login
                    WHERE StdLogin_Date IS NOT NULL AND YEAR(StdLogin_Date) = @y
                    GROUP BY MONTH(StdLogin_Date)", conn))
                {
                    cmd.Parameters.AddWithValue("@y", year);
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            int m = rdr.GetInt32(0);
                            int c = rdr.GetInt32(1);
                            stdLogin[m - 1] = c;
                        }
                    }
                }

                // lecturer logout
                using (SqlCommand cmd = new SqlCommand(@"
                    SELECT MONTH(LecLogout_Date) AS M, COUNT(*) AS C
                    FROM Lecturer_Login
                    WHERE LecLogout_Date IS NOT NULL AND YEAR(LecLogout_Date) = @y
                    GROUP BY MONTH(LecLogout_Date)", conn))
                {
                    cmd.Parameters.AddWithValue("@y", year);
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            int m = rdr.GetInt32(0);
                            int c = rdr.GetInt32(1);
                            lecLogout[m - 1] = c;
                        }
                    }
                }

                // student logout
                using (SqlCommand cmd = new SqlCommand(@"
                    SELECT MONTH(StdLogout_Date) AS M, COUNT(*) AS C
                    FROM Student_Login
                    WHERE StdLogout_Date IS NOT NULL AND YEAR(StdLogout_Date) = @y
                    GROUP BY MONTH(StdLogout_Date)", conn))
                {
                    cmd.Parameters.AddWithValue("@y", year);
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            int m = rdr.GetInt32(0);
                            int c = rdr.GetInt32(1);
                            stdLogout[m - 1] = c;
                        }
                    }
                }
            }

            // now emit JS
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<script>");
            sb.AppendLine("const months = ['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec'];");

            sb.AppendLine($"const lecLogin = [{string.Join(",", lecLogin)}];");
            sb.AppendLine($"const stdLogin = [{string.Join(",", stdLogin)}];");
            sb.AppendLine($"const lecLogout = [{string.Join(",", lecLogout)}];");
            sb.AppendLine($"const stdLogout = [{string.Join(",", stdLogout)}];");

            // login chart (months on X)
            sb.AppendLine(@"
                const loginCtx = document.getElementById('loginChart').getContext('2d');
                if (window.loginChartObj) { window.loginChartObj.destroy(); }
                window.loginChartObj = new Chart(loginCtx, {
                    type: 'bar',
                    data: {
                        labels: months,
                        datasets: [
                            {
                                label: 'Lecturer Login',
                                data: lecLogin,
                                backgroundColor: '#6EC5FF',
                                borderColor: '#6EC5FF',
                                borderWidth: 1
                            },
                            {
                                label: 'Student Login',
                                data: stdLogin,
                                backgroundColor: '#FFFFFF',
                                borderColor: '#FFFFFF',
                                borderWidth: 1
                            }
                        ]
                    },
                    options: {
                        responsive: true,
                        maintainAspectRatio: false,
                        scales: {
                            x: {
                                beginAtZero: true,
                                ticks: {
                                    color: '#fff',
                                    font: {
                                        family: 'Teko',
                                        size: 12
                                    }
                                }
                            },
                            y: {
                                beginAtZero: true,
                                ticks: {
                                    color: '#fff',
                                    stepSize: 1,
                                    precision: 0,
                                    font: {
                                        family: 'Teko',
                                        size: 12
                                    }
                                }
                            }
                        },
                        plugins: {
                            legend: {
                                labels: {
                                    color: '#fff',
                                    font: {
                                        family: 'Teko',
                                        size: 13
                                    }
                                }
                            }
                        }
                    }
                });
            ");

            // logout chart (months on X)
            sb.AppendLine(@"
                const logoutCtx = document.getElementById('logoutChart').getContext('2d');
                if (window.logoutChartObj) { window.logoutChartObj.destroy(); }
                window.logoutChartObj = new Chart(logoutCtx, {
                    type: 'bar',
                    data: {
                        labels: months,
                        datasets: [
                            {
                                label: 'Lecturer Logout',
                                data: lecLogout,
                                backgroundColor: '#6EC5FF',
                                borderColor: '#6EC5FF',
                                borderWidth: 1
                            },
                            {
                                label: 'Student Logout',
                                data: stdLogout,
                                backgroundColor: '#FFFFFF',
                                borderColor: '#FFFFFF',
                                borderWidth: 1
                            }
                        ]
                    },
                    options: {
                        responsive: true,
                        maintainAspectRatio: false,
                        scales: {
                            x: {
                                beginAtZero: true,
                                ticks: {
                                    color: '#fff',
                                    font: {
                                        family: 'Teko',
                                        size: 12
                                    }
                                }
                            },
                            y: {
                                beginAtZero: true,
                                ticks: {
                                    color: '#fff',
                                    stepSize: 1,
                                    precision: 0,
                                    font: {
                                        family: 'Teko',
                                        size: 12
                                    }
                                }
                            }
                        },
                        plugins: {
                            legend: {
                                labels: {
                                    color: '#fff',
                                    font: {
                                        family: 'Teko',
                                        size: 13
                                    }
                                }
                            }
                        }
                    }
                });
            ");

            sb.AppendLine("</script>");

            litChartData.Text = sb.ToString();
        }
    }
}
