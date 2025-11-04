<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewDiscussions.aspx.cs" Inherits="WAPP_KiddieCTF.Student.ViewDiscussions" %>
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

        .each-card {
            background-color: #455066;
            opacity: .7;
            border-radius: 12px;
            padding: 25px;
            min-height: 100vh;
        }

        .discussion-card {
            display: flex;
            align-items: start;
            justify-content: space-between;
            margin-bottom: 20px;
        }

        .discussion-left img {
            width: 300px;
            height: auto;
            border-radius: 10px;
            object-fit: cover;
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

        .comment {

        }

        .comment .title {
            display: flex;
            flex-direction: row;
            gap: 10px;
            align-items: center;
        }

        .comment img {
            width: 15px;
            height: 15px;
        }

        .comment h5 {
            font-size: 15px;
            color: white;
            font-weight: 500;
        }

        .comment-input {
            background-color: white;
            border-radius: 10px;
            padding: 20px;
            margin: 10px 0;
            opacity: 1;
        }

        .comment-input textarea {
            width: 100%;
            height: 80px;
            text-decoration: none;
            border: none;
            color: black;
            background-color: white;
            font-weight: 500;
            caret-color: black;
            opacity: 1;
        }

        .comment-input textarea:focus {
            outline: none;
            border: none;
        }

        .comment-input .submit-button {
            background-color: #1B263B;
            color: white;
            border-radius: 10px;
            aspect-ratio: 100/40;
            min-width: 70px;
            border: none;
            text-decoration: none;
            cursor: pointer;
        }

    </style>
</head>
<body>
    <form id="form1" runat="server">
        <uc:SideBar ID="SidebarControl" runat="server" />
        <div class="main-content" style="margin-left: 250px; padding: 40px;">
            <h2 style="color: white; margin-bottom: 20px;">Discussion Name</h2>

            <div class="discussion-container" style="background-color: #1B263B;">
                <div class="each-card">
                    <div class="discussion-card">
                        <div class="discussion-left">
                            <div class="discussion-subtext">About cyber security topics...</div>
                            <img src="../Images/discussion/discussion.jpg"/>
                        </div>
                        <div class="discussion-right">
                            <div>Posted by: <strong>Angeline</strong></div>
                            <div>Date: 4/112025</div>
                        </div>
                    </div>
                    <div class="comment">
                        <div class="title">
                            <img src="../Images/icons/chat.png" />
                            <h5>Comment</h5>
                        </div>
                        <div class="comment-input">
                            <textarea class="input" name="comment" placeholder="Leave a Comment..." required="required"></textarea>
                            <button class="submit-button" name="submit" type="submit">Comment</button>
                        </div>
                    </div>
                </div>
            </div>

        </div>
    </form>
</body>
</html>
