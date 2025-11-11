using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace WAPP_KiddieCTF.Student
{
    public partial class Certificate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string studentId = Session["StudentID"]?.ToString();
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

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
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

            if (certificates.Rows.Count == 0)
            {
                NoCertificatesLabel.Visible = true;
                rptCertificates.Visible = false;
            }
            else
            {
                rptCertificates.DataSource = certificates;
                rptCertificates.DataBind();
                NoCertificatesLabel.Visible = false;
                rptCertificates.Visible = true;
            }
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