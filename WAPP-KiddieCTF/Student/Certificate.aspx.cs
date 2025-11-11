using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace WAPP_KiddieCTF.Student
{
    public partial class Certificate : System.Web.UI.Page
    {
        private readonly string connStr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string studentId = Session["StudentID"] as string;
                if (string.IsNullOrEmpty(studentId))
                {
                    Response.Redirect("~/LogIn.aspx");
                    return;
                }

                LoadCertificates(studentId);
            }
        }

        private void LoadCertificates(string studentId)
        {
            DataTable certificates = new DataTable();

            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(@"
                    SELECT c.Certificate_ID, c.Course_ID, co.Course_Name, s.Student_Name
                    FROM Certificate c
                    INNER JOIN Course co ON c.Course_ID = co.Course_ID
                    INNER JOIN Student s ON c.Student_ID = s.Student_ID
                    WHERE c.Student_ID = @StudentID", con))
                {
                    cmd.Parameters.AddWithValue("@StudentID", studentId);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(certificates);
                    }
                }
            }

            rptCertificates.DataSource = certificates;
            rptCertificates.DataBind();
        }

        public string GenerateCertificateLink(object certificateId, object studentName, object courseName)
        {
            string certId = certificateId.ToString();
            string student = studentName.ToString();
            string course = courseName.ToString();

            string fileName = $"{certId}_{student.Replace(" ", "_")}_{course}.pdf";
            return $"/Uploads/Certificates/{fileName}";
        }
    }
}