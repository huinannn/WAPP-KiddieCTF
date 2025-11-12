<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="WAPP_KiddieCTF.Lecturer.Profile" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Kiddie CTF - Profile</title>

    <!-- Reusable Sidebar CSS -->
    <link href="css/sidebar.css" rel="stylesheet" runat="server" />

    <!-- Profile Page CSS -->
    <link href="css/profile.css" rel="stylesheet" runat="server" />

    <!-- Google Font: Teko -->
    <link href="https://fonts.googleapis.com/css2?family=Teko:wght@400;500;600&display=swap" rel="stylesheet"/>

    <!-- Alert -->
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>

</head>
<body>
    <form id="form1" runat="server">
        <!-- SIDEBAR -->
        <div class="sidebar">
            <img class="logo" src="images/logo.png" alt="Logo" />
            <nav class="nav">
                <a href="Dashboard.aspx" class="nav-item active"><span class="icon dashboard"></span><span class="label">DASHBOARD</span></a>
                <a href="Courses.aspx" class="nav-item"><span class="icon courses"></span><span class="label">Courses</span></a>
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
        <!-- === MAIN CONTENT === -->
        <div class="main-content">
            <div class="profile-title">Profile</div>

            <div class="profile-container">
                <div class="profile-id">Lecturer ID: <%= Session["LecturerID"] %></div>

                <div class="profile-box">
                    <div class="input-container">
                        <div class="label-p">Lecturer Name</div>
                        <input class="input-field" value="<%= Session["LecturerName"] %>" disabled />
                    </div>

                    <div class="input-container">
                        <div class="label-p">Lecturer Password</div>
                        <div class="password-container">
                            <input id="passwordField" class="input-field" value="<%= Session["LecturerPassword"] %>" type="password" disabled />
                            <span id="togglePassword" class="eye-icon fa fa-eye-slash" onclick="togglePasswordVisibility()"></span>
                        </div>
                    </div>

                    <a href="EditProfile.aspx" class="edit-link">EDIT</a>

                    <asp:Label ID="lblMessage" runat="server" CssClass="success-label" Visible="false"></asp:Label>
                </div>
            </div>
        </div>

    </form>

    <script>
        function togglePasswordVisibility() {
            var passwordField = document.getElementById('passwordField');
            var eyeIcon = document.getElementById('togglePassword');
            if (passwordField.type === "password") {
                passwordField.type = "text";
                eyeIcon.classList.replace("fa-eye-slash", "fa-eye");
            } else {
                passwordField.type = "password";
                eyeIcon.classList.replace("fa-eye", "fa-eye-slash");
            }
        }
    </script>

</body>
</html>

