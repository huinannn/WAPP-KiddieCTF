<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CourseDetails.aspx.cs" Inherits="WAPP_KiddieCTF.Admin.InnerFunction.CourseDetails" %>
<%@ Register Src="~/Admin/SideBar.ascx" TagPrefix="uc" TagName="SideBar" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Kiddie CTF - Course Details (Admin)</title>

    <!-- we are inside /Admin/InnerFunction, so go up one level -->
    <link href="../css/sidebar.css" rel="stylesheet" />
    <link href="../css/css2/courseDetails.css" rel="stylesheet" />

    <!-- fonts -->
    <link href="https://fonts.googleapis.com/css2?family=Teko:wght@400;500;600&display=swap" rel="stylesheet" />

    <style>
        body {
            margin: 0;
            background: #000;
            font-family: 'Teko', sans-serif;
        }

        /* push content to the right of the fixed 250px sidebar */
        .main {
            margin-left: 250px;           /* same as sidebar width */
            width: calc(100% - 250px);    /* take rest of space */
            padding: 40px 50px;
            min-height: 100vh;
            box-sizing: border-box;
            background: #000;
        }

        .page-title {
            font-size: 50px;
            font-weight: 600;
            letter-spacing: 2.5px;
            color: #fff;
            margin-bottom: 30px;
        }

        /* in case your courseDetails.css had display:flex;align-items:center; on .main, this overrides it */
        .main {
            display: block;
        }

        /* small tweak so the big panel centers nicely */
        .content-panel {
            max-width: 1060px;
            background: #1B263B;
            border-radius: 20px;
            padding: 40px;
            position: relative;
        }

        .back-btn {
            background: none;
            border: none;
            cursor: pointer;
            position: absolute;
            top: 25px;
            left: 25px;
        }

        .back-btn img {
            width: 35px;
            height: 35px;
/*            transform: rotate(180deg);*/
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <!-- your admin sidebar -->
        <uc:SideBar ID="SidebarControl" runat="server" />

        <!-- MAIN CONTENT -->
        <div class="main">
            <h1 class="page-title">
                <asp:Label ID="lblCourseName" runat="server" Text="Course Name (Course Code)"></asp:Label>
            </h1>

            <div class="content-panel">
                <!-- simple back to Admin/Courses.aspx -->
                <button type="button" class="back-btn" onclick="window.location='../Courses.aspx'">
                    <img src="../images/back_icon.png" alt="Back" />
                </button>

                <asp:UpdatePanel ID="UpdatePanelContent" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>

                        <!-- Chapters Section -->
                        <div class="section chapters-section">
                            <div class="section-header">
                                <h2>Chapters</h2>
                            </div>
                            <div class="chapter-list">
                                <asp:Repeater ID="rptChapters" runat="server">
                                    <ItemTemplate>
                                        <div class="chapter-item">
                                            <!-- we are still in /Admin/InnerFunction, so link to sibling page -->
                                            <a href='<%# "EditChapter.aspx?chapterid=" + Eval("Chapter_ID") + "&courseid=" + Request.QueryString["id"] %>' class="chapter-link">
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
                                        <a href='<%# "EditAssignment.aspx?faid=" + Eval("FA_ID") + "&courseid=" + Request.QueryString["id"] %>'
                                           class="assignment-link">
                                            <%# Eval("FA_Name") %>
                                        </a>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <asp:Literal ID="litNoAssignment" runat="server" Text="<em>No final assignment added.</em>" Visible="false"></asp:Literal>
                            </div>
                            <div class="action-buttons">
                                <asp:Button ID="btnAddAssignment" runat="server"
                                            Text="Add Assignment"
                                            CssClass="action-btn add-btn"
                                            OnClick="btnAddAssignment_Click" />
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
</body>
</html>
