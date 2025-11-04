<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddDiscussions.aspx.cs" Inherits="WAPP_KiddieCTF.Student.AddDiscussions" %>
<%@ Register Src="~/Student/SideBar.ascx" TagPrefix="uc" TagName="SideBar" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>New Discussion</title>
    <link href="css/discussions.css" rel="stylesheet" />
    <style>
        .head {
            display: flex;
            flex-direction: row;
            align-items: center;
        }

        .head .back {
            background-color: #1B263B;
            color: white;
            border-radius: 5px;
            padding: 5px 10px;
            letter-spacing: 1px;
            font-weight: 500;
            min-width: 70px;
            border: none;
            text-decoration: none;
            cursor: pointer;
        }

        .discussion-container {
            background-color: #1B263B;
            border-radius: 20px;
            padding: 40px;
        }

        .each-card {
            background-color: #455066;
            opacity: .7;
            border-radius: 12px;
            margin-bottom: 10px;
        }


    </style>
</head>
<body>
    <form id="form1" runat="server">
        <uc:SideBar ID="SidebarControl" runat="server" />
        <div class="main-content" style="margin-left: 250px; padding: 40px;">
            <div class="head">
                <h2 style="color: white; margin-bottom: 20px;">New Discussion</h2>
                <div class="spacer"></div>
                <button class="back" onclick="window.location.href='Discussions.aspx'">Back</button>
            </div>

            <div class="discussion-container">
                <div class="each-card">

                </div>
                <div class="each-card">

                </div>
                <div class="each-card">

                </div>
            </div>
        </div>
    </form>
</body>
</html>
