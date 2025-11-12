using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WAPP_KiddieCTF
{
    public partial class LogOut : System.Web.UI.Page
    {
       protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["StudentID"] != null || Session["LecturerID"] != null || Session["AdminID"] != null)
            {
                try
                {
                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                    con.Open();

                    string logoutTime = DateTime.Now.ToString("HH:mm:ss");
                    string logoutDate = DateTime.Now.ToString("yyyy-MM-dd");

                    string userId = "";
                    string loginTable = "";
                    string loginIdColumn = "";
                    string logoutTimeColumn = "";
                    string logoutDateColumn = "";
                    string loginId = "";
                    string userIdColumn = "";

                    if (Session["StudentID"] != null)
                    {
                        userId = Session["StudentID"].ToString();
                        loginTable = "Student_Login";
                        loginIdColumn = "StdLogin_ID";
                        logoutTimeColumn = "StdLogout_Time";
                        logoutDateColumn = "StdLogout_Date";
                        loginId = Session["StudentLoginID"]?.ToString();
                        userIdColumn = "Student_ID";
                    }
                    else if (Session["LecturerID"] != null)
                    {
                        userId = Session["LecturerID"].ToString();
                        loginTable = "Lecturer_Login";
                        loginIdColumn = "LecLogin_ID";
                        logoutTimeColumn = "LecLogout_Time";
                        logoutDateColumn = "LecLogout_Date";
                        loginId = Session["LecturerLoginID"]?.ToString();
                        userIdColumn = "Lecturer_ID";          
                    }

                    string updateQuery = $"UPDATE {loginTable} SET {logoutTimeColumn}=@LogoutTime, {logoutDateColumn}=@LogoutDate " +
                                         $"WHERE {loginIdColumn}=@LoginID AND {userIdColumn}=@UserID";

                    SqlCommand updateCmd = new SqlCommand(updateQuery, con);
                    updateCmd.Parameters.AddWithValue("@LogoutTime", logoutTime);
                    updateCmd.Parameters.AddWithValue("@LogoutDate", logoutDate);
                    updateCmd.Parameters.AddWithValue("@LoginID", loginId);
                    updateCmd.Parameters.AddWithValue("@UserID", userId);

                    int rowsAffected = updateCmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Session["StudentID"] = null;
                        Session["LecturerID"] = null;
                        Session["AdminID"] = null;
                        Response.Redirect("/LogIn.aspx");
                    }
                    else
                    {
                        Response.Redirect("/LogIn.aspx");
                    }

                    Session.Clear();
                    Session.Abandon();
                }
                catch (Exception ex)
                {
                    Response.Write("Error: " + ex.Message);
                }
            }
            else
            {
                Response.Redirect("/LogIn.aspx");
            }
        }
    }
}