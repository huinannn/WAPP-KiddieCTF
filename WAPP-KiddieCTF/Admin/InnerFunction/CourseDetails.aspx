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
                                <strong >Assigned Lecturer: </strong>
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
                                            <a href='<%# "EditChapter.aspx?chapterid=" + Eval("Chapter_ID") + "&Course_ID=" + Request.QueryString["Course_ID"] %>' class="chapter-link">
                                                <%# Eval("Chapter_Name") %>
                                            </a>
                                        </div>
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
                            </div>
                            <div class="assignment-details">
                                <asp:Repeater ID="rptAssignment" runat="server">
                                    <ItemTemplate>
                                        <a href='<%# "EditAssignment.aspx?faid=" + Eval("FA_ID") + "&Course_ID=" + Request.QueryString["Course_ID"] %>' 
                                           class="assignment-link">
                                            <%# Eval("FA_Name") %>
                                        </a>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <asp:Literal ID="litNoAssignment" runat="server" Text="<em>No final assignment added.</em>" Visible="false"></asp:Literal>
                            </div>
                            <div class="action-buttons">
                                <asp:Button ID="btnAddAssignment" runat="server" Text="Add Assignment" CssClass="action-btn add-btn" OnClick="btnAddAssignment_Click" />
                                <asp:Button ID="btnViewAssignProgress" runat="server" Text="View Students Progress" CssClass="action-btn view-btn" OnClick="btnViewAssignProgress_Click" />
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </form>
    <script>
        // Navigation Script to highlight active link (for Course Details)
        document.addEventListener("DOMContentLoaded", function () {
            // Update the href links to be relative to the root directory
            document.querySelectorAll(".sidebar .nav a").forEach(link => {
                const href = link.getAttribute("href");
                if (href && !href.startsWith("/") && !href.startsWith("http")) {
                    link.setAttribute("href", "../" + href);
                }
            });

            // Highlight the active link for Course Details
            const courseDetailsLink = document.querySelector('.sidebar .nav a[href*="CourseDetails.aspx"]');
            if (courseDetailsLink) {
                // Remove active class from all links
                document.querySelectorAll('.sidebar .nav a').forEach(a => a.classList.remove('active'));

                // Add active class to the current CourseDetails link
                courseDetailsLink.classList.add('active');
            }
        });
    </script>

</body>
</html>

