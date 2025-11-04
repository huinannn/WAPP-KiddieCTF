<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="WAPP_KiddieCTF.Student.Profile" %>
<%@ Register Src="~/Student/SideBar.ascx" TagPrefix="uc" TagName="SideBar" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Student Profile</title>
    <link href="css/profile.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Teko:wght@400;500;600&display=swap" rel="stylesheet"/>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0-beta3/css/all.min.css" rel="stylesheet"/>
    <link href="~/Images/Logo.png" rel="shortcut icon" type="image/x-icon" />
</head>
<body>
    <form id="form1" runat="server">
        <uc:SideBar ID="SidebarControl" runat="server" />
        
        <div class="main-content">
            <div class="profile-title">Profile</div>
          
            <div class="profile-container">
                <div class="profile-id">Student ID: <%= Session["StudentID"] %></div>

                <div class="profile-box">
                    <div class="input-container">
                        <div class="label-p">Student Name</div>
                        <input class="input-field" value="<%= Session["StudentName"] %>" disabled />
                    </div>

                    <div class="input-container">
                        <div class="label-p">Student Intake Code</div>
                        <input class="input-field" value="<%= Session["StudentIntakeCode"] %>" disabled />
                    </div>

                    <div class="input-container">
                        <div class="label-p">Student Password</div>
                        <div class="password-container">
                            <input id="passwordField" class="input-field" value="<%= Session["StudentPassword"] %>" type="password" disabled />
                            <span id="togglePassword" class="eye-icon fa fa-eye-slash" onclick="togglePasswordVisibility()"></span>
                        </div>
                    </div>

                    <a href="EditProfile.aspx" class="edit-link">EDIT</a>

                    <asp:Label ID="lblMessage" runat="server" CssClass="success-label" Visible="false"></asp:Label>
                </div>
            </div>
        </div>
   </form>
</body>

<script>
    function togglePasswordVisibility() {
        var passwordField = document.getElementById('passwordField');
        var eyeIcon = document.getElementById('togglePassword');

        if (passwordField.type === "password") {
            passwordField.type = "text";
            eyeIcon.classList.remove("fa-eye-slash");
            eyeIcon.classList.add("fa-eye");
        } else {
            passwordField.type = "password";
            eyeIcon.classList.remove("fa-eye");
            eyeIcon.classList.add("fa-eye-slash");
        }
    }
</script>
</html>