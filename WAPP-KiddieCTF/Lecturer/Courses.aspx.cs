using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WAPP_KiddieCTF.Lecturer
{
    public partial class Courses : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCourses();
            }
        }

        private void BindCourses()
        {
            // Sample data (replace with DB call)
            var courses = new[]
            {
        new { CourseID = 1, CourseName = "Course Name 1" },
        new { CourseID = 2, CourseName = "Course Name 2" },
        new { CourseID = 3, CourseName = "Course Name 3" },
        new { CourseID = 4, CourseName = "Course Name 4" },
        new { CourseID = 5, CourseName = "Course Name 5" },
        new { CourseID = 6, CourseName = "Course Name 6" }
    };

            CourseRepeater.DataSource = courses;
            CourseRepeater.DataBind();
        }
    }
}