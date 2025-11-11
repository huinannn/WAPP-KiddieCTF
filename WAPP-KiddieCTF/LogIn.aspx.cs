using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebGrease.Activities;

namespace WAPP_KiddieCTF
{
    public partial class LogIn : System.Web.UI.Page
    {
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                con.Open();

                string userId = txtUserID.Text.Trim();
                string password = txtPassword.Text.Trim();
                string role = ddlRole.SelectedValue;

                string tableName = "";
                string idColumn = "";
                string passColumn = "";

                switch (role)
                {
                    case "Admin":
                        tableName = "Admin";
                        idColumn = "Admin_ID";
                        passColumn = "Admin_Password";
                        break;
                    case "Lecturer":
                        tableName = "Lecturer";
                        idColumn = "Lecturer_ID";
                        passColumn = "Lecturer_Password";
                        break;
                    case "Student":
                        tableName = "Student";
                        idColumn = "Student_ID";
                        passColumn = "Student_Password";
                        break;
                    default:
                        lblError.Visible = true;
                        lblError.Text = "Please select a valid role.";
                        return;
                }

                string query = $"SELECT COUNT(*) FROM {tableName} WHERE {idColumn}=@ID AND {passColumn}=@Password";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ID", userId);
                cmd.Parameters.AddWithValue("@Password", password);

                int count = Convert.ToInt32(cmd.ExecuteScalar());

                if (count == 1)
                {
                    string currentTime = DateTime.Now.ToString("HH:mm:ss");
                    string currentDate = DateTime.Now.ToString("yyyy-MM-dd");

                    string LoginId = "";
                    string prefix = "";
                    string loginTable = "";
                    string loginIdColumn = "";

                    if (role == "Student")
                    {
                        prefix = "SL";
                        loginTable = "Student_Login";
                        loginIdColumn = "StdLogin_ID";

                        string studentNameQuery = "SELECT Student_Name, Intake_Code, Student_Password FROM Student WHERE Student_ID=@StudentID";
                        SqlCommand nameCmd = new SqlCommand(studentNameQuery, con);
                        nameCmd.Parameters.AddWithValue("@StudentID", userId);
                        SqlDataReader reader = nameCmd.ExecuteReader();

                        if (reader.Read())
                        {
                            string studentName = reader["Student_Name"].ToString();
                            string intakeCode = reader["Intake_Code"].ToString();
                            string studentPassword = reader["Student_Password"].ToString();

                            Session["StudentName"] = studentName;
                            Session["StudentID"] = userId;
                            Session["StudentIntakeCode"] = intakeCode;
                            Session["StudentPassword"] = studentPassword;
                        }

                        reader.Close();
                    }
                    else if (role == "Lecturer")
                    {
                        prefix = "LL";
                        loginTable = "Lecturer_Login";
                        loginIdColumn = "LecLogin_ID";

                        string lecturerNameQuery = "SELECT Lecturer_Name FROM Lecturer WHERE Lecturer_ID=@LecturerID";
                        SqlCommand nameCmd = new SqlCommand(lecturerNameQuery, con);
                        nameCmd.Parameters.AddWithValue("@LecturerID", userId);
                        string lecturerName = nameCmd.ExecuteScalar()?.ToString();

                        Session["LecturerName"] = lecturerName;
                        Session["LecturerID"] = userId;
                    }

                    if (!string.IsNullOrEmpty(loginTable))
                    {
                        string getIdQuery = $"SELECT TOP 1 {loginIdColumn} FROM {loginTable} ORDER BY {loginIdColumn} DESC";
                        SqlCommand getIdCmd = new SqlCommand(getIdQuery, con);
                        object result = getIdCmd.ExecuteScalar();

                        if (result != null)
                        {
                            string lastId = result.ToString();
                            int num = int.Parse(lastId.Substring(2)) + 1;
                            LoginId = prefix + num.ToString("D3");
                        }
                        else
                        {
                            LoginId = prefix + "001";
                        }

                        if (role == "Student")
                        {
                            Session["StudentLoginID"] = LoginId;
                        }
                        else if (role == "Lecturer")
                        {
                            Session["LecturerLoginID"] = LoginId;
                        }

                        string insertQuery = "";
                        if (role == "Student")
                        {
                            insertQuery = "INSERT INTO Student_Login (StdLogin_ID, Student_ID, StdLogin_Time, StdLogin_Date) " +
                                          "VALUES (@LoginID, @UserID, @Time, @Date)";
                        }
                        else if (role == "Lecturer")
                        {
                            insertQuery = "INSERT INTO Lecturer_Login (LecLogin_ID, Lecturer_ID, LecLogin_Time, LecLogin_Date) " +
                                          "VALUES (@LoginID, @UserID, @Time, @Date)";
                        }

                        SqlCommand insertCmd = new SqlCommand(insertQuery, con);
                        insertCmd.Parameters.AddWithValue("@LoginID", LoginId);
                        insertCmd.Parameters.AddWithValue("@UserID", userId);
                        insertCmd.Parameters.AddWithValue("@Time", currentTime);
                        insertCmd.Parameters.AddWithValue("@Date", currentDate);
                        insertCmd.ExecuteNonQuery();
                    }

                    switch (role)
                    {
                        case "Admin":
                            Response.Redirect("Admin/Dashboard.aspx");
                            break;
                        case "Lecturer":
                            Response.Redirect("Lecturer/Dashboard.aspx");
                            break;
                        case "Student":
                            Response.Redirect("Student/Dashboard.aspx");
                            break;
                    }
                }
                else
                {
                    lblError.Visible = true;
                    lblError.Text = "Login failed! Invalid ID or password!";
                    txtUserID.Text = "";
                    txtPassword.Text = "";
                    ddlRole.SelectedIndex = 0;
                }
                con.Close();
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Login failed! " + ex.Message;
                txtUserID.Text = "";
                txtPassword.Text = "";
                ddlRole.SelectedIndex = 0;
            }
        }
    }
}