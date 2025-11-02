using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace WAPP_KiddieCTF.Admin
{
    public partial class Account : System.Web.UI.Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadStudentData();
            }
        }

        private void LoadStudentData()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT Student_ID, Student_Name, Intake_Code FROM Student WHERE Student_ID IS NOT NULL", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvAccounts.DataSource = dt;
                gvAccounts.DataBind();
            }

            SetActiveTab("student");
        }

        private void LoadLecturerData()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT Lecturer_ID, Lecturer_Name FROM Lecturer WHERE Lecturer_ID IS NOT NULL", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvAccounts.DataSource = dt;
                gvAccounts.DataBind();
            }

            SetActiveTab("lecturer");
        }

        private void LoadIntakeData()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT Intake_Code, Intake_Name, Intake_Month, Intake_Year FROM Intake WHERE Intake_Code IS NOT NULL", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvAccounts.DataSource = dt;
                gvAccounts.DataBind();
            }

            SetActiveTab("intake");
        }

        // ✅ Handle tab clicks
        protected void btnStudentTab_Click(object sender, EventArgs e)
        {
            LoadStudentData();
        }

        protected void btnLecturerTab_Click(object sender, EventArgs e)
        {
            LoadLecturerData();
        }

        protected void btnIntakeTab_Click(object sender, EventArgs e)
        {
            LoadIntakeData();
        }

        // ✅ Apply tab style dynamically
        private void SetActiveTab(string tab)
        {
            btnStudentTab.CssClass = "tab";
            btnLecturerTab.CssClass = "tab";
            btnIntakeTab.CssClass = "tab";

            switch (tab)
            {
                case "student":
                    btnStudentTab.CssClass += " active";
                    break;
                case "lecturer":
                    btnLecturerTab.CssClass += " active";
                    break;
                case "intake":
                    btnIntakeTab.CssClass += " active";
                    break;
            }
        }
    }
}
