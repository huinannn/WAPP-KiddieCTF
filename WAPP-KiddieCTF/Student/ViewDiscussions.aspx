<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewDiscussions.aspx.cs" Inherits="WAPP_KiddieCTF.Student.ViewDiscussions" %>
<%@ Register Src="~/Student/SideBar.ascx" TagPrefix="uc" TagName="SideBar" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Discussion</title>
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
            border-radius: 12px;
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

        .comment-input textarea, .reply-input textarea {
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

        .comment-input textarea:focus, .reply-input textarea:focus {
            outline: none;
            border: none;
        }

        .comment-input .submit-button, .reply-input .submit-button {
            background-color: #1B263B;
            color: white;
            border-radius: 10px;
            aspect-ratio: 100/40;
            min-width: 70px;
            border: none;
            text-decoration: none;
            cursor: pointer;
        }

        .each-comment {
            background-color: #455066;
            border-radius: 10px;
            padding: 20px;
            margin-bottom: 10px;
        }

        .each-comment .comment-element {
            display: flex;
            flex-direction: row;
            gap: 10px;
            align-items: center;

        }

        .each-comment .comment-element p {
            font-size: 10px;
            color: white;
            letter-spacing: 1px;
        }

        .each-comment .comment-element h5 {
            font-size: 15px;
            color: white;
            letter-spacing: 1px;
        }

        .each-comment .comment-element span {
            color: #9BA0A6;
            font-size: 13px;
            cursor: pointer;
            letter-spacing: 1px;
        }

        .replies {
            margin-left: 40px;
            margin-top: 10px;
            width: 90%;
            display: none;
        }

        .replies.show {
            display: block; 
        }

        .each-reply {
            display: flex;
            flex-direction: column;
            margin-bottom: 10px;
        }

        .each-reply .reply-element {
            display: flex;
            gap: 10px;
            align-items: center;
        }

        .each-reply .reply-element p {
            font-size: 10px;
            color: white;
            letter-spacing: 1px;
        }

        .each-reply .reply-element h5 {
            font-size: 13px;
            color: white;
            letter-spacing: 1px;
        }

        .reply-input textarea {
            padding: 10px;
            border-radius: 10px;
        }

    </style>
</head>
<body>
    <form id="form1" runat="server">
        <uc:SideBar ID="SidebarControl" runat="server" />
        <div class="main-content" style="margin-left: 250px; padding: 40px;">
            <div class="head">
                <h2 style="color: white; margin-bottom: 20px;">Discussion Name</h2>
                <div class="spacer"></div>
                <button class="back" onclick="window.location.href='Discussions.aspx'">Back</button>
            </div>

            <div class="discussion-container">
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
                            <textarea class="input" name="comment" placeholder="Leave a Comment..." title="Please leave a comment before you submit!" required="required"></textarea>
                            <button class="submit-button" name="submit" type="submit">Comment</button>
                        </div>
                        <div class="comment-view">
                            <div class="each-comment">
                                <div class="comment-element">
                                    <p>Angeline XC</p>
                                    <p>4/11/2025</p>
                                </div>
                                <div class="comment-element">
                                    <h5>Talk about some comments. I hate WAPP!!!!</h5>
                                    <div class="spacer"></div>
                                    <span class="reply-toggle">Reply</span>
                                </div>
                                <div class="replies">
                                    <div class="each-reply">
                                        <div class="reply-element">
                                            <p>John Doe</p>
                                            <p>4/11/2025</p>
                                        </div>
                                        <div class="reply-element">
                                            <h5>I understand your frustration!</h5>
                                        </div>
                                    </div>
                                    <div class="each-reply">
                                        <div class="reply-element">
                                            <p>Jane Smith</p>
                                            <p>4/11/2025</p>
                                        </div>
                                        <div class="reply-element">
                                            <h5>WAPP can be tricky sometimes.</h5>
                                        </div>
                                    </div>

                                    <div class="reply-input">
                                        <textarea placeholder="Add a reply..." required="required"></textarea>
                                        <button class="submit-button" type="submit">Reply</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
    <script>
        document.querySelectorAll('.reply-toggle').forEach(function(btn) {
            btn.addEventListener('click', function() {
                const replies = this.closest('.each-comment').querySelector('.replies');
                replies.classList.toggle('show');
            });
        });
    </script>
</body>
</html>
