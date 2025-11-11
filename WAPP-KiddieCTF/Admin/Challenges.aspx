<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Challenges.aspx.cs" Inherits="WAPP_KiddieCTF.Admin.Challenges" %>
<%@ Register Src="~/Admin/SideBar.ascx" TagPrefix="uc" TagName="SideBar" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Kiddie CTF - Challenges (Admin)</title>

    <!-- we are inside /Admin/, so css is just /Admin/css/... -->
    <link href="css/sidebar.css" rel="stylesheet" />
    <link href="css/css2/challenges.css" rel="stylesheet" />

    <link href="https://fonts.googleapis.com/css2?family=Teko:wght@400;500;600&display=swap" rel="stylesheet" />
    <style>
        body {
            margin: 0;
            background: #000;
            font-family: 'Teko', sans-serif;
        }

        /* main sits to the right of fixed 250px sidebar */
        .main {
            margin-left: 250px;
            width: calc(100% - 250px);
            min-height: 100vh;
            padding: 40px 50px;
            box-sizing: border-box;
            background: #000;
        }

        .page-title {
            font-size: 50px;
            font-weight: 600;
            letter-spacing: 2.5px;
            color: #fff;
            margin-bottom: 25px;
        }

        /* ensure toolbar doesn't hide behind sidebar on small screens */
        .toolbar {
            display: flex;
            gap: 12px;
            align-items: center;
            flex-wrap: wrap;
        }

        .add-box {
            margin-left: auto;
            cursor: pointer;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <!-- your shared admin sidebar -->
        <uc:SideBar ID="SidebarControl" runat="server" />

        <!-- MAIN CONTENT -->
        <div class="main">
            <h1 class="page-title">Challenges</h1>

            <!-- Toolbar with Category Tabs -->
            <div class="toolbar">
                <asp:LinkButton ID="lnkAll" runat="server" CssClass="tab-btn active" OnClick="Filter_Click" CommandArgument="">All</asp:LinkButton>
                <asp:LinkButton ID="lnkOSINT" runat="server" CssClass="tab-btn" OnClick="Filter_Click" CommandArgument="CT001">OSINT</asp:LinkButton>
                <asp:LinkButton ID="lnkCrypto" runat="server" CssClass="tab-btn" OnClick="Filter_Click" CommandArgument="CT002">Cryptography</asp:LinkButton>
                <asp:LinkButton ID="lnkStegano" runat="server" CssClass="tab-btn" OnClick="Filter_Click" CommandArgument="CT003">Steganography</asp:LinkButton>
                <asp:LinkButton ID="lnkReverse" runat="server" CssClass="tab-btn" OnClick="Filter_Click" CommandArgument="CT004">Reverse Engineering</asp:LinkButton>

                <!-- go to innerfunction add challenge (if that's where you put it) -->
                <div class="add-box" onclick="location.href='InnerFunction/AddChallenge.aspx'">
                    <img src="images/add_icon.png" alt="" />
                    <span>Add New Challenge</span>
                </div>
            </div>

            <!-- Challenge Grid -->
            <asp:UpdatePanel ID="UpdatePanelChallenges" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="content-panel">
                        <div class="challenge-grid">
                            <asp:Repeater ID="ChallengeRepeater" runat="server">
                                <ItemTemplate>
                                    <div class="challenge-card"
                                         onclick="location.href='InnerFunction/ChallengeDetails.aspx?id=<%# Eval("Challenge_ID") %>'"
                                         style="cursor:pointer;">

                                        <div class="top-info">
                                            <div class="category"><%# GetCategoryName(Eval("Category_ID")) %></div>
                                            <h3 class="challenge-name"><%# Eval("Challenge_Name") %></h3>
                                        </div>

                                        <div class="bottom-info">
                                            <div class="difficulty <%# GetDifficultyClass(Eval("Challenge_Difficulty")) %>">
                                                <%# Eval("Challenge_Difficulty").ToString().ToUpper() %>
                                            </div>

                                            <!-- Edit Button -->
                                            <asp:LinkButton ID="lnkEdit" runat="server"
                                                CssClass="edit-btn"
                                                CommandArgument='<%# Eval("Challenge_ID") %>'
                                                OnClick="lnkEdit_Click"
                                                OnClientClick="event.stopPropagation();">
                                                <img src="images/edit_icon.png" alt="Edit" class="edit-icon" />
                                                EDIT
                                            </asp:LinkButton>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
