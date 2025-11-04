<%@ Page Title="Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WAPP_KiddieCTF._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.0/css/all.min.css" />
    <link href="https://fonts.googleapis.com/css2?family=Teko:wght@400;500;600&display=swap" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" href="Default.css" />

    <div class="login">
        <div class="left-panel">
            <img src="Images/Login.png" alt="Background" class="background-img" />
        </div>

        <div class="right-panel">
            <img src="Images/Logo.png" alt="Logo" class="logo" />
            <h1 class="welcome-text">WELCOME BACK!</h1>

            <div class="input-group">
                <label class="label">ID:</label>
                <div class="input-box">
                    <i class="fa-solid fa-user icon"></i>
                    <asp:TextBox ID="txtUserID" runat="server" placeholder="Enter your ID" CssClass="input-field"></asp:TextBox>
                </div>
            </div>

            <div class="input-group">
                <label class="label">Password:</label>
                <div class="input-box" style="position: relative;">
                    <i class="fa-solid fa-key icon"></i>
                    <asp:TextBox ID="txtPassword" runat="server" 
                                 TextMode="Password" 
                                 placeholder="Enter your password" 
                                 CssClass="input-field"></asp:TextBox>

                    <i class="fa-solid fa-eye-slash toggle-eye" 
                       id="togglePassword" 
                       onclick="togglePasswordVisibility()"></i>
                </div>
            </div>


            <div class="input-group">
                <label class="label">Role:</label>
                <div class="input-box">
                    <i class="fa-solid fa-users icon"></i>
                    <asp:DropDownList ID="ddlRole" runat="server" CssClass="input-field" AppendDataBoundItems="true">
                        <asp:ListItem Text="Select your role" Value="" disabled="true" selected="true" hidden="true" />
                        <asp:ListItem Text="Admin" Value="Admin" />
                        <asp:ListItem Text="Lecturer" Value="Lecturer" />
                        <asp:ListItem Text="Student" Value="Student" />
                    </asp:DropDownList>
                </div>
            </div>

            <asp:Button ID="btnLogin" runat="server"
                Text="LOGIN" CssClass="login-btn"
                OnClientClick="return validateAndFocus();"
                OnClick="btnLogin_Click" />

            <asp:Label ID="lblError" runat="server" CssClass="error-label" Visible="false"></asp:Label>

            <span id="jsError" class="error-label" style="display:none;"></span>
        </div>
    </div>

    <script type="text/javascript">
        function validateAndFocus() {
            var userId = document.getElementById('<%= txtUserID.ClientID %>');
            var password = document.getElementById('<%= txtPassword.ClientID %>');
            var role = document.getElementById('<%= ddlRole.ClientID %>');
            var jsError = document.getElementById('jsError');
            var serverError = document.getElementById('<%= lblError.ClientID %>');

            jsError.style.display = "none";
            if (serverError) {
                serverError.style.display = "none";
            }
            var isValid = true;

            if (userId.value.trim() === "" || password.value.trim() === "" || role.selectedIndex === 0) {
                isValid = false;
            }

            if (!isValid) {
                jsError.style.display = "block";
                jsError.innerHTML = "Please fill in all the required fields!";

                if (userId.value.trim() === "") {
                    userId.focus();
                } else if (password.value.trim() === "") {
                    password.focus();
                } else if (role.selectedIndex === 0) {
                    role.focus();
                }
                return false;
            }
            return true;
        }

        function togglePasswordVisibility() {
            var passwordField = document.getElementById('<%= txtPassword.ClientID %>');
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
</asp:Content>