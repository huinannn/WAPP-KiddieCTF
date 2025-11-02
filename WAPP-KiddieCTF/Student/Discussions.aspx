<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Discussions.aspx.cs" Inherits="WAPP_Assignment.Student.Discussions" %>
<%@ Register Src="~/Student/SideBar.ascx" TagPrefix="uc" TagName="SideBar" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Student Discussions</title>
    <link href="css/discussions.css" rel="stylesheet" />
    <link href="~/Images/Logo.png" rel="shortcut icon" type="image/x-icon" />
</head>
<body>
   <form id="form1" runat="server">
        <uc:SideBar ID="SidebarControl" runat="server" />
    </form>
</body>
</html>