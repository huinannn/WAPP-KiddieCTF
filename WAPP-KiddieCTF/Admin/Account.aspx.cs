using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

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
                btnAddStudent.Text = "Add New Student"; // default when page loads
            }
        }

        // Method to load student data
        private void LoadStudentData(string searchQuery = "", string filterQuery = "")
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT Student_ID, Student_Name, Intake_Code FROM Student WHERE Student_ID IS NOT NULL";

                // Apply search filter if any
                if (!string.IsNullOrEmpty(searchQuery))
                    query += " AND (Student_ID LIKE @SearchQuery OR Student_Name LIKE @SearchQuery OR Intake_Code LIKE @SearchQuery)";

                // Apply filter
                if (!string.IsNullOrEmpty(filterQuery))
                    query += filterQuery;

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                da.SelectCommand.Parameters.AddWithValue("@SearchQuery", "%" + searchQuery + "%");
                da.Fill(dt);
            }

            gvAccounts.Columns.Clear();
            gvAccounts.Columns.Add(new BoundField { DataField = "Student_ID", HeaderText = "Student ID" });
            gvAccounts.Columns.Add(new BoundField { DataField = "Student_Name", HeaderText = "Name" });
            gvAccounts.Columns.Add(new BoundField { DataField = "Intake_Code", HeaderText = "Intake" });

            gvAccounts.Columns.Add(CreateActionColumn());

            gvAccounts.DataSource = dt;
            gvAccounts.DataBind();

            SetActiveTab("student");
        }

        // Method to load lecturer data
        private void LoadLecturerData(string searchQuery = "", string filterQuery = "")
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                // Update this query to filter only by valid columns in the Lecturer table
                string query = "SELECT Lecturer_ID, Lecturer_Name FROM Lecturer WHERE Lecturer_ID IS NOT NULL";

                // Apply search filter if any
                if (!string.IsNullOrEmpty(searchQuery))
                    query += " AND (Lecturer_ID LIKE @SearchQuery OR Lecturer_Name LIKE @SearchQuery)";

                // Apply filter
                if (!string.IsNullOrEmpty(filterQuery))
                    query += filterQuery;

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                da.SelectCommand.Parameters.AddWithValue("@SearchQuery", "%" + searchQuery + "%");
                da.Fill(dt);
            }

            gvAccounts.Columns.Clear();
            gvAccounts.Columns.Add(new BoundField { DataField = "Lecturer_ID", HeaderText = "Lecturer ID" });
            gvAccounts.Columns.Add(new BoundField { DataField = "Lecturer_Name", HeaderText = "Name" });

            gvAccounts.Columns.Add(CreateActionColumn());

            gvAccounts.DataSource = dt;
            gvAccounts.DataBind();

            SetActiveTab("lecturer");
        }


        // Method to load intake data
        private void LoadIntakeData(string searchQuery = "", string filterQuery = "")
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT Intake_Code, Intake_Name, Intake_Month, Intake_Year FROM Intake WHERE Intake_Code IS NOT NULL";

                // Apply search filter if any
                if (!string.IsNullOrEmpty(searchQuery))
                    query += " AND (Intake_Code LIKE @SearchQuery OR Intake_Name LIKE @SearchQuery)";

                // Apply filter
                if (!string.IsNullOrEmpty(filterQuery))
                    query += filterQuery;

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                da.SelectCommand.Parameters.AddWithValue("@SearchQuery", "%" + searchQuery + "%");
                da.Fill(dt);
            }

            gvAccounts.Columns.Clear();
            gvAccounts.Columns.Add(new BoundField { DataField = "Intake_Code", HeaderText = "Code" });
            gvAccounts.Columns.Add(new BoundField { DataField = "Intake_Name", HeaderText = "Name" });
            gvAccounts.Columns.Add(new BoundField { DataField = "Intake_Month", HeaderText = "Month" });
            gvAccounts.Columns.Add(new BoundField { DataField = "Intake_Year", HeaderText = "Year" });

            gvAccounts.Columns.Add(CreateActionColumn());

            gvAccounts.DataSource = dt;
            gvAccounts.DataBind();

            SetActiveTab("intake");
        }

        // Handling search button click
        protected void btnFilter_Click(object sender, EventArgs e)
        {
            string searchQuery = txtSearch.Text.Trim();
            string filterQuery = "";

            if (btnStudentTab.CssClass.Contains("active"))
            {
                // Filter for student (Student ID, Name, or Intake Code in ascending order)
                filterQuery = " ORDER BY Student_ID ASC, Student_Name ASC, Intake_Code ASC";
                LoadStudentData(searchQuery, filterQuery);
            }
            else if (btnLecturerTab.CssClass.Contains("active"))
            {
                // Filter for lecturer (Lecturer ID or Student ID)
                filterQuery = " ORDER BY Lecturer_ID ASC, Lecturer_Name ASC";
                LoadLecturerData(searchQuery, filterQuery);
            }
            else if (btnIntakeTab.CssClass.Contains("active"))
            {
                // Filter for intake (Intake Code, Name, Month, Year)
                filterQuery = " ORDER BY Intake_Code ASC, Intake_Name ASC, Intake_Month DESC, Intake_Year DESC";
                LoadIntakeData(searchQuery, filterQuery);
            }
        }

        // Handling tab button clicks
        protected void btnStudentTab_Click(object sender, EventArgs e)
        {
            LoadStudentData();
            btnAddStudent.Text = "Add New Student"; // Update button text
        }

        protected void btnLecturerTab_Click(object sender, EventArgs e)
        {
            LoadLecturerData();
            btnAddStudent.Text = "Add New Lecturer"; // Update button text
        }

        protected void btnIntakeTab_Click(object sender, EventArgs e)
        {
            LoadIntakeData();
            btnAddStudent.Text = "Add New Intake"; // Update button text
        }

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

        private TemplateField CreateActionColumn()
        {
            TemplateField actions = new TemplateField { HeaderText = "Actions" };
            actions.ItemTemplate = new ActionTemplate();
            return actions;
        }

        public class ActionTemplate : ITemplate
        {
            public void InstantiateIn(Control container)
            {
                Button btnEdit = new Button
                {
                    CssClass = "edit-btn",
                    Text = "\uf044", // FontAwesome edit
                    Font = { Name = "FontAwesome" },
                    UseSubmitBehavior = false
                };
                Button btnDelete = new Button
                {
                    CssClass = "delete-btn",
                    Text = "\uf1f8", // FontAwesome trash
                    Font = { Name = "FontAwesome" },
                    UseSubmitBehavior = false
                };

                container.Controls.Add(btnEdit);
                container.Controls.Add(new LiteralControl("&nbsp;"));
                container.Controls.Add(btnDelete);
            }
        }
    }
}