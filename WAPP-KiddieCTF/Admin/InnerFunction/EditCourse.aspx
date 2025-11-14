<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditCourse.aspx.cs" Inherits="WAPP_KiddieCTF.Admin.InnerFunction.EditCourse" %>
<%@ Register Src="~/Admin/SideBar.ascx" TagPrefix="uc" TagName="SideBar" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Kiddie CTF - Edit Course</title>

    <link href="../css/sidebar.css" rel="stylesheet" />
    <link href="../css/css2/editCourse.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Teko:wght@400;500;600&display=swap" rel="stylesheet"/>

    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <!-- SIDEBAR -->
        <div class="sidebar">
            <uc:SideBar ID="SidebarControl" runat="server" />
        </div>

        <!-- MAIN CONTENT -->
        <div class="main">
            <h1 class="page-title">Edit Course</h1>

            <div class="content-panel">

                <button type="button" class="back-btn" onclick="window.location='../Courses.aspx'">
                    <img src="../images/back_icon.png" alt="Back" />
                </button>

                <div class="course-id-label">
                    Course ID: <asp:Label ID="lblCourseID" runat="server"></asp:Label>
                </div>

                <!-- COURSE NAME -->
                <div class="input-group">
                    <label>Course Name</label>
                    <div class="course-name-box">
                        <asp:TextBox ID="txtCourseName" runat="server"
                            CssClass="course-name-input" placeholder=" "></asp:TextBox>
                    </div>
                </div>

                <!-- LECTURER DROPDOWN -->
                <div class="input-group">
                    <label>Assign Lecturer</label>
                    <div class="lecturer-box">
                        <asp:DropDownList ID="ddlLecturer" runat="server"
                            CssClass="lecturer-dropdown"></asp:DropDownList>
                    </div>
                </div>

                <!-- STUDENT BUTTONS -->
                <div class="student-section">
                    <label>Student Enrolled</label>

                    <div class="student-buttons">

                        <div class="student-btn-wrapper">
                            <asp:LinkButton ID="btnAddStudents" runat="server"
                                CssClass="student-btn" OnClick="btnAddStudents_Click">
                                <div class="btn-content">
                                    <img src="../images/addStudent.png" class="btn-icon" />
                                    <span class="btn-text">Add Students</span>
                                </div>
                            </asp:LinkButton>
                        </div>

                        <div class="student-btn-wrapper">
                            <asp:LinkButton ID="btnViewStudents" runat="server"
                                CssClass="student-btn" OnClick="btnViewStudents_Click">
                                <div class="btn-content">
                                    <img src="../images/viewStudent.png" class="btn-icon" />
                                    <span class="btn-text">View Students</span>
                                </div>
                            </asp:LinkButton>
                        </div>

                    </div>
                </div>

                <!-- ACTION BUTTONS -->
                <div class="action-buttons">
                    <asp:Button ID="btnDelete" runat="server" Text="DELETE"
                        CssClass="delete-btn" OnClick="btnDelete_Click"
                        OnClientClick="return showDeleteConfirm();" />

                    <asp:Button ID="btnEdit" runat="server" Text="EDIT"
                        CssClass="edit-btn" OnClick="btnEdit_Click" />
                </div>

            </div>
        </div>
    </form>

    <!-- DELETE CONFIRM -->
    <script>
        function showDeleteConfirm() {
            Swal.fire({
                title: "Are you sure?",
                text: "This course and all assigned students will be permanently deleted.",
                icon: "warning",
                background: "#1B263B",
                color: "#fff",
                showCancelButton: true,
                confirmButtonColor: "#d33",
                cancelButtonColor: "#3085d6",
                confirmButtonText: "Yes, delete it!"
            }).then((result) => {
                if (result.isConfirmed) {
                    __doPostBack('<%= btnDelete.ClientID %>', '');
                }
            });
            return false;
        }
        document.addEventListener("DOMContentLoaded", function () {
            // we are in /Admin/InnerFunction/*

            // 1) fix sidebar nav links so they point back to /Admin/*.aspx
            const navLinks = document.querySelectorAll(".sidebar .nav a");

            navLinks.forEach(function (link) {
                const href = link.getAttribute("href");
                if (!href) return;

                // skip absolute paths or full urls
                if (href.startsWith("/") || href.startsWith("http")) return;

                // if it already starts with ../ then it's already fixed
                if (href.startsWith("../")) return;

                // otherwise, prepend ../
                link.setAttribute("href", "../" + href);
            });

            // 2) force the correct link to be active on inner pages

            // Determine if this is a Course inner page (EditCourse, AddCourse, etc.)
            const path = window.location.pathname.toLowerCase();
            const isCourseInner = path.includes("editcourse") || path.includes("addcourse");

            // Add other course-related inner pages if needed:
            // || path.includes("addstudents") || path.includes("viewstudents"); 

            if (isCourseInner) {
                // remove active from all first
                document.querySelectorAll(".sidebar .nav a").forEach(a => a.classList.remove("active"));

                // find the Courses link (should contain "Courses.aspx")
                const coursesLink = Array.from(document.querySelectorAll(".sidebar .nav a"))
                    .find(a => (a.getAttribute("href") || "").toLowerCase().includes("courses.aspx"));

                if (coursesLink) {
                    coursesLink.classList.add("active");
                }
            }

            // Keep the original Challenges logic for other Challenge inner pages (if they use this script)
            const isChallengeInner =
                path.includes("addchallenge") ||
                path.includes("editchallenge") ||
                path.includes("challengedetails");

            if (isChallengeInner) {
                // remove active from all first (already done above, but safe to repeat if you only use this section)
                // document.querySelectorAll(".sidebar .nav a").forEach(a => a.classList.remove("active"));

                // find the Challenges link
                const challengesLink = Array.from(document.querySelectorAll(".sidebar .nav a"))
                    .find(a => (a.getAttribute("href") || "").toLowerCase().includes("challenges.aspx"));

                if (challengesLink) {
                    challengesLink.classList.add("active");
                }
            }
        });
    </script>
   

</body>
</html>
