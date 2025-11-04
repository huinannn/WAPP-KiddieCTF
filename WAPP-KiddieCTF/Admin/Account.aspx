<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Account.aspx.cs" Inherits="WAPP_KiddieCTF.Admin.Account" %>
<%@ Register Src="~/Admin/SideBar.ascx" TagPrefix="uc" TagName="SideBar" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Kiddie CTF | Accounts</title>
    <link href="css/Account.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0-beta3/css/all.min.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Teko:wght@400;500;600&display=swap" rel="stylesheet" />
</head>

<body>
    <form id="form1" runat="server">
        <div class="sidebar">
            <uc:SideBar ID="SidebarControl" runat="server" />
        </div>

        <div class="main">
            <!-- Header -->
            <div class="header">
                <h1>Accounts</h1>
                <div class="search-container" style="font-family: 'Teko', sans-serif;">
                    <asp:TextBox ID="txtSearch" runat="server" CssClass="search-box" placeholder="Search ID" 
                                 style="font-family: 'Teko', sans-serif;"></asp:TextBox>
                    <asp:Button ID="btnFilter" runat="server" CssClass="filter-btn" Text="Search" 
                                style="font-family: 'Teko', sans-serif;" OnClick="btnFilter_Click" />
                    <asp:Button ID="btnAddStudent" runat="server" CssClass="add-btn" Text="Add New Student" 
                                style="font-family: 'Teko', sans-serif;" />
                </div>
            </div>

            <!-- Tabs -->
            <div class="tab-container">
                <asp:Button ID="btnStudentTab" runat="server" CssClass="tab active" Text="Student" OnClick="btnStudentTab_Click" />
                <asp:Button ID="btnLecturerTab" runat="server" CssClass="tab" Text="Lecturer" OnClick="btnLecturerTab_Click" />
                <asp:Button ID="btnIntakeTab" runat="server" CssClass="tab" Text="Intake" OnClick="btnIntakeTab_Click" />
            </div>

            <!-- GridView -->
            <asp:GridView ID="gvAccounts" runat="server" CssClass="account-table" AutoGenerateColumns="false">
            </asp:GridView>

        </div>
    </form>
</body>
</html>

