<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Discussions.aspx.cs" Inherits="WAPP_KiddieCTF.Student.Discussions" %>
<%@ Register Src="~/Student/SideBar.ascx" TagPrefix="uc" TagName="SideBar" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Discussions</title>
    <link href="css/discussions.css" rel="stylesheet" />
    <style>
        .discussion-container {
            background-color: #1B263B;
            border-radius: 20px;
            padding: 40px;
        }

        .discussion-upper {
            display: flex;
            flex-direction: row;
            justify-content: center;
            align-items: center;
            margin-bottom: 30px;
        }

        .each-card {
            background-color: #455066;
            opacity: .7;
            border-radius: 12px;
            padding: 25px;
            margin-bottom: 25px;
            cursor: pointer;
        }

        .discussion-card {
            display: flex;
            align-items: start;
            justify-content: space-between;
        }

        .discussion-left img {
            width: auto;
            height: 200px;
            border-radius: 10px;
            object-fit: cover;
        }

        .discussion-title {
            font-size: 20px;
            font-weight: 600;
            color: white;
        }

        .discussion-subtext {
            color: #9BA0A6;
            font-size: 14px;
            margin-bottom: 20px;
        }

        .discussion-right {
            text-align: right;
            color: #E0E0E0;
            height: 100%;
        }

        .discussion-bottom {
            text-align: right;
            width: 100%;
            color: #E0E0E0;
            display: flex;
            flex-direction: row;
            gap: 10px;
            align-items: center;
            justify-content: flex-end;
            cursor: pointer;
        }

        .discussion-bottom img {
            width: 15px;
            height: 15px;
        }

        .search-bar {
            display: flex;
            align-items: center;
            gap: 10px;
            background-color: #2E3A55;
            border-radius: 10px;
            padding: 15px;
            width: 70%;
        }

        .search-bar img {
            width: 20px;
            height: 20px;
        }

        .search-bar input {
            flex-grow: 1;
            background: none;
            border: none;
            outline: none;
            color: white;
            font-size: 18px;
            font-weight: 500;
            letter-spacing: 0.9px;
        }

        .create-btn {
            background-color: #133B5C;
            border: none;
            color: #9BA0A6;
            aspect-ratio: 111/46;
            max-width: 150px;
            padding: 10px 20px;
            border-radius: 8px;
            font-weight: bold;
            font-size: 18px;
            cursor: pointer;
            display: flex;
            align-items: center;
            justify-content: center;
            gap: 10px;
        }

        .create-btn img {
            width: 20px;
            height: 20px;
        }
    </style>
</head>

<body>
    <form id="form1" runat="server">
        <uc:SideBar ID="SidebarControl" runat="server" />

        <div class="main-content" style="margin-left: 250px; padding: 40px;">
            <h2 style="color: white; margin-bottom: 20px;">Discussions</h2>

            <div class="discussion-upper">
                <div class="search-bar">
                    <img src="../Images/icons/search.png"/>
                    <input type="text" placeholder="Search Discussion" />
                </div>
                <div class="spacer"></div>
                <button class="create-btn" type="button" onclick="window.location.href='AddDiscussions.aspx'"><img src="../Images/icons/add.png"/>CREATE</button>
            </div>

            <div class="discussion-container">
                <asp:Repeater ID="rptDiscussions" runat="server">
                    <ItemTemplate>
                        <div class="each-card" onclick="window.location.href='ViewDiscussions.aspx?id=<%# Eval("Discussion_ID") %>'">
                            <div class="discussion-card">
                                <div class="discussion-left">
                                    <div class="discussion-title"><%# Eval("Discussion_Title") %></div>
                                    <div class="discussion-subtext"><%# Eval("Discussion_Message") %></div>
                                    <img src='../<%# Eval("Discussion_Post") %>' alt="Discussion Image" />
                                </div>
                                <div class="discussion-right">
                                    <div>Posted by: <strong><%# Eval("Student_Name") %></strong></div>
                                    <div>Date: <%# Eval("Discussion_DateTime", "{0:dd/MM/yyyy hh:mm tt}") %></div>
                                </div>
                            </div>
                            <div class="discussion-bottom">
                                <%# Eval("CommentCount") %> Comments
                                <img src="../Images/icons/chat.png" />
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </form>
</body>
</html>

