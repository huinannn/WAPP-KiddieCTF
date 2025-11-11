<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StudentList.aspx.cs" Inherits="WAPP_KiddieCTF.Admin.InnerFunction.StudentList" %>
<%@ Register Src="~/Admin/SideBar.ascx" TagPrefix="uc" TagName="SideBar" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Kiddie CTF - Student List (Admin)</title>

    <!-- we are inside /Admin/InnerFunction so go one level up -->
    <link href="../css/sidebar.css" rel="stylesheet" />
    <link href="../css/css2/studentList.css" rel="stylesheet" />

    <!-- Google Font: Teko -->
    <link href="https://fonts.googleapis.com/css2?family=Teko:wght@400;500;600&display=swap" rel="stylesheet"/>

    <!-- Alert -->
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>

    <style>
        /* make room for the fixed 250px sidebar */
        body {
            margin: 0;
            background: #000;
            font-family: 'Teko', sans-serif;
        }
        .main {
            margin-left: 250px;          /* SAME width as sidebar */
            padding: 40px 50px;
            min-height: 100vh;
            background: #000;
            box-sizing: border-box;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <!-- your shared sidebar -->
        <uc:SideBar ID="SidebarControl" runat="server" />

        <!-- main content -->
        <div class="main">
            <h1 class="page-title">Student List</h1>

            <!-- TOOLBAR -->
            <div class="toolbar">
                <!-- BACK BUTTON -->
                <button type="button" class="back-btn" onclick="window.location='../Courses.aspx'">
                    <!-- path from /Admin/InnerFunction to /Admin/images/... -->
                    <img src="../images/back_icon2.png" alt="Back" />
                </button>

                <asp:UpdatePanel ID="UpdatePanelSearch" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="search-box">
                            <img src="../images/search.png" alt="" />
                            <asp:TextBox ID="txtSearch" runat="server"
                                         CssClass="search-input"
                                         AutoPostBack="true"
                                         OnTextChanged="txtSearch_TextChanged">
                            </asp:TextBox>
                            <label class="placeholder-label">Search student...</label>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="txtSearch" EventName="TextChanged" />
                    </Triggers>
                </asp:UpdatePanel>

                <div class="add-box" onclick="location.href='AddStudent.aspx?from=course&course=<%= Request.QueryString["course"] %>'" style="cursor:pointer;">
                    <img src="../images/add_icon.png" alt="" />
                    <span>Add New Student</span>
                </div>
            </div>

            <!-- STUDENT TABLE -->
            <asp:UpdatePanel ID="UpdatePanelStudents" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="content-panel">
                        <!-- HEADER -->
                        <div class="table-header">
                            <div class="col-id">Student ID</div>
                            <div class="col-name">Student Name</div>
                            <div class="col-intake">Student Intake Code</div>
                            <div class="col-action">Action</div>
                        </div>

                        <!-- BODY -->
                        <div class="table-body">
                            <asp:Repeater ID="StudentRepeater" runat="server">
                                <ItemTemplate>
                                    <div class="table-row">
                                        <div class="col-id"><%# Eval("Student_ID") %></div>
                                        <div class="col-name"><%# Eval("Student_Name") %></div>
                                        <div class="col-intake"><%# Eval("Intake_Code") %></div>
                                        <div class="col-action">
                                            <asp:Button ID="btnRemove" runat="server" CssClass="remove-btn"
                                                        Text="Remove"
                                                        CommandArgument='<%# Eval("Student_ID") %>'
                                                        OnClick="btnRemove_Click"
                                                        OnClientClick="return confirm('Remove this student from this course?');" />
                                        </div>
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
        // placeholder sync (same pattern you used)
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
            }
        }
        document.addEventListener("DOMContentLoaded", initPlaceholderEvents);
        if (window.Sys && Sys.Application) {
            Sys.Application.add_load(initPlaceholderEvents);
        }
    </script>
</body>
</html>
