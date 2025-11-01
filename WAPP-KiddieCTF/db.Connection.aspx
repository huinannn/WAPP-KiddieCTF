<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="db.Connection.aspx.cs" Inherits="WAPP_KiddieCTF.db_Connection" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" ProviderName="<%$ ConnectionStrings:ConnectionString.ProviderName %>" SelectCommand="SELECT * FROM [Access_Challenge_Record]"></asp:SqlDataSource>
        </div>
    </form>
</body>
</html>
