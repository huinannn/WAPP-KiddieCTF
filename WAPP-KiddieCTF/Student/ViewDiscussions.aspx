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
            color: white;
            font-size: 20px;
            margin-bottom: 20px;
            letter-spacing: 1px;
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
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
</head>
<body>
    <form id="form1" runat="server">
        <uc:SideBar ID="SidebarControl" runat="server" />
        <div class="main-content" style="margin-left: 250px; padding: 40px;">
            <div class="head">
                <h2 style="color: white; margin-bottom: 20px;"><asp:Label ID="lblTitle" runat="server" /></h2>
                <div class="spacer"></div>
                <button class="back" type="button" onclick="window.location.href='Discussions.aspx'">Back</button>
            </div>

            <div class="discussion-container">
                <div class="each-card">
                    <div class="discussion-card">
                        <div class="discussion-left" style="display: flex; flex-direction:column;">
                            <asp:Label ID="lblMessage" runat="server" CssClass="discussion-subtext" Visible="false" />
                            <asp:Image ID="imgPost" runat="server" Width="300px" CssClass="discussion-img" Visible="false" />
                        </div>
                        <div class="discussion-right">
                            <div>Posted by: <strong><asp:Label ID="lblStudentName" runat="server" /></strong></div>
                            <div>Date: <asp:Label ID="lblDateTime" runat="server" /></div>
                        </div>
                    </div>

                    <div class="comment">
                        <div class="title">
                            <img src="../Images/icons/chat.png" />
                            <h5>Comments</h5>
                        </div>

                       <div class="comment-input">
                            <asp:TextBox ID="txtComment" runat="server" CssClass="input" 
                                         TextMode="MultiLine" placeholder="Leave a Comment..."></asp:TextBox>
                            <asp:Button ID="btnAddComment" runat="server" CssClass="submit-button" 
                                        Text="Comment" OnClick="btnAddComment_Click" />
                        </div>

                        <div class="comment-view">
                            <asp:Repeater ID="rptComments" runat="server">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hfCommentID" runat="server" Value='<%# Eval("Comment_ID") %>' />
                                    <div class="each-comment">
                                        <div class="comment-element">
                                            <p><%# Eval("Student_Name") %></p>
                                            <p><%# Eval("Comment_DateTime", "{0:dd/MM/yyyy hh:mm tt}") %></p>
                                        </div>
                                        <div class="comment-element">
                                            <h5><%# Eval("Comment_Message") %></h5>
                                            <div class="spacer"></div>
                                            <span class="reply-toggle">Reply</span>
                                        </div>

                                        <div class="replies">
                                            <asp:Repeater ID="rptReplies" runat="server">
                                                <ItemTemplate>
                                                    <div class="each-reply">
                                                        <div class="reply-element">
                                                            <p><%# Eval("Student_Name") %></p>
                                                            <p><%# Eval("Reply_DateTime", "{0:dd/MM/yyyy hh:mm tt}") %></p>
                                                        </div>
                                                        <div class="reply-element">
                                                            <h5><%# Eval("Reply_Message") %></h5>
                                                        </div>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:Repeater>

                                            <div class="reply-input">
                                                <asp:TextBox ID="txtReply" runat="server" CssClass="input" 
                                                             TextMode="MultiLine" placeholder="Add a reply..."></asp:TextBox>
                                                <asp:Button ID="btnAddReply" runat="server" CssClass="submit-button" 
                                                            Text="Reply" CommandName="AddReply" CommandArgument='<%# Eval("Comment_ID") %>' OnClick="btnAddReply_Click" />
                                            </div>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>

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
