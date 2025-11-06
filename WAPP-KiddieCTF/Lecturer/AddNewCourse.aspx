<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddNewCourse.aspx.cs" Inherits="WAPP_KiddieCTF.Lecturer.AddNewCourse" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Kiddie CTF - Add New Course</title>

    <!-- Reusable Sidebar CSS -->
    <link href="css/sidebar.css" rel="stylesheet" runat="server" />

    <!-- Add New Course Page CSS -->
    <link href="css/addNewCourse.css" rel="stylesheet" runat="server" />

    <!-- Google Font: Teko -->
    <link href="https://fonts.googleapis.com/css2?family=Teko:wght@400;500;600&display=swap" rel="stylesheet"/>
    
    <!-- Alert -->
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>

</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <!-- === SIDEBAR === -->
        <div class="sidebar">
            <img class="logo" src="images/logo.png" alt="Logo" />
            <nav class="nav">
                <a href="Dashboard.aspx" class="nav-item"><span class="icon dashboard"></span><span class="label">DASHBOARD</span></a>
                <a href="Courses.aspx" class="nav-item active"><span class="icon courses"></span><span class="label">Courses</span></a>
                <a href="Challenges.aspx" class="nav-item"><span class="icon challenges"></span><span class="label">Challenges</span></a>
                <a href="Tools.aspx" class="nav-item"><span class="icon tools"></span><span class="label">Tools</span></a>
            </nav>
            <div class="divider"></div>
            <div class="user-profile">
                    <div class="avatar">
                        <img src="images/profile.png" alt="Profile" />
                    </div>
                    <div class="user-info">
                        <div class="name">
                            <asp:Label ID="lblLecturerName" runat="server" Text=""></asp:Label>
                        </div>
                        <div class="id">
                            <asp:Label ID="lblLecturerID" runat="server" Text=""></asp:Label>
                        </div>
                    </div>
                </div>
                <a href="../LogOut.aspx" class="logout">
                    <img src="images/logout.png" alt="Logout" class="logout-img" />
                    <span class="label">LOG OUT</span>
                </a>
        </div>

        <!-- === MAIN CONTENT === -->
        <div class="main">
            <h1 class="page-title">Add New Course</h1>

            <!-- BIG PANEL (Taller, no toolbar) -->
            <div class="content-panel">
                <div class="form-container">
                    <!-- Back Button -->
                    <button type="button" class="back-btn" onclick="window.location='Courses.aspx'">
                        <img src="images/back_icon.png" alt="Back" />
                    </button>

                    <!-- Course ID (Auto) -->
                    <div class="course-id-label">Course ID: <asp:Label ID="lblCourseID" runat="server" Text=""></asp:Label></div>

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

                                <!-- ADD STUDENTS -->
                                <div class="student-btn-wrapper">
                                    <asp:LinkButton ID="btnAddStudents" runat="server" CssClass="student-btn" OnClick="btnAddStudents_Click">
                                        <div class="btn-content">
                                            <img src="images/addStudent.png" alt="Add" class="btn-icon" />
                                            <span class="btn-text">Add Students</span>
                                        </div>
                                    </asp:LinkButton>
                                </div>

                                <!-- VIEW STUDENTS -->
                                <div class="student-btn-wrapper">
                                    <asp:LinkButton ID="btnViewStudents" runat="server" CssClass="student-btn" OnClick="btnViewStudents_Click">
                                        <div class="btn-content">
                                            <img src="images/viewStudent.png" alt="View" class="btn-icon" />
                                            <span class="btn-text">View Students</span>
                                        </div>
                                    </asp:LinkButton>
                                </div>

                            </div>
                        </div>
                    </div>

                    <!-- DONE Button -->
                    <asp:Button ID="btnDone" runat="server" CssClass="done-btn" OnClick="btnDone_Click" Text="DONE" />
                </div>
            </div>
        </div>

    </form>
</body>
</html>
