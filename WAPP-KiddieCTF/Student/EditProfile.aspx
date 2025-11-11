<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditProfile.aspx.cs" Inherits="WAPP_Assignment.Student.EditProfile" %>
<%@ Register Src="~/Student/SideBar.ascx" TagPrefix="uc" TagName="SideBar" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Edit Profile</title>
    <link href="css/profile.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Teko:wght@400;500;600&display=swap" rel="stylesheet"/>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0-beta3/css/all.min.css" rel="stylesheet"/>
    <link href="~/Images/Logo.png" rel="shortcut icon" type="image/x-icon" />
</head>
<body>
    <form id="form1" runat="server">
        <uc:SideBar ID="SidebarControl" runat="server" />
    
        <div class="main-content">
            <div class="profile-title">Edit Profile</div>

            <div class="profile-container">
                <div class="profile-header">
                    <a href="Profile.aspx" class="back-button">
                        <i class="fas fa-arrow-left back-icon"></i>
                    </a>
                    <div class="profile-id">Student ID: <%= Session["StudentID"] %></div>
                </div>

                <div class="profile-box">
                    <div class="input-container">
                        <div class="label-p">Student Name</div>
                        <input id="nameField" class="name-field" name="nameField" value="<%= Session["StudentName"] %>" />
                    </div>

                    <div class="input-container">
                        <div class="label-p">Student Intake Code</div>
                        <input class="input-field" value="<%= Session["StudentIntakeCode"] %>" disabled />
                    </div>

                    <div class="input-container">
                        <div class="label-p">Student Password</div>
                        <input id="passwordField" class="password-field" name="passwordField" value="<%= Session["StudentPassword"] %>" />
                    </div>

                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="edit-link" OnClick="SaveProfile" />

                    <asp:Label ID="lblError" runat="server" CssClass="error-label" Visible="false"></asp:Label>
                </div>
            </div>
        </div>
    </form>
</body>
</html>