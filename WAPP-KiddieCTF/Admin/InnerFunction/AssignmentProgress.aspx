<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AssignmentProgress.aspx.cs" Inherits="WAPP_KiddieCTF.Admin.InnerFunction.AssignmentProgress" %>
<%@ Register Src="~/Admin/SideBar.ascx" TagPrefix="uc" TagName="SideBar" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Kiddie CTF - Assignment Progress</title>

    <link href="../css/sidebar.css" rel="stylesheet" runat="server" />

    <link href="../css/css2/assignmentProgress.css" rel="stylesheet" runat="server" />

    <link href="https://fonts.googleapis.com/css2?family=Teko:wght@400;500;600&display=swap" rel="stylesheet"/>

    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>

</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <!-- SIDEBAR (Admin Version with UC) -->
        <div class="sidebar">
            <uc:SideBar ID="SidebarControl" runat="server" />
        </div>

        <!-- MAIN -->
        <div class="main">
            <h1 class="page-title">Final Assignment Progress</h1>
            <div class="content-wrapper">
                <div class="toolbar">
                    <button type="button" class="back-btn" onclick="history.back()">
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
                                <label class="placeholder-label">Search Student ID/Student Name</label>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="txtSearch" EventName="TextChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>

                <asp:UpdatePanel ID="UpdatePanelProgress" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="content-panel">
                            <div class="table-header">
                                <div class="col-id">Student ID</div>
                                <div class="col-name">Student Name</div>
                                <div class="col-status">Final Assignment Progress</div> <!-- Removed 'Action' column -->
                            </div>

                            <div class="table-body">
                                <asp:Repeater ID="rptProgress" runat="server">
                                    <ItemTemplate>
                                        <div class="table-row">
                                            <div class="col-id"><%# Eval("Student_ID") %></div>
                                            <div class="col-name"><%# Eval("Student_Name") %></div>
                                            <div class="col-status">
                                                <span class='status <%# Eval("Status").ToString().ToLower().Replace(" ", "-") %>'>
                                                    <%# Eval("Status") %>
                                                </span>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>

                                <asp:Literal ID="litNoData" runat="server"
                                             Text="<div class='no-data'>No students enrolled or no assignment set.</div>"
                                             Visible="false"></asp:Literal>
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
        // Placeholder/Floating Label Logic
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
