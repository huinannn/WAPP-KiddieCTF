<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Discussions.aspx.cs" Inherits="WAPP_Assignment.Student.Discussions" %>
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
            margin-left: 250px;
        }

        .discussion-upper {
            display: flex;
            flex-direction: row;
            justify-content: center;
            align-items: center;
        }

        .discussion-card {
            background-color: #2E3A55;
            border-radius: 12px;
            padding: 20px;
            margin-bottom: 25px;
            display: flex;
            align-items: center;
            justify-content: space-between;
        }

        .discussion-left {
            display: flex;
            align-items: flex-start;
            gap: 20px;
        }

        .discussion-left img {
            width: 140px;
            height: 120px;
            border-radius: 8px;
        }

        .discussion-title {
            font-size: 20px;
            font-weight: 600;
            color: white;
        }

        .discussion-subtext {
            color: #9BA0A6;
            font-size: 14px;
        }

        .discussion-right {
            text-align: right;
            color: #9BA0A6;
        }

        .search-bar {
            display: flex;
            align-items: center;
            gap: 10px;
            background-color: #2E3A55;
            border-radius: 10px;
            padding: 10px 15px;
            margin-bottom: 30px;
            width: 70%;
        }

        .search-bar input {
            flex-grow: 1;
            background: none;
            border: none;
            outline: none;
            color: white;
            font-size: 15px;
        }

        .create-btn {
            background-color: #133B5C;
            border: none;
            color: white;
            aspect-ratio: 111/46;
            max-width: 150px;
            padding: 10px 20px;
            border-radius: 8px;
            font-weight: bold;
            cursor: pointer;
        }
    </style>
</head>

<body style="background-color: #0B132B;">
    <form id="form1" runat="server">
        <uc:SideBar ID="SidebarControl" runat="server" />

        <div class="discussion-container">
            <h2 style="color: white; font-size: 28px; margin-bottom: 20px;">Discussions</h2>

            <div class="discussion-upper">
                <div class="search-bar">
                    <input type="text" placeholder="Search Discussion" />
                </div>
                <div class="spacer"></div>
                <button class="create-btn" type="button">CREATE</button>
            </div>

            <div class="discussion-card">
                <div class="discussion-left">
                    <img src="https://cdn.pixabay.com/photo/2016/03/09/09/31/computer-1245714_960_720.jpg" alt="Discussion Image" />
                    <div>
                        <div class="discussion-title">Discussion Name</div>
                        <div class="discussion-subtext">About tool or hacking...</div>
                    </div>
                </div>
                <div class="discussion-right">
                    <div>Posted by: <strong>Angeline 22</strong></div>
                    <div>2 hours ago</div>
                    <div>3 comments 💬</div>
                </div>
            </div>

            <div class="discussion-card">
                <div class="discussion-left">
                    <img src="https://cdn.pixabay.com/photo/2017/01/06/19/15/hacker-1952027_960_720.jpg" alt="Discussion Image" />
                    <div>
                        <div class="discussion-title">Discussion Name</div>
                        <div class="discussion-subtext">About cyber security topics...</div>
                    </div>
                </div>
                <div class="discussion-right">
                    <div>Posted by: <strong>Angeline 22</strong></div>
                    <div>1 day ago</div>
                    <div>5 comments 💬</div>
                </div>
            </div>

            <div class="discussion-card">
                <div class="discussion-left">
                    <img src="https://cdn.pixabay.com/photo/2016/03/09/09/31/computer-1245714_960_720.jpg" alt="Discussion Image" />
                    <div>
                        <div class="discussion-title">Discussion Name</div>
                        <div class="discussion-subtext">About CTF or security tools...</div>
                    </div>
                </div>
                <div class="discussion-right">
                    <div>Posted by: <strong>Angeline 22</strong></div>
                    <div>3 days ago</div>
                    <div>1 comment 💬</div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
