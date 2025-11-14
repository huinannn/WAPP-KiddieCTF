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
                    <div class="avatar" onclick="window.location='Profile.aspx'" style="cursor:pointer;">
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
            <h1 class="page-title">Courses</h1>

            <!--FIXED WRAPPER -->
            <div class="page-container">

                <!-- === TOOLBAR === -->
                <div class="toolbar">

                    <asp:UpdatePanel ID="UpdatePanelSearch" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="search-box">
                                <img src="images/search.png" alt="" />
                                <asp:TextBox ID="txtSearch" runat="server"
                                             CssClass="search-input"
                                             AutoPostBack="true"
                                             OnTextChanged="txtSearch_TextChanged">
                                </asp:TextBox>
                                <label class="placeholder-label">Search Course Name</label>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="txtSearch" EventName="TextChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                    <div class="add-box" onclick="location.href='AddNewCourse.aspx?from=home'" style="cursor:pointer;">
                        <img src="images/add_icon.png" alt="" />
                        <span>Add New Course</span>
                    </div>
                </div>

                <!-- === COURSE PANEL === -->
                <asp:UpdatePanel ID="UpdatePanelCourses" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="content-panel">
                            <div class="course-grid">
                                <asp:Repeater ID="CourseRepeater" runat="server">
                                    <ItemTemplate>
                                        <div class="course-card" onclick="location.href='CourseDetails.aspx?id=<%# Eval("Course_ID") %>'">
                                            <asp:LinkButton ID="lnkEdit" runat="server"
                                                            CssClass="edit-btn"
                                                            CommandArgument='<%# Eval("Course_ID") %>'
                                                            OnClick="lnkEdit_Click"
                                                            OnClientClick="event.stopPropagation();">
                                                <span>EDIT</span>
                                            </asp:LinkButton>

                                            <h3 class="course-name"><%# Eval("Course_Name") %></h3>
                                            <p class="course-id">Course ID: <%# Eval("Course_ID") %></p>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="txtSearch" EventName="TextChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </div> 
        </div> 
    </form>

    <script>
        // Sync placeholder behavior with server control
        function updatePlaceholder() {
            var txt = document.getElementById('<%= txtSearch.ClientID %>');
            var label = txt ? txt.parentNode.querySelector('.placeholder-label') : null;
            if (!txt || !label) return;

            if (txt.value.trim() === "") {
                label.style.opacity = '1';
                label.style.transform = 'translateY(-50%)';
                label.style.top = '50%';
                label.style.fontSize = '18px';
            } else {
                label.style.opacity = '0';
                label.style.transform = 'translateY(-50%) scale(0.8)';
                label.style.top = '10px';
                label.style.fontSize = '14px';
            }
        }

        function initPlaceholderEvents() {
            updatePlaceholder();

            var txt = document.getElementById('<%= txtSearch.ClientID %>');
            if (txt) {
                txt.addEventListener('input', updatePlaceholder);
                txt.addEventListener('keydown', function (e) {
                    if (e.key === 'Enter') {
                        setTimeout(updatePlaceholder, 200);
                    }
                });
            }
        }

        // Run once when page first loads
        document.addEventListener("DOMContentLoaded", initPlaceholderEvents);

        // Run again after every partial postback (UpdatePanel)
        Sys.Application.add_load(initPlaceholderEvents);

        function editCourse(id) {
            //Debug purposes
            //alert("Edit Course ID: " + id);
            // window.location = "EditCourse.aspx?id=" + id;
        }
    </script>

</body>
</html>
