<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddStudent.aspx.cs" Inherits="WAPP_KiddieCTF.Lecturer.AddStudent" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Kiddie CTF - Add Student</title>

    <!-- Reusable Sidebar CSS -->
    <link href="css/sidebar.css" rel="stylesheet" runat="server" />

    <!-- Courses Page CSS -->
    <link href="css/addStudent.css" rel="stylesheet" runat="server" />

    <!-- Google Font: Teko -->
    <link href="https://fonts.googleapis.com/css2?family=Teko:wght@400;500;600&display=swap" rel="stylesheet"/>

    <!-- Alert -->
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>

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
                    <div class="avatar">
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
            <h1 class="page-title">Add New Student</h1>

            <!-- TOOLBAR -->
            <div class="toolbar">
                <%
                    string fromPage = Request.QueryString["from"];
                    string backUrl = "Courses.aspx"; // default fallback
                    if (fromPage == "add") backUrl = "AddNewCourse.aspx";
                    else if (fromPage == "edit") backUrl = $"EditCourse.aspx?id={Request.QueryString["course"]}";
                %>

                <button type="button" class="back-btn" onclick="window.location.href='<%= backUrl %>';">
                    <img src="images/back_icon2.png" alt="Back" />
                </button>

                <asp:UpdatePanel ID="UpdatePanelSearch" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="search-box">
                            <img src="images/search.png" alt="" />
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
                                                OnClick="btnAdd_Click"
                                                OnClientClick='return confirmAdd("<%# Eval("Student_Name") %>");' />
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>

                            <!-- NO RESULTS -->
                            <asp:Panel ID="pnlNoResults" runat="server" Visible="false" CssClass="no-results">
                                <p>No students found. Try searching by ID, name, or intake code.</p>
                            </asp:Panel>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

    </form>

    <script>
        function showAddConfirm(name) {
            return Swal.fire({
                title: "Add Student?",
                text: "Are you sure you want to add " + name + " to this course?",
                icon: "question",
                showCancelButton: true,
                confirmButtonText: "Yes, add",
                cancelButtonText: "Cancel",
                background: "#1B263B",
                color: "#fff",
                confirmButtonColor: "#3085d6",
                cancelButtonColor: "#d33"
            }).then((result) => {
                return result.isConfirmed; // return true if confirmed
            });
        }

        function confirmAdd(name) {
            event.preventDefault();
            Swal.fire({
                title: "Add Student?",
                text: "Are you sure you want to add " + name + " to this course?",
                icon: "question",
                showCancelButton: true,
                confirmButtonText: "Yes, add",
                cancelButtonText: "Cancel",
                background: "#1B263B",
                color: "#fff",
                confirmButtonColor: "#3085d6",
                cancelButtonColor: "#d33"
            }).then((result) => {
                if (result.isConfirmed) {
                    // manually trigger the original button click
                    event.target.closest("form").submit();
                }
            });
            return false;
        }

        function updatePlaceholder() {
            var txt = document.getElementById('<%= txtSearch.ClientID %>');
            if (!txt) return;

            // Use querySelector inside the UpdatePanel wrapper
            var label = txt.parentNode.querySelector('.placeholder-label') || document.querySelector('.placeholder-label');
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

            // Run on typing
            txt.addEventListener('input', updatePlaceholder);

            // Run after pressing Enter
            txt.addEventListener('keydown', function (e) {
                if (e.key === 'Enter') {
                    setTimeout(updatePlaceholder, 200);
                }
            });
        }

        // Run on first load
        document.addEventListener("DOMContentLoaded", initPlaceholderEvents);

        // Run after every UpdatePanel partial postback
        Sys.Application.add_load(initPlaceholderEvents);
    </script>

</body>
</html>
