<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Courses.aspx.cs" Inherits="WAPP_KiddieCTF.Admin.Courses" %>
<%@ Register Src="~/Admin/SideBar.ascx" TagPrefix="uc" TagName="SideBar" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Kiddie CTF - Courses</title>

    <!-- Reusable Sidebar CSS -->
    <link href="css/sidebar.css" rel="stylesheet" runat="server" />

    <!-- Courses Page CSS -->
    <link href="css/css2/courses.css" rel="stylesheet" runat="server" />

    <!-- Google Font: Teko -->
    <link href="https://fonts.googleapis.com/css2?family=Teko:wght@400;500;600&display=swap" rel="stylesheet"/>
</head>
<body>
    <form id="form1" runat="server">
        <!-- Place the ScriptManager at the beginning of the form -->
        <asp:ScriptManager ID="ScriptManager1" runat="server" />

        <div class="sidebar">
            <uc:SideBar ID="SidebarControl" runat="server" />
        </div>

        <!-- === MAIN CONTENT === -->
        <div class="main">
            <h1 class="page-title">Courses</h1>

            <!-- Toolbar with Real Search -->
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

                <div class="add-box" onclick="location.href='InnerFunction/AddNewCourse.aspx'" style="cursor:pointer;">
                    <img src="images/add_icon.png" alt="" />
                    <span>Add New Course</span>
                </div>
            </div>

            <!-- Wrap Course Panel in UpdatePanel -->
            <asp:UpdatePanel ID="UpdatePanelCourses" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="content-panel">
                        <div class="course-grid">
                            <asp:Repeater ID="CourseRepeater" runat="server">
                                <ItemTemplate>
                                    <div class="course-card" onclick="location.href='<%# ResolveUrl("~/Admin/InnerFunction/CourseDetails.aspx?Course_ID=" + Eval("Course_ID")) %>'">
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
    </script>
</body>
</html>
