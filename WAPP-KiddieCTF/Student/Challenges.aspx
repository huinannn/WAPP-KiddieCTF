<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Challenges.aspx.cs" Inherits="WAPP_KiddieCTF.Student.Challenges" %>
<%@ Register Src="~/Student/SideBar.ascx" TagPrefix="uc" TagName="SideBar" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Student Challenges</title>
    <link href="css/challenges.css" rel="stylesheet" />
    <link href="~/Images/Logo.png" rel="shortcut icon" type="image/x-icon" />
</head>
<body>
   <form id="form1" runat="server">
        <uc:SideBar ID="SidebarControl" runat="server" />
    </form>
</body>
</html>