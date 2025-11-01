<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Courses.aspx.cs" Inherits="WAPP_KiddieCTF.Lecturer.Courses" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Kiddie CTF - Courses</title>

    <!-- Reusable Sidebar CSS -->
    <link href="css/sidebar.css" rel="stylesheet" runat="server" />

    <!-- Courses Page CSS -->
    <link href="css/courses.css" rel="stylesheet" runat="server" />

    <!-- Google Font: Teko -->
    <link href="https://fonts.googleapis.com/css2?family=Teko:wght@400;500;600&display=swap" rel="stylesheet"/>

</head>
<body>
    <form id="form2" runat="server">
        <!-- === SIDEBAR === -->
        <div class="sidebar">
            <img class="logo" src="images/logo.png" alt="Logo" />
            <nav class="nav">
                <a href="Dashboard.aspx" class="nav-item"><span class="icon dashboard"></span><span class="label">DASHBOARD</span></a>
                <a href="Courses.aspx" class="nav-item active"><span class="icon courses"></span><span class="label">Courses</span></a>
                <a href="Challenges.aspx" class="nav-item"><span class="icon challenges"></span><span class="label">Challenges</span></a>
                <a href="Tools.aspx" class="nav-item"><span class="icon tools"></span><span class="label">Tools</span></a>
                <a href="Dashboard.aspx" class="nav-item"><span class="icon dashboard"></span><span class="label">DASHBOARD</span></a>
            </nav>
            <div class="divider"></div>
            <div class="user-profile">
                <div class="avatar"></div>
                <div class="user-info">
                    <div class="name">Wong Xin Yee</div>
                    <div class="id">LC123456</div>
                </div>
            </div>
            <a href="Login.aspx" class="logout"><span class="icon logout-icon"></span><span class="label">LOG OUT</span></a>
        </div>

        <!-- === MAIN CONTENT === -->
        <div class="main">
            <h1 class="page-title">Courses</h1>

            <!-- Toolbar -->
            <div class="toolbar">
                <div class="search-box"><img src="https://placehold.co/25x27" alt="" /><span>Search Course Name</span></div>
                <div class="filter-box"><img src="https://placehold.co/31x28" alt="" /><span>Filter</span></div>
                <div class="add-box"><img src="https://placehold.co/30x25" alt="" /><span>Add New Course</span></div>
            </div>

            <!-- BIG PANEL: ONLY COURSES -->
            <div class="content-panel">
                <div class="course-grid">
                    <asp:Repeater ID="CourseRepeater" runat="server">
                        <ItemTemplate>
                            <div class="course-card">
                                <button type="button" class="edit-btn" onclick="editCourse('<%# Eval("CourseID") %>')">Edit</button>
                                <h3 class="course-name"><%# Eval("CourseName") %></h3>
                                <p class="intake">Intake Enrolled</p>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </div>
    </form>

    <!-- Optional: JS for Edit Button -->
    <script>
        function editCourse(id) {
            alert("Edit Course ID: " + id);
            // window.location = "EditCourse.aspx?id=" + id;
        }
    </script>
</body>
</html>
