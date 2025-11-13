<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditProfile.aspx.cs" Inherits="WAPP_KiddieCTF.Lecturer.EditProfile" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Kiddie CTF - Edit Profile</title>

    <!-- Reusable Sidebar CSS -->
    <link href="css/sidebar.css" rel="stylesheet" runat="server" />

    <!-- Profile Page CSS -->
    <link href="css/profile.css" rel="stylesheet" runat="server" />

    <!-- Google Font: Teko -->
    <link href="https://fonts.googleapis.com/css2?family=Teko:wght@400;500;600&display=swap" rel="stylesheet"/>

    <!-- Alert -->
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>

    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0-beta3/css/all.min.css" rel="stylesheet"/>

</head>
<body>
    <form id="form1" runat="server">
        <!-- SIDEBAR -->
        <div class="sidebar">
            <img class="logo" src="images/logo.png" alt="Logo" />
            <nav class="nav">
                <a href="Dashboard.aspx" class="nav-item"><span class="icon dashboard"></span><span class="label">DASHBOARD</span></a>
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
        <div class="main-content">
            <div class="profile-title">Edit Profile</div>

            <div class="profile-container">
                <div class="profile-header">
                    <a href="Profile.aspx" class="back-button"><i class="fas fa-arrow-left back-icon"></i></a>
                    <div class="profile-id">Lecturer ID: <%= Session["LecturerID"] %></div>
                </div>

                <div class="profile-box">
                    <div class="input-container">
                        <div class="label-p">Lecturer Name</div>
                        <input id="nameField" name="nameField" class="name-field" value="<%= Session["LecturerName"] %>" />
                    </div>

                    <div class="input-container">
                        <div class="label-p">Lecturer Password</div>
                        <input id="passwordField" name="passwordField" class="password-field" value="<%= Session["LecturerPassword"] %>" />
                    </div>

                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="edit-link" OnClick="SaveProfile" />
                    <asp:Label ID="lblError" runat="server" CssClass="error-label" Visible="false"></asp:Label>
                </div>
            </div>
        </div>

    </form>
</body>
</html>

