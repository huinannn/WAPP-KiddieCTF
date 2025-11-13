<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditCourse.aspx.cs" Inherits="WAPP_KiddieCTF.Admin.InnerFunction.EditCourse" %>
<%@ Register Src="~/Admin/SideBar.ascx" TagPrefix="uc" TagName="SideBar" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/sidebar.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
         <!-- Place the ScriptManager at the beginning of the form -->
     <asp:ScriptManager ID="ScriptManager1" runat="server" />

     <div class="sidebar">
         <uc:SideBar ID="SidebarControl" runat="server" />
     </div>
        <div class="main">

        </div>
    </form>
</body>
</html>
