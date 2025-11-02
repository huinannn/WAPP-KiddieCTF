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
            <div class="header">
                <h1>Accounts</h1>
                <div class="search-container">
                    <input type="text" placeholder="Search Student ID" class="search-box" />
                    <button class="filter-btn"><i class="fas fa-filter"></i> Filter</button>
                    <button class="add-btn"><i class="fas fa-user-plus"></i> Add New Student</button>
                </div>
            </div>

            <div class="tab-container">
                <button class="tab active">Student</button>
                <button class="tab">Lecturer</button>
                <button class="tab">Intake</button>
            </div>

            <table class="account-table">
                <thead>
                    <tr>
                        <th>Student ID</th>
                        <th>Student Name</th>
                        <th>Student Intake Code</th>
                        <th>Action</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>TP072094</td>
                        <td>Wong Xin Yee</td>
                        <td>AP02F25202CS(CYB)</td>
                        <td class="action-icons">
                            <i class="fas fa-edit"></i>
                            <i class="fas fa-trash"></i>
                        </td>
                    </tr>
                    <tr>
                        <td>TP123456</td>
                        <td>Ahmad Faiz Bin Rahman</td>
                        <td>AP02F25202CS(CYB)</td>
                        <td class="action-icons">
                            <i class="fas fa-edit"></i>
                            <i class="fas fa-trash"></i>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </form>
</body>
</html>
