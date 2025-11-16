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
                btnAddStudent.Text = "Add New Student";
            }
            else
            {
                RebindActiveTab();

                if (ViewState["ShowModal"] != null && (bool)ViewState["ShowModal"] == true)
                    pnlModal.Visible = true;

                if (ViewState["ShowSuccess"] != null && (bool)ViewState["ShowSuccess"] == true)
                    pnlSuccess.Visible = true;

                if (ViewState["ShowConfirm"] != null && (bool)ViewState["ShowConfirm"] == true)
                    pnlConfirmDelete.Visible = true;
            }
        }

        private void RebindActiveTab()
        {
            string searchQuery = txtSearch.Text.Trim();

            if (btnStudentTab.CssClass.Contains("active"))
                LoadStudentData(searchQuery);
            else if (btnLecturerTab.CssClass.Contains("active"))
                LoadLecturerData(searchQuery);
            else if (btnIntakeTab.CssClass.Contains("active"))
                LoadIntakeData(searchQuery);
        }

        // ========== LOAD STUDENTS ==========
        private void LoadStudentData(string searchQuery = "", string filterQuery = "")
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT Student_ID, Student_Name, Intake_Code FROM Student WHERE Student_ID IS NOT NULL";

                bool hasSearch = !string.IsNullOrEmpty(searchQuery);
                if (hasSearch)
                    query += " AND (Student_ID LIKE @SearchQuery OR Student_Name LIKE @SearchQuery OR Intake_Code LIKE @SearchQuery)";

                if (!string.IsNullOrEmpty(filterQuery))
                    query += filterQuery;

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                if (hasSearch)
                    da.SelectCommand.Parameters.AddWithValue("@SearchQuery", "%" + searchQuery + "%");
                da.Fill(dt);
            }

            gvAccounts.Columns.Clear();
            gvAccounts.Columns.Add(new BoundField { DataField = "Student_ID", HeaderText = "Student ID" });
            gvAccounts.Columns.Add(new BoundField { DataField = "Student_Name", HeaderText = "Name" });
            gvAccounts.Columns.Add(new BoundField { DataField = "Intake_Code", HeaderText = "Intake" });
            gvAccounts.Columns.Add(CreateActionColumn());

            gvAccounts.DataSource = dt;
            gvAccounts.DataKeyNames = new[] { "Student_ID" };
            gvAccounts.DataBind();

            SetActiveTab("student");
        }

        // ========== LOAD LECTURERS ==========
        private void LoadLecturerData(string searchQuery = "", string filterQuery = "")
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT Lecturer_ID, Lecturer_Name FROM Lecturer WHERE Lecturer_ID IS NOT NULL";

                bool hasSearch = !string.IsNullOrEmpty(searchQuery);
                if (hasSearch)
                    query += " AND (Lecturer_ID LIKE @SearchQuery OR Lecturer_Name LIKE @SearchQuery)";

                if (!string.IsNullOrEmpty(filterQuery))
                    query += filterQuery;

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                if (hasSearch)
                    da.SelectCommand.Parameters.AddWithValue("@SearchQuery", "%" + searchQuery + "%");
                da.Fill(dt);
            }

            gvAccounts.Columns.Clear();
            gvAccounts.Columns.Add(new BoundField { DataField = "Lecturer_ID", HeaderText = "Lecturer ID" });
            gvAccounts.Columns.Add(new BoundField { DataField = "Lecturer_Name", HeaderText = "Name" });
            gvAccounts.Columns.Add(CreateActionColumn());

            gvAccounts.DataSource = dt;
            gvAccounts.DataKeyNames = new[] { "Lecturer_ID" };
            gvAccounts.DataBind();

            SetActiveTab("lecturer");
        }

        // ========== LOAD INTAKES ==========
        private void LoadIntakeData(string searchQuery = "", string filterQuery = "")
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT Intake_Code, Intake_Name, Intake_Month, Intake_Year FROM Intake WHERE Intake_Code IS NOT NULL";

                bool hasSearch = !string.IsNullOrEmpty(searchQuery);
                if (hasSearch)
                {
                    query += @" AND (
                                    Intake_Code  LIKE @SearchQuery
                                    OR Intake_Name  LIKE @SearchQuery
                                    OR Intake_Month LIKE @SearchQuery
                                    OR Intake_Year  LIKE @SearchQuery
                                )";
                }

                if (!string.IsNullOrEmpty(filterQuery))
                    query += filterQuery;

                SqlDataAdapter da = new SqlDataAdapter(query, con);
                if (hasSearch)
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
            gvAccounts.DataKeyNames = new[] { "Intake_Code" };
            gvAccounts.DataBind();

            SetActiveTab("intake");
        }

        // ========== FILTER ==========
        protected void btnFilter_Click(object sender, EventArgs e)
        {
            string searchQuery = txtSearch.Text.Trim();
            string filterQuery = "";

            if (btnStudentTab.CssClass.Contains("active"))
            {
                filterQuery = " ORDER BY Student_ID ASC, Student_Name ASC, Intake_Code ASC";
                LoadStudentData(searchQuery, filterQuery);
            }
            else if (btnLecturerTab.CssClass.Contains("active"))
            {
                filterQuery = " ORDER BY Lecturer_ID ASC, Lecturer_Name ASC";
                LoadLecturerData(searchQuery, filterQuery);
            }
            else if (btnIntakeTab.CssClass.Contains("active"))
            {
                filterQuery = " ORDER BY Intake_Code ASC, Intake_Name ASC, Intake_Month DESC, Intake_Year DESC";
                LoadIntakeData(searchQuery, filterQuery);
            }
        }

        // ========== TABS ==========
        protected void btnStudentTab_Click(object sender, EventArgs e)
        {
            LoadStudentData();
            btnAddStudent.Text = "Add New Student";
        }

        protected void btnLecturerTab_Click(object sender, EventArgs e)
        {
            LoadLecturerData();
            btnAddStudent.Text = "Add New Lecturer";
        }

        protected void btnIntakeTab_Click(object sender, EventArgs e)
        {
            LoadIntakeData();
            btnAddStudent.Text = "Add New Intake";
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

        // ========== OPEN MODAL (ADD) ==========
        protected void btnAddStudent_Click(object sender, EventArgs e)
        {
            ClearAllModalErrors();

            pnlAddStudent.Visible = false;
            pnlAddLecturer.Visible = false;
            pnlAddIntake.Visible = false;

            ViewState["EditMode"] = null;
            ViewState["EditKey"] = null;

            if (btnStudentTab.CssClass.Contains("active"))
            {
                pnlAddStudent.Visible = true;
                BindStudentIntakeDropdown();
                txtNewStudentID.Enabled = true;
                txtNewStudentID.Text = "";
                txtNewStudentName.Text = "";
                lblStudentModalTitle.Text = "Add New Student";
                btnSaveStudent.Text = "Add";
            }
            else if (btnLecturerTab.CssClass.Contains("active"))
            {
                pnlAddLecturer.Visible = true;
                txtNewLecturerID.Enabled = true;
                txtNewLecturerID.Text = "";
                txtNewLecturerName.Text = "";
                lblLecturerModalTitle.Text = "Add New Lecturer";
                btnSaveLecturer.Text = "Add";
            }
            else if (btnIntakeTab.CssClass.Contains("active"))
            {
                pnlAddIntake.Visible = true;
                BindIntakeMonthYearDropdowns();
                txtNewIntakeCode.Enabled = true;
                txtNewIntakeCode.Text = "";
                txtNewIntakeName.Text = "";
                lblIntakeModalTitle.Text = "Add New Intake";
                btnSaveIntake.Text = "Add";
            }

            pnlModal.Visible = true;
            ViewState["ShowModal"] = true;
        }

        // ========== CANCEL MODAL ==========
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            pnlModal.Visible = false;
            pnlSuccess.Visible = false;
            pnlConfirmDelete.Visible = false;
            ViewState["ShowModal"] = false;
            ViewState["ShowSuccess"] = false;
            ViewState["ShowConfirm"] = false;
            ViewState["EditMode"] = null;
            ViewState["EditKey"] = null;
        }

        // ========== SAVE STUDENT ==========
        protected void btnSaveStudent_Click(object sender, EventArgs e)
        {
            string studentID = (txtNewStudentID.Text ?? "").Trim().ToUpper();
            string studentName = (txtNewStudentName.Text ?? "").Trim();
            string intakeCode = (ddlStudentIntake.SelectedValue ?? "").Trim().ToUpper();

            if (string.IsNullOrEmpty(studentID) || string.IsNullOrEmpty(studentName))
            {
                ShowModalError("Please fill in student ID and student name.");
                pnlAddStudent.Visible = true;
                return;
            }

            if (string.IsNullOrEmpty(intakeCode))
            {
                ShowModalError("Please select an intake for this student.");
                pnlAddStudent.Visible = true;
                return;
            }

            bool isEdit = (ViewState["EditMode"] != null && ViewState["EditMode"].ToString() == "Student");

            if (isEdit)
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        string update = "UPDATE Student SET Student_Name=@Student_Name, Intake_Code=@Intake_Code WHERE Student_ID=@Student_ID";
                        using (SqlCommand cmd = new SqlCommand(update, con))
                        {
                            cmd.Parameters.AddWithValue("@Student_ID", studentID);
                            cmd.Parameters.AddWithValue("@Student_Name", studentName);
                            cmd.Parameters.AddWithValue("@Intake_Code", intakeCode);
                            con.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }

                    LoadStudentData();
                    ShowSuccessMessage("Student updated successfully!", "The student record has been updated.");
                }
                catch (SqlException ex)
                {
                    ShowModalError("Error updating student: " + ex.Message);
                    pnlAddStudent.Visible = true;
                    ViewState["ShowModal"] = true;
                }
            }
            else
            {
                string monthYear = DateTime.Now.ToString("MMMMyyyy");
                string safeName = studentName.Replace(" ", "");
                if (string.IsNullOrEmpty(safeName))
                    safeName = "Student";
                string defaultPassword = $"{safeName}@Temp!({monthYear})";

                try
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        string insert = "INSERT INTO Student (Student_ID, Student_Name, Intake_Code, Student_Password) " +
                                        "VALUES (@Student_ID, @Student_Name, @Intake_Code, @Student_Password)";
                        using (SqlCommand cmd = new SqlCommand(insert, con))
                        {
                            cmd.Parameters.AddWithValue("@Student_ID", studentID);
                            cmd.Parameters.AddWithValue("@Student_Name", studentName);
                            cmd.Parameters.AddWithValue("@Intake_Code", intakeCode);
                            cmd.Parameters.AddWithValue("@Student_Password", defaultPassword);

                            con.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }

                    LoadStudentData();
                    ShowSuccessMessage("Student added successfully!", "This is the temporary password:<br/><b>" + defaultPassword + "</b>");
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2627 || ex.Number == 2601)
                        ShowModalError("Student ID already exists. Please use another ID.");
                    else
                        ShowModalError("Error adding student: " + ex.Message);

                    pnlAddStudent.Visible = true;
                    ViewState["ShowModal"] = true;
                }
            }
        }

        // ========== SAVE LECTURER ==========
        protected void btnSaveLecturer_Click(object sender, EventArgs e)
        {
            string lecturerID = (txtNewLecturerID.Text ?? "").Trim().ToUpper();
            string lecturerName = (txtNewLecturerName.Text ?? "").Trim();

            if (string.IsNullOrEmpty(lecturerID) || string.IsNullOrEmpty(lecturerName))
            {
                ShowModalError("Please fill in lecturer ID and lecturer name.");
                pnlAddLecturer.Visible = true;
                return;
            }

            bool isEdit = (ViewState["EditMode"] != null && ViewState["EditMode"].ToString() == "Lecturer");

            if (isEdit)
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        string update = "UPDATE Lecturer SET Lecturer_Name=@Name WHERE Lecturer_ID=@ID";
                        using (SqlCommand cmd = new SqlCommand(update, con))
                        {
                            cmd.Parameters.AddWithValue("@ID", lecturerID);
                            cmd.Parameters.AddWithValue("@Name", lecturerName);
                            con.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }

                    LoadLecturerData();
                    ShowSuccessMessage("Lecturer updated successfully!", "The lecturer record has been updated.");
                }
                catch (SqlException ex)
                {
                    ShowModalError("Error updating lecturer: " + ex.Message);
                    pnlAddLecturer.Visible = true;
                    ViewState["ShowModal"] = true;
                }
            }
            else
            {
                string monthYear = DateTime.Now.ToString("MMMMyyyy");
                string cleanName = lecturerName.Replace(" ", "");
                if (string.IsNullOrEmpty(cleanName))
                    cleanName = "Lecturer";
                string defaultPassword = $"{cleanName}@Temp!({monthYear})";

                try
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        string insert = "INSERT INTO Lecturer (Lecturer_ID, Lecturer_Name, Lecturer_Password) " +
                                        "VALUES (@Lecturer_ID, @Lecturer_Name, @Lecturer_Password)";
                        using (SqlCommand cmd = new SqlCommand(insert, con))
                        {
                            cmd.Parameters.AddWithValue("@Lecturer_ID", lecturerID);
                            cmd.Parameters.AddWithValue("@Lecturer_Name", lecturerName);
                            cmd.Parameters.AddWithValue("@Lecturer_Password", defaultPassword);

                            con.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }

                    LoadLecturerData();
                    ShowSuccessMessage("Lecturer added successfully!", "This is the temporary password:<br/><b>" + defaultPassword + "</b>");
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2627 || ex.Number == 2601)
                        ShowModalError("Lecturer ID already exists. Please use another ID.");
                    else
                        ShowModalError("Error adding lecturer: " + ex.Message);

                    pnlAddLecturer.Visible = true;
                    ViewState["ShowModal"] = true;
                }
            }
        }

        // ========== SAVE INTAKE ==========
        protected void btnSaveIntake_Click(object sender, EventArgs e)
        {
            string intakeCode = (txtNewIntakeCode.Text ?? "").Trim().ToUpper();
            string intakeName = (txtNewIntakeName.Text ?? "").Trim();
            string intakeMonth = ddlIntakeMonth.SelectedValue;
            string intakeYear = ddlIntakeYear.SelectedValue;

            if (string.IsNullOrEmpty(intakeCode) || string.IsNullOrEmpty(intakeName))
            {
                ShowModalError("Please fill in intake code and intake name.");
                pnlAddIntake.Visible = true;
                return;
            }

            bool isEdit = (ViewState["EditMode"] != null && ViewState["EditMode"].ToString() == "Intake");

            if (isEdit)
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        string update = "UPDATE Intake SET Intake_Name=@Name, Intake_Month=@Month, Intake_Year=@Year WHERE Intake_Code=@Code";
                        using (SqlCommand cmd = new SqlCommand(update, con))
                        {
                            cmd.Parameters.AddWithValue("@Code", intakeCode);
                            cmd.Parameters.AddWithValue("@Name", intakeName);
                            cmd.Parameters.AddWithValue("@Month", intakeMonth);
                            cmd.Parameters.AddWithValue("@Year", intakeYear);
                            con.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }

                    LoadIntakeData();
                    ShowSuccessMessage("Intake updated successfully!", "The intake has been successfully updated.");
                }
                catch (SqlException ex)
                {
                    ShowModalError("Error updating intake: " + ex.Message);
                    pnlAddIntake.Visible = true;
                    BindIntakeMonthYearDropdowns();
                    ViewState["ShowModal"] = true;
                }
            }
            else
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        string insert = "INSERT INTO Intake (Intake_Code, Intake_Name, Intake_Month, Intake_Year) " +
                                        "VALUES (@Intake_Code, @Intake_Name, @Intake_Month, @Intake_Year)";
                        using (SqlCommand cmd = new SqlCommand(insert, con))
                        {
                            cmd.Parameters.AddWithValue("@Intake_Code", intakeCode);
                            cmd.Parameters.AddWithValue("@Intake_Name", intakeName);
                            cmd.Parameters.AddWithValue("@Intake_Month", intakeMonth);
                            cmd.Parameters.AddWithValue("@Intake_Year", intakeYear);

                            con.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }

                    LoadIntakeData();
                    ShowSuccessMessage("Intake added successfully!", "The intake has been successfully created.");
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 2627 || ex.Number == 2601)
                        ShowModalError("Intake code already exists. Please use another code.");
                    else
                        ShowModalError("Error adding intake: " + ex.Message);

                    pnlAddIntake.Visible = true;
                    BindIntakeMonthYearDropdowns();
                    ViewState["ShowModal"] = true;
                }
            }
        }

        // ========== GRID COMMAND ==========
        protected void gvAccounts_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditRow")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                string key = gvAccounts.DataKeys[rowIndex].Value.ToString();

                if (btnStudentTab.CssClass.Contains("active"))
                    LoadStudentToModal(key);
                else if (btnLecturerTab.CssClass.Contains("active"))
                    LoadLecturerToModal(key);
                else if (btnIntakeTab.CssClass.Contains("active"))
                    LoadIntakeToModal(key);
            }
            else if (e.CommandName == "ConfirmDelete")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                string key = gvAccounts.DataKeys[rowIndex].Value.ToString();

                ViewState["DeleteKey"] = key;
                if (btnStudentTab.CssClass.Contains("active"))
                    ViewState["DeleteType"] = "Student";
                else if (btnLecturerTab.CssClass.Contains("active"))
                    ViewState["DeleteType"] = "Lecturer";
                else if (btnIntakeTab.CssClass.Contains("active"))
                    ViewState["DeleteType"] = "Intake";

                lblConfirmText.Text = "Are you sure you want to delete this record?";
                pnlConfirmDelete.Visible = true;
                ViewState["ShowConfirm"] = true;
            }
        }

        // ========== CONFIRM BUTTONS ==========
        protected void btnConfirmYes_Click(object sender, EventArgs e)
        {
            string key = ViewState["DeleteKey"] as string;
            string type = ViewState["DeleteType"] as string;
            bool deleted = false;

            if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(type))
            {
                if (type == "Student")
                {
                    deleted = DeleteStudent(key);
                    LoadStudentData();
                }
                else if (type == "Lecturer")
                {
                    deleted = DeleteLecturer(key);
                    LoadLecturerData();
                }
                else if (type == "Intake")
                {
                    deleted = DeleteIntake(key);
                    LoadIntakeData();
                }
            }

            pnlConfirmDelete.Visible = false;
            ViewState["ShowConfirm"] = false;
            ViewState["DeleteKey"] = null;
            ViewState["DeleteType"] = null;

            if (deleted)
                ShowSuccessMessage("Record deleted successfully!", "The selected record has been removed.");
        }

        protected void btnConfirmNo_Click(object sender, EventArgs e)
        {
            pnlConfirmDelete.Visible = false;
            ViewState["ShowConfirm"] = false;
            ViewState["DeleteKey"] = null;
            ViewState["DeleteType"] = null;
        }

        // ========== LOAD FOR EDIT ==========
        private void LoadStudentToModal(string studentId)
        {
            ClearAllModalErrors();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = "SELECT Student_ID, Student_Name, Intake_Code FROM Student WHERE Student_ID=@id";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", studentId);
                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            pnlAddStudent.Visible = true;
                            pnlAddLecturer.Visible = false;
                            pnlAddIntake.Visible = false;

                            txtNewStudentID.Text = dr["Student_ID"].ToString();
                            txtNewStudentID.Enabled = false;
                            txtNewStudentName.Text = dr["Student_Name"].ToString();

                            BindStudentIntakeDropdown();
                            string intake = dr["Intake_Code"].ToString();
                            if (ddlStudentIntake.Items.FindByValue(intake) != null)
                                ddlStudentIntake.SelectedValue = intake;

                            lblStudentModalTitle.Text = "Edit Student";
                            btnSaveStudent.Text = "Edit";

                            pnlModal.Visible = true;
                            ViewState["ShowModal"] = true;
                            ViewState["EditMode"] = "Student";
                            ViewState["EditKey"] = studentId;
                        }
                    }
                }
            }
        }

        private void LoadLecturerToModal(string lecturerId)
        {
            ClearAllModalErrors();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = "SELECT Lecturer_ID, Lecturer_Name FROM Lecturer WHERE Lecturer_ID=@id";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", lecturerId);
                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            pnlAddStudent.Visible = false;
                            pnlAddLecturer.Visible = true;
                            pnlAddIntake.Visible = false;

                            txtNewLecturerID.Text = dr["Lecturer_ID"].ToString();
                            txtNewLecturerID.Enabled = false;
                            txtNewLecturerName.Text = dr["Lecturer_Name"].ToString();

                            lblLecturerModalTitle.Text = "Edit Lecturer";
                            btnSaveLecturer.Text = "Edit";

                            pnlModal.Visible = true;
                            ViewState["ShowModal"] = true;
                            ViewState["EditMode"] = "Lecturer";
                            ViewState["EditKey"] = lecturerId;
                        }
                    }
                }
            }
        }

        private void LoadIntakeToModal(string intakeCode)
        {
            ClearAllModalErrors();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = "SELECT Intake_Code, Intake_Name, Intake_Month, Intake_Year FROM Intake WHERE Intake_Code=@id";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", intakeCode);
                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            pnlAddStudent.Visible = false;
                            pnlAddLecturer.Visible = false;
                            pnlAddIntake.Visible = true;

                            txtNewIntakeCode.Text = dr["Intake_Code"].ToString();
                            txtNewIntakeCode.Enabled = false;
                            txtNewIntakeName.Text = dr["Intake_Name"].ToString();

                            BindIntakeMonthYearDropdowns();
                            string month = dr["Intake_Month"].ToString();
                            string year = dr["Intake_Year"].ToString();

                            if (ddlIntakeMonth.Items.FindByValue(month) != null)
                                ddlIntakeMonth.SelectedValue = month;
                            if (ddlIntakeYear.Items.FindByValue(year) != null)
                                ddlIntakeYear.SelectedValue = year;

                            lblIntakeModalTitle.Text = "Edit Intake";
                            btnSaveIntake.Text = "Edit";

                            pnlModal.Visible = true;
                            ViewState["ShowModal"] = true;
                            ViewState["EditMode"] = "Intake";
                            ViewState["EditKey"] = intakeCode;
                        }
                    }
                }
            }
        }

        // ========== DELETE HELPERS ==========
        private bool DeleteStudent(string studentId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    string checkSql = "SELECT COUNT(*) FROM Assigned_Course WHERE Student_ID = @id";
                    using (SqlCommand checkCmd = new SqlCommand(checkSql, con))
                    {
                        checkCmd.Parameters.AddWithValue("@id", studentId);
                        int used = (int)checkCmd.ExecuteScalar();
                        if (used > 0)
                        {
                            ShowModalError("This student is still assigned in a course (Assigned_Course). Remove that assignment first.");
                            return false;
                        }
                    }

                    string sql = "DELETE FROM Student WHERE Student_ID=@id";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@id", studentId);
                        int affected = cmd.ExecuteNonQuery();
                        if (affected == 0)
                        {
                            ShowModalError("Student not found or already deleted.");
                            return false;
                        }
                    }
                }

                return true;
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547)
                    ShowModalError("This student is still referenced by other records. Remove those first, then delete.");
                else
                    ShowModalError("Cannot delete student: " + ex.Message);

                return false;
            }
        }

        private bool DeleteLecturer(string lecturerId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    string checkSql = "SELECT COUNT(*) FROM Course WHERE Lecturer_ID = @id";
                    using (SqlCommand checkCmd = new SqlCommand(checkSql, con))
                    {
                        checkCmd.Parameters.AddWithValue("@id", lecturerId);
                        int used = (int)checkCmd.ExecuteScalar();
                        if (used > 0)
                        {
                            ShowModalError("This lecturer is still assigned to a course. Remove/change the course first.");
                            return false;
                        }
                    }

                    string sql = "DELETE FROM Lecturer WHERE Lecturer_ID=@id";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@id", lecturerId);
                        int affected = cmd.ExecuteNonQuery();
                        if (affected == 0)
                        {
                            ShowModalError("Lecturer not found or already deleted.");
                            return false;
                        }
                    }
                }

                return true;
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547)
                    ShowModalError("This lecturer is still referenced. Remove related records first.");
                else
                    ShowModalError("Cannot delete lecturer: " + ex.Message);

                return false;
            }
        }

        private bool DeleteIntake(string intakeCode)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    string checkSql = "SELECT COUNT(*) FROM Student WHERE Intake_Code = @code";
                    using (SqlCommand checkCmd = new SqlCommand(checkSql, con))
                    {
                        checkCmd.Parameters.AddWithValue("@code", intakeCode);
                        int used = (int)checkCmd.ExecuteScalar();
                        if (used > 0)
                        {
                            ShowModalError("This intake code is being used by students. Move/delete those students first.");
                            return false;
                        }
                    }

                    string sql = "DELETE FROM Intake WHERE Intake_Code=@code";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@code", intakeCode);
                        int affected = cmd.ExecuteNonQuery();
                        if (affected == 0)
                        {
                            ShowModalError("Intake not found or already deleted.");
                            return false;
                        }
                    }
                }

                return true;
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547)
                    ShowModalError("This intake is still referenced by other records. Remove those first.");
                else
                    ShowModalError("Cannot delete intake: " + ex.Message);

                return false;
            }
        }

        // ========== BINDERS ==========
        private void BindStudentIntakeDropdown()
        {
            ddlStudentIntake.Items.Clear();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = "SELECT Intake_Code, Intake_Name FROM Intake ORDER BY Intake_Code ASC";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            ddlStudentIntake.DataSource = dr;
                            ddlStudentIntake.DataTextField = "Intake_Code";
                            ddlStudentIntake.DataValueField = "Intake_Code";
                            ddlStudentIntake.DataBind();

                            ddlStudentIntake.Items.Insert(0, new ListItem("Select Student Intake Code", ""));
                            btnSaveStudent.Enabled = true;
                        }
                        else
                        {
                            ddlStudentIntake.Items.Add(new ListItem("Currently no intake. Please add an intake first.", ""));
                            btnSaveStudent.Enabled = false;
                            ShowModalError("Currently no intake. Please add an intake first.");
                        }
                    }
                }
            }
        }

        private void BindIntakeMonthYearDropdowns()
        {
            ddlIntakeMonth.Items.Clear();
            ddlIntakeYear.Items.Clear();

            for (int m = 1; m <= 12; m++)
            {
                string monthName = new DateTime(2000, m, 1).ToString("MMMM");
                ddlIntakeMonth.Items.Add(new ListItem(monthName, monthName));
            }

            int currentYear = DateTime.Now.Year;
            for (int y = currentYear - 1; y <= currentYear + 5; y++)
            {
                ddlIntakeYear.Items.Add(new ListItem(y.ToString(), y.ToString()));
            }
        }

        // ========== UI HELPERS ==========
        private void ClearAllModalErrors()
        {
            lblStudentError.Visible = false;
            lblStudentError.Text = "";
            lblLecturerError.Visible = false;
            lblLecturerError.Text = "";
            lblIntakeError.Visible = false;
            lblIntakeError.Text = "";
        }

        private void ShowModalError(string message)
        {
            if (pnlAddStudent.Visible)
            {
                lblStudentError.Text = message;
                lblStudentError.Visible = true;
                pnlModal.Visible = true;
                ViewState["ShowModal"] = true;
            }
            else if (pnlAddLecturer.Visible)
            {
                lblLecturerError.Text = message;
                lblLecturerError.Visible = true;
                pnlModal.Visible = true;
                ViewState["ShowModal"] = true;
            }
            else if (pnlAddIntake.Visible)
            {
                lblIntakeError.Text = message;
                lblIntakeError.Visible = true;
                pnlModal.Visible = true;
                ViewState["ShowModal"] = true;
            }
            else
            {
                pnlModal.Visible = false;
                ViewState["ShowModal"] = false;

                pnlSuccess.Visible = true;
                lblSuccessTitle.InnerText = "Action blocked";
                lblSuccessMessage.Text = message;
                ViewState["ShowSuccess"] = true;
            }
        }

        private void ShowSuccessMessage(string title, string message)
        {
            pnlModal.Visible = false;
            ViewState["ShowModal"] = false;

            pnlSuccess.Visible = true;
            lblSuccessTitle.InnerText = title;
            lblSuccessMessage.Text = message;
            ViewState["ShowSuccess"] = true;
        }

        protected void btnSuccessClose_Click(object sender, EventArgs e)
        {
            pnlSuccess.Visible = false;
            ViewState["ShowSuccess"] = false;
        }

        // ========== ACTION COLUMN ==========
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
                // EDIT
                LinkButton btnEdit = new LinkButton
                {
                    CssClass = "edit-btn",
                    Text = "\uf044"
                };
                btnEdit.Font.Name = "FontAwesome";
                btnEdit.CommandName = "EditRow";
                btnEdit.DataBinding += (s, e) =>
                {
                    LinkButton lb = (LinkButton)s;
                    GridViewRow row = (GridViewRow)lb.NamingContainer;
                    lb.CommandArgument = row.RowIndex.ToString();
                };

                // DELETE
                LinkButton btnDelete = new LinkButton
                {
                    CssClass = "delete-btn",
                    Text = "\uf1f8"
                };
                btnDelete.Font.Name = "FontAwesome";
                btnDelete.CommandName = "ConfirmDelete";
                btnDelete.DataBinding += (s, e) =>
                {
                    LinkButton lb = (LinkButton)s;
                    GridViewRow row = (GridViewRow)lb.NamingContainer;
                    lb.CommandArgument = row.RowIndex.ToString();
                };

                container.Controls.Add(btnEdit);
                container.Controls.Add(new LiteralControl("&nbsp;"));
                container.Controls.Add(btnDelete);
            }
        }
    }
}
