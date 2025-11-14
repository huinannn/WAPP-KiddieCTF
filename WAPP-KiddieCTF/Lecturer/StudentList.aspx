<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StudentList.aspx.cs" Inherits="WAPP_KiddieCTF.Lecturer.StudentList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Kiddie CTF - Student List</title>

    <!-- Reusable Sidebar CSS -->
    <link href="css/sidebar.css" rel="stylesheet" runat="server" />

    <!-- Student List Page CSS -->
    <link href="css/studentList.css" rel="stylesheet" runat="server" />

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
                    <div class="avatar" onclick="window.location='Profile.aspx'" style="cursor:pointer;">
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
            <h1 class="page-title">Student List</h1>
            <div class="content-wrapper">

                <!-- TOOLBAR (Back + Search + Filter + Add) -->
                <div class="toolbar">
                    <!-- BACK BUTTON -->
                   <button type="button" class="back-btn" onclick="goBack();">
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
                                <label class="placeholder-label">Search Course Name</label>
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
                        <!-- CONTENT PANEL -->
                        <div class="content-panel">
                            <!-- TABLE HEADER -->
                            <div class="table-header">
                                <div class="col-id">Student ID</div>
                                <div class="col-name">Student Name</div>
                                <div class="col-intake">Student Intake Code</div>
                                <div class="col-action">Action</div>
                            </div>

                            <!-- TABLE BODY (SCROLLABLE) -->
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
                                                            UseSubmitBehavior="false"
                                                            OnClientClick="return sweetRemoveConfirm(this);"
                                                            OnClick="btnRemove_Click" />
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

        function sweetRemoveConfirm(btn) {
            Swal.fire({
                title: "Remove Student?",
                text: "This student will be removed from the course.",
                icon: "warning",
                background: "#1B263B",
                color: "#fff",
                showCancelButton: true,
                confirmButtonColor: "#d33",
                cancelButtonColor: "#3085d6",
                confirmButtonText: "Yes, remove"
            }).then((result) => {
                if (result.isConfirmed) {
                    __doPostBack(btn.name, "");
                }
            });

            return false;
        }

        function goBack() {
            const params = new URLSearchParams(window.location.search);
            const courseId = params.get("course");
            const from = params.get("from");

            if (from === "add") {
                window.location = "AddNewCourse.aspx?course=" + courseId;
                return;
            }

            if (from === "edit") {
                window.location = "EditCourse.aspx?id=" + courseId;
                return;
            }

            window.location = "Courses.aspx";
        }

    </script>

</body>
</html>

