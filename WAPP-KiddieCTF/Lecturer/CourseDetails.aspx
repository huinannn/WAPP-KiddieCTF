<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CourseDetails.aspx.cs" Inherits="WAPP_KiddieCTF.Lecturer.CourseDetails" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Kiddie CTF - Course Details</title>

    <!-- Reusable Sidebar CSS -->
    <link href="css/sidebar.css" rel="stylesheet" runat="server" />

    <!-- Course Details Page CSS -->
    <link href="css/courseDetails.css" rel="stylesheet" runat="server" />

    <!-- Google Font: Teko -->
    <link href="https://fonts.googleapis.com/css2?family=Teko:wght@400;500;600&display=swap" rel="stylesheet"/>

</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <!-- SIDEBAR (Same as Courses.aspx) -->
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

        <!-- MAIN CONTENT -->
        <div class="main">
            <h1 class="page-title">
                <asp:Label ID="lblCourseName" runat="server" Text="Course Name (Course Code)"></asp:Label>
            </h1>

            <!-- BIG PANEL -->
            <div class="content-panel">
                <button type="button" class="back-btn" 
                        onclick="window.location.replace('Courses.aspx?id=<%= Request.QueryString["courseid"] %>');">
                    <img src="images/back_icon.png" alt="Back" />
                </button>
                <asp:UpdatePanel ID="UpdatePanelContent" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <!-- Chapters Section -->
                        <div class="section chapters-section">
                            <div class="section-header">
                                <h2>Chapters</h2>
                                <asp:LinkButton ID="lnkEditChapters" runat="server" CssClass="edit-btn" OnClick="lnkEditChapters_Click" Visible="false">
                                    <span>EDIT</span>
                                    <img src="images/edit_icon.png" alt="Edit" />
                                </asp:LinkButton>
                            </div>
                            <div class="chapter-list">
                                <asp:Repeater ID="rptChapters" runat="server">
                                    <ItemTemplate>
                                        <div><%# Eval("Chapter_Name") %></div>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <asp:Literal ID="litNoChapters" runat="server" Text="<em>No chapters added yet.</em>" Visible="false"></asp:Literal>
                            </div>
                            <div class="action-buttons">
                                <asp:Button ID="btnAddChapter" runat="server" 
                                            Text="Add Chapter" 
                                            CssClass="action-btn add-btn" 
                                            OnClick="btnAddChapter_Click" />
                                <asp:Button ID="btnViewProgress" runat="server" Text="View Students Progress" CssClass="action-btn view-btn" OnClick="btnViewProgress_Click" />
                            </div>
                        </div>

                        <!-- Final Assignment Section -->
                        <div class="section assignment-section">
                            <div class="section-header">
                                <h2>Final Assignment</h2>
                                <asp:LinkButton ID="lnkEditAssignment" runat="server" CssClass="edit-btn" OnClick="lnkEditAssignment_Click" Visible="false">
                                    <span>EDIT</span>
                                    <img src="images/edit_icon.png" alt="Edit" />
                                </asp:LinkButton>
                            </div>
                            <div class="assignment-details">
                                <asp:Label ID="lblAssignment" runat="server" Text="Assignment: Not set"></asp:Label>
                            </div>
                            <div class="action-buttons">
                                <asp:Button ID="btnAddAssignment" runat="server" Text="Add Assignment" CssClass="action-btn add-btn" OnClick="btnAddAssignment_Click" />
                                <asp:Button ID="btnViewAssignProgress" runat="server" Text="View Students Progress" CssClass="action-btn view-btn" OnClick="btnViewAssignProgress_Click" />
                            </div>
                            <asp:Literal ID="litNoAssignment" runat="server" Text="<em>No final assignment added.</em>" Visible="false"></asp:Literal>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </form>
</body>
</html>
