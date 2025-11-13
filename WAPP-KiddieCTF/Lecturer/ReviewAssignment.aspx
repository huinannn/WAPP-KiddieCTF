<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReviewAssignment.aspx.cs" Inherits="WAPP_KiddieCTF.Lecturer.ReviewAssignment" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Kiddie CTF - Review Assignment</title>

    <!-- Reusable Sidebar CSS -->
    <link href="css/sidebar.css" rel="stylesheet" runat="server" />

    <!-- Review Assignment Page CSS -->
    <link href="css/reviewAssignment.css" rel="stylesheet" runat="server" />

    <!-- Google Font: Teko -->
    <link href="https://fonts.googleapis.com/css2?family=Teko:wght@400;500;600&display=swap" rel="stylesheet"/>

    <!-- Alert -->
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>

</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <!-- === SIDEBAR & MAIN (100% SAME AS AddChapter.aspx) === -->
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

        <div class="main">
            <h1 class="page-title">Review Assignment</h1>

            <!-- BIG PANEL (FIGMA EXACT) -->
            <div class="content-panel">
            <button type="button" class="back-btn" onclick="history.back()">
                <img src="images/back_icon.png" alt="Back" />
            </button>

            <div class="assignment-id">Assignment ID: <asp:Label ID="lblFAID" runat="server"></asp:Label></div>

            <div class="form-container">
                <!-- Final Assignment Name -->
                <div>
                    <div class="form-label">Final Assignment Name</div>
                    <div class="input-box full-width">
                        <asp:Label ID="lblFAName" runat="server"></asp:Label>
                    </div>
                </div>

                <!-- Student ID & Name -->
                <div class="dual-row">
                    <div>
                        <div class="form-label">Student ID</div>
                        <div class="input-box student-id-box">
                            <asp:Label ID="lblStudentID" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div>
                        <div class="form-label">Student Name</div>
                        <div class="input-box student-name-box">
                            <asp:Label ID="lblStudentName" runat="server"></asp:Label>
                        </div>
                    </div>
                </div>

                <!-- File & Mark -->
                <div class="file-mark-row">
                    <div>
                        <div class="form-label">File Attached</div>
                        <div class="file-box">
                            <asp:HyperLink ID="lnkDownload" runat="server" CssClass="file-link" Target="_blank">
                                <asp:Label ID="lblFileName" runat="server"></asp:Label>
                            </asp:HyperLink>
                        </div>
                    </div>
                    <div>
                        <div class="form-label">Mark</div>
                        <div class="mark-wrapper">
                            <asp:TextBox ID="txtMark" runat="server" CssClass="mark-input" MaxLength="3"></asp:TextBox>
                            <span class="slash">/100</span>
                        </div>
                    </div>
                </div>
            </div>

            <asp:Button ID="btnDone" runat="server" CssClass="done-btn" Text="DONE" OnClick="btnDone_Click" />
        </div>
        </div>
    </form>

    <script>
        const input = document.getElementById('<%= txtMark.ClientID %>');
        input.addEventListener('input', () => {
            input.value = input.value.replace(/[^0-9]/g, '');
            if (input.value > 100) input.value = 100;
        });
    </script>

</body>
</html>
