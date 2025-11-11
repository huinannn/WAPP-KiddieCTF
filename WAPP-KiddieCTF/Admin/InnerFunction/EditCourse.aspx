<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditCourse.aspx.cs" Inherits="WAPP_KiddieCTF.Admin.InnerFunction.EditCourse" %>
<%@ Register Src="~/Admin/SideBar.ascx" TagPrefix="uc" TagName="SideBar" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Kiddie CTF - Edit Course (Admin)</title>

    <!-- we are in /Admin/InnerFunction, so go up one level -->
    <link href="../css/sidebar.css" rel="stylesheet" />
    <link href="../css/css2/addNewCourse.css" rel="stylesheet" />
    <link href="../css/css2/editCourse.css" rel="stylesheet" />

    <link href="https://fonts.googleapis.com/css2?family=Teko:wght@400;500;600&display=swap" rel="stylesheet"/>
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>

    <style>
        body {
            margin: 0;
            background: #000;
            font-family: 'Teko', sans-serif;
        }
        /* leave space for fixed 250px sidebar */
        .main {
            margin-left: 250px;
            width: calc(100% - 250px);
            padding: 40px 50px;
            min-height: 100vh;
            background: #000;
            box-sizing: border-box;
        }
        .page-title {
            font-size: 50px;
            font-weight: 600;
            letter-spacing: 2.5px;
            color: #fff;
            margin-bottom: 20px;
        }
        .content-panel {
            width: 100%;
            background: #1B263B;
            border-radius: 20px;
            padding: 40px;
            position: relative;
        }
        /* make sure the back button points LEFT */
        .back-btn img {
            width: 30px;
            height: 30px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <!-- shared admin sidebar -->
        <uc:SideBar ID="SidebarControl" runat="server" />

        <!-- MAIN CONTENT -->
        <div class="main">
            <h1 class="page-title">Edit Course</h1>

            <div class="content-panel">
                <div class="form-container">
                    <!-- Back Button -->
                    <button type="button" class="back-btn" onclick="window.location='../Courses.aspx'">
                        <!-- path is from /Admin/InnerFunction -->
                        <img src="../images/back_icon.png" alt="Back" />
                    </button>

                    <!-- Course ID (Read-only) -->
                    <div class="course-id-label">Course ID:
                        <asp:Label ID="lblCourseID" runat="server" Text=""></asp:Label>
                    </div>

                    <!-- Course Name Input -->
                    <div class="input-group">
                        <label>Course Name</label>
                        <div class="course-name-box">
                            <asp:TextBox ID="txtCourseName" runat="server" CssClass="course-name-input" placeholder=" "></asp:TextBox>
                            <label class="placeholder-label">Enter course name</label>
                        </div>
                    </div>

                    <!-- Student Enrolled Section -->
                    <div class="student-section">
                        <label>Student Enrolled</label>
                        <div class="student-panel">
                            <div class="student-buttons">
                                <!-- ADD STUDENTS (go to innerfunction AddStudent) -->
                                <div class="student-btn-wrapper">
                                    <asp:LinkButton ID="btnAddStudents" runat="server" CssClass="student-btn" OnClick="btnAddStudents_Click">
                                        <div class="btn-content">
                                            <img src="../images/addStudent.png" alt="Add" class="btn-icon" />
                                            <span class="btn-text">Add Students</span>
                                        </div>
                                    </asp:LinkButton>
                                </div>
                                <!-- VIEW STUDENTS -->
                                <div class="student-btn-wrapper">
                                    <asp:LinkButton ID="btnViewStudents" runat="server" CssClass="student-btn" OnClick="btnViewStudents_Click">
                                        <div class="btn-content">
                                            <img src="../images/viewStudent.png" alt="View" class="btn-icon" />
                                            <span class="btn-text">View Students</span>
                                        </div>
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- DELETE & EDIT BUTTONS -->
                    <div class="action-buttons">
                        <asp:Button ID="btnDelete" runat="server" CssClass="delete-btn"
                            Text="DELETE" OnClick="btnDelete_Click"
                            OnClientClick="return confirm('Are you sure you want to permanently delete this course?');" />
                        <asp:Button ID="btnEdit" runat="server" CssClass="edit-btn" Text="EDIT" OnClick="btnEdit_Click" />
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
