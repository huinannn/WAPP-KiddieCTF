<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddStudent.aspx.cs" Inherits="WAPP_KiddieCTF.Admin.InnerFunction.AddStudent" %>
<%@ Register Src="~/Admin/SideBar.ascx" TagPrefix="uc" TagName="SideBar" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Kiddie CTF - Add Student (Admin)</title>

    <!-- we are in /Admin/InnerFunction, go up one level -->
    <link href="../css/sidebar.css" rel="stylesheet" />
    <link href="../css/css2/addStudent.css" rel="stylesheet" />

    <link href="https://fonts.googleapis.com/css2?family=Teko:wght@400;500;600&display=swap" rel="stylesheet"/>
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>

    <style>
        body {
            margin: 0;
            background: #000;
            font-family: 'Teko', sans-serif;
        }
        /* leave room for the fixed 250px sidebar */
        .main {
            margin-left: 250px;
            width: calc(100% - 250px);
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

        <!-- shared admin sidebar -->
        <uc:SideBar ID="SidebarControl" runat="server" />

        <!-- MAIN CONTENT -->
        <div class="main">
            <h1 class="page-title">Add New Student</h1>

            <!-- TOOLBAR -->
            <div class="toolbar">
                <%
                    // figure out where to go back
                    string fromPage = Request.QueryString["from"];
                    string backUrl = "../Courses.aspx"; // default: admin courses
                    if (fromPage == "add") backUrl = "AddNewCourse.aspx";                 // same folder
                    else if (fromPage == "edit") backUrl = "EditCourse.aspx?id=" + Request.QueryString["course"];
                %>

                <button type="button" class="back-btn" onclick="window.location.href='<%= backUrl %>';">
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
                            <label class="placeholder-label">Search Student ID / Student Name</label>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="txtSearch" EventName="TextChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>

            <!-- STUDENT TABLE -->
            <asp:UpdatePanel ID="UpdatePanelStudents" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="content-panel">
                        <div class="table-header">
                            <div class="col-id">Student ID</div>
                            <div class="col-name">Student Name</div>
                            <div class="col-intake">Student Intake Code</div>
                            <div class="col-action">Action</div>
                        </div>

                        <div class="table-body">
                            <asp:Repeater ID="StudentRepeater" runat="server">
                                <ItemTemplate>
                                    <div class="table-row">
                                        <div class="col-id"><%# Eval("Student_ID") %></div>
                                        <div class="col-name"><%# Eval("Student_Name") %></div>
                                        <div class="col-intake"><%# Eval("Intake_Code") %></div>
                                        <div class="col-action">
                                            <asp:Button ID="btnAdd" runat="server" CssClass="add-btn"
                                                Text="Add"
                                                CommandArgument='<%# Eval("Student_ID") %>'
                                                OnClick="btnAdd_Click" />
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>

                            <asp:Panel ID="pnlNoResults" runat="server" Visible="false" CssClass="no-results">
                                <p>No students found. Try searching by ID, name, or intake code.</p>
                            </asp:Panel>
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
        // same placeholder logic
        function updatePlaceholder() {
            var txt = document.getElementById('<%= txtSearch.ClientID %>');
            if (!txt) return;

            var label = txt.parentNode.querySelector('.placeholder-label');
            if (!label) return;

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
            if (!txt) return;

            txt.addEventListener('input', updatePlaceholder);
            txt.addEventListener('keydown', function (e) {
                if (e.key === 'Enter') {
                    setTimeout(updatePlaceholder, 200);
                }
            });
        }

        document.addEventListener("DOMContentLoaded", initPlaceholderEvents);
        if (window.Sys && Sys.Application) {
            Sys.Application.add_load(initPlaceholderEvents);
        }
    </script>
</body>
</html>
