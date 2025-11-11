<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Courses.aspx.cs" Inherits="WAPP_KiddieCTF.Admin.Courses" %>
<%@ Register Src="~/Admin/SideBar.ascx" TagPrefix="uc" TagName="SideBar" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Kiddie CTF - Courses (Admin)</title>

    <!-- sidebar + page css -->
    <link href="css/sidebar.css" rel="stylesheet" />
    <link href="css/css2/courses.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Teko:wght@400;500;600&display=swap" rel="stylesheet"/>
</head>
<body>
    <form id="form1" runat="server">
        <!-- script manager goes here, no sidebar wrapper -->
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <!-- actual sidebar -->
        <uc:SideBar ID="SidebarControl" runat="server" />

        <!-- main content, pushed right by sidebar width -->
        <div class="main" style="margin-left:250px;">
            <h1 class="page-title">Courses</h1>

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

            <asp:UpdatePanel ID="UpdatePanelCourses" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="content-panel">
                        <div class="course-grid">
                            <asp:Repeater ID="CourseRepeater" runat="server">
                                <ItemTemplate>
                                    <div class="course-card"
                                         onclick="location.href='InnerFunction/CourseDetails.aspx?id=<%# Eval("Course_ID") %>'">
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

        document.addEventListener("DOMContentLoaded", initPlaceholderEvents);
        Sys.Application.add_load(initPlaceholderEvents);
    </script>
</body>
</html>
