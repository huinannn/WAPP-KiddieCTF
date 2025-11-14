<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CourseDetails.aspx.cs" Inherits="WAPP_KiddieCTF.Admin.InnerFunction.CourseDetails" %>
<%@ Register Src="~/Admin/SideBar.ascx" TagPrefix="uc" TagName="SideBar" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Kiddie CTF - Course Details</title>

    <!-- Reusable Sidebar CSS -->
    <link href="../css/sidebar.css" rel="stylesheet" runat="server" />

    <!-- Course Details Page CSS -->
    <link href="../css/css2/courseDetails.css" rel="stylesheet" runat="server" />

    <!-- Google Font: Teko -->
    <link href="https://fonts.googleapis.com/css2?family=Teko:wght@400;500;600&display=swap" rel="stylesheet"/>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <!-- SIDEBAR (Admin Version) -->
        <div class="sidebar">
            <uc:SideBar ID="SidebarControl" runat="server" />
        </div>

        <!-- MAIN CONTENT -->
        <div class="main">
            <h1 class="page-title" style="text-align: center;">
                <asp:Label ID="lblCourseName" runat="server" Text="Course Name (Course Code)"></asp:Label>
            </h1>

            <!-- BIG PANEL -->
            <div class="content-panel">
                <button type="button" class="back-btn" 
                        onclick="window.location.replace('../Courses.aspx?id=<%= Request.QueryString["Course_ID"] %>');">
                    <img src="../images/back_icon.png" alt="Back" />
                </button>
                <asp:UpdatePanel ID="UpdatePanelContent" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <!-- Course Information Section -->
                        <div class="section course-info-section">
                            <div class="section-header">
                                <h2>Course Information</h2>
                            </div>
                            <div class="course-info" style="color:#fff">
                                <strong>Assigned Lecturer: </strong>
                                <asp:Label ID="lblAssignedLecturer" runat="server" Text=""></asp:Label>
                            </div>
                        </div>

                        <!-- Chapters Section -->
                        <div class="section chapters-section" style="margin-top:40px">
                            <div class="section-header">
                                <h2>Chapters</h2>
                            </div>
                            <div class="chapter-list">
                                <asp:Repeater ID="rptChapters" runat="server">
                                    <ItemTemplate>
                                        <div class="chapter-item">
                                            <span class="chapter-link">
                                                <%# Eval("Chapter_Name") %>
                                            </span>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <asp:Literal ID="litNoChapters" runat="server" Text="<em>No chapters added yet.</em>" Visible="false"></asp:Literal>
                            </div>
                            <div class="action-buttons">
                                <asp:Button ID="btnViewProgress" runat="server" 
                                            Text="View Students Progress" 
                                            CssClass="action-btn view-btn" 
                                            OnClick="btnViewProgress_Click" />
                            </div>
                        </div>

                        <!-- Final Assignment Section -->
                        <div class="section assignment-section">
                            <div class="section-header">
                                <h2>Final Assignment</h2>
                            </div>
                            <div class="assignment-details">
                                <asp:Repeater ID="rptAssignment" runat="server">
                                    <ItemTemplate>
                                        <div class="assignment-item">
                                            <span class="assignment-link">
                                                <%# Eval("FA_Name") %>
                                            </span>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <asp:Literal ID="litNoAssignment" runat="server" Text="<em>No final assignment added.</em>" Visible="false"></asp:Literal>
                            </div>
                            <div class="action-buttons">
                                <asp:Button ID="btnViewAssignProgress" runat="server" 
                                            Text="View Students Progress" 
                                            CssClass="action-btn view-btn" 
                                            OnClick="btnViewAssignProgress_Click" />
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </form>

    <script>
        document.addEventListener("DOMContentLoaded", function () {
            const navLinks = document.querySelectorAll(".sidebar .nav a");

            navLinks.forEach(function (link) {
                const href = link.getAttribute("href");
                if (!href) return;
                if (href.startsWith("/") || href.startsWith("http")) return;
                if (href.startsWith("../")) return;

                link.setAttribute("href", "../" + href);
            });

            const path = window.location.pathname.toLowerCase();
            const isChallengeInner =
                path.includes("assignmentprogress") ||
                path.includes("coursedetails");

            if (isChallengeInner) {
                document.querySelectorAll(".sidebar .nav a").forEach(a => a.classList.remove("active"));
                const challengesLink = Array.from(document.querySelectorAll(".sidebar .nav a"))
                    .find(a => (a.getAttribute("href") || "").toLowerCase().includes("courses.aspx"));

                if (challengesLink) {
                    challengesLink.classList.add("active");
                }
            }
        });
    </script>

</body>
</html>
