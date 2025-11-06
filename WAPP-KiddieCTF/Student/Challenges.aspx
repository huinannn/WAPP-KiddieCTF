<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Challenges.aspx.cs" Inherits="WAPP_KiddieCTF.Student.Challenges" %>
<%@ Register Src="~/Student/SideBar.ascx" TagPrefix="uc" TagName="SideBar" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Student Challenges</title>
    <link href="css/challenges.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Teko:wght@400;500;600&display=swap" rel="stylesheet"/>
    <link href="~/Images/Logo.png" rel="shortcut icon" type="image/x-icon" />
</head>
<body>
    <form id="form1" runat="server">
        <uc:SideBar ID="SidebarControl" runat="server" />

        <div class="main-content">
            <div class="challenges-title">Challenges</div>

            <div class="category-tabs">
                <asp:PlaceHolder ID="categoryTabs" runat="server"></asp:PlaceHolder>
            </div>

            <div class="challenges-container">
                <asp:Panel ID="challengeGrid" runat="server" CssClass="challenge-grid"></asp:Panel>
            </div>
        </div>

        <script type="text/javascript">
            function redirectToChallenge(challengeId) {
                window.location.href = 'ChallengeDetails.aspx?challengeId=' + challengeId;
            }
        </script>
    </form>
</body>
</html>