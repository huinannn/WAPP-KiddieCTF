<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChallengeDetails.aspx.cs" Inherits="WAPP_KiddieCTF.Student.ChallengeDetails" %>
<%@ Register Src="~/Student/SideBar.ascx" TagPrefix="uc" TagName="SideBar" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Challenge Details</title>
    <link href="css/challenges.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Teko:wght@400;500;600&display=swap" rel="stylesheet"/>
    <link href="~/Images/Logo.png" rel="shortcut icon" type="image/x-icon" />
</head>
<body>
    <form id="form1" runat="server">
        <uc:SideBar ID="SidebarControl" runat="server" />

        <div class="main-content">
            <div class="challenges-title">Challenge Details</div>

            <div class="challenge-details">
                <a href="Challenges.aspx" class="back-button">
                    <i class="fas fa-arrow-left back-icon"></i>
                </a>

                <asp:Panel ID="challengeDetailPanel" runat="server">
                    <div class="challenge-header" runat="server" id="challengeHeader" Visible="false">
                        <div class="header-content">
                            <div class="challenge-meta">
                                <div class="category-name">
                                    <span class="category-label"><asp:Label ID="lblCategoryName" runat="server" /></span>
                                </div>

                                <h1><asp:Label ID="lblChallengeName" runat="server" /></h1>

                                <div class="author-difficulty">
                                    <span class="lecture-id-label">Author: <asp:Label ID="lblname" runat="server" /></span>
                                    <span class="difficulty-label">Difficulty: <span class="difficulty-value"><asp:Label ID="difficultyLabel" runat="server" /></span></span>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="challenge-container">
                        <div class="challenge-description" runat="server" id="challengeDescription" Visible="false">
                            <asp:Literal ID="litDescription" runat="server" />
                        </div>

                        <div class="challenge-file" runat="server" id="challengeFileSection" Visible="false">
                            <div class="file-section">
                                <div class="file-divider"></div>
                                <div class="file-text">File Attached</div>
                                <div class="file-divider"></div>
                            </div>
                            <asp:HyperLink ID="lnkFile" runat="server" CssClass="download-btn" Text="Download" Target="_blank" Download />
                        </div>

                        <div class="submit-section" runat="server" id="submitSection" Visible="false">
                            <div class="flag-container">
                                <i class="fas fa-flag flag-icon"></i>
                                <asp:TextBox ID="flagTextBox" runat="server" placeholder="KIDDIECTF{}" CssClass="flag-input"></asp:TextBox>
                            </div>
                            <asp:Button ID="submitButton" runat="server" Text="Submit Flag" CssClass="submit-btn" OnClick="SubmitButton_Click" />
                        </div>
                    </div>

                    <asp:Label ID="errorMessage" runat="server" CssClass="error" Visible="false"></asp:Label>
                    <asp:Label ID="successMessage" runat="server" CssClass="success" Visible="false"></asp:Label>
                </asp:Panel>
            </div>
        </div>
     </form>
</body>
</html>