<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Challenges.aspx.cs" Inherits="WAPP_KiddieCTF.Lecturer.Challenges" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Kiddie CTF - Challenges</title>

    <!-- Reusable Sidebar CSS -->
    <link href="css/sidebar.css" rel="stylesheet" runat="server" />

    <!-- Challenges Page CSS -->
    <link href="css/challenges.css" rel="stylesheet" runat="server" />

    <!-- Google Font: Teko -->
    <link href="https://fonts.googleapis.com/css2?family=Teko:wght@400;500;600&display=swap" rel="stylesheet"/>

</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <!-- === SIDEBAR === -->
        <div class="sidebar">
            <img class="logo" src="images/logo.png" alt="Logo" />
            <nav class="nav">
                <a href="Dashboard.aspx" class="nav-item"><span class="icon dashboard"></span><span class="label">DASHBOARD</span></a>
                <a href="Courses.aspx" class="nav-item"><span class="icon courses"></span><span class="label">Courses</span></a>
                <a href="Challenges.aspx" class="nav-item active"><span class="icon challenges"></span><span class="label">Challenges</span></a>
                <a href="Tools.aspx" class="nav-item"><span class="icon tools"></span><span class="label">Tools</span></a>
            </nav>
            <div class="divider"></div>
            <div class="user-profile">
                    <div class="avatar" onclick="window.location='Profile.aspx'" style="cursor:pointer;">
                        <img src="images/profile.png" alt="Profile" />
                    </div>
                    <div class="user-info">
                        <div class="name">
                            <asp:Label ID="lblLecturerName" runat="server" Text=""></asp:Label>
                        </div>
                        <div class="id">
                            <asp:Label ID="lblLecturerID" runat="server" Text=""></asp:Label>
                        </div>
                    </div>
                </div>
                <a href="../LogOut.aspx" class="logout">
                    <img src="images/logout.png" alt="Logout" class="logout-img" />
                    <span class="label">LOG OUT</span>
                </a>
        </div>

        <!-- === MAIN CONTENT === -->
        <div class="main">
            <h1 class="page-title">Challenges</h1>

            <!-- Toolbar with Category Tabs -->
            <div class="toolbar">
                <asp:LinkButton ID="lnkAll" runat="server" CssClass="tab-btn active" OnClick="Filter_Click" CommandArgument="">All</asp:LinkButton>
                <asp:LinkButton ID="lnkOSINT" runat="server" CssClass="tab-btn" OnClick="Filter_Click" CommandArgument="CT001">OSINT</asp:LinkButton>
                <asp:LinkButton ID="lnkCrypto" runat="server" CssClass="tab-btn" OnClick="Filter_Click" CommandArgument="CT002">Cryptography</asp:LinkButton>
                <asp:LinkButton ID="lnkStegano" runat="server" CssClass="tab-btn" OnClick="Filter_Click" CommandArgument="CT003">Steganography</asp:LinkButton>
                <asp:LinkButton ID="lnkReverse" runat="server" CssClass="tab-btn" OnClick="Filter_Click" CommandArgument="CT004">Reverse Engineering</asp:LinkButton>
                <div class="add-box" onclick="location.href='AddChallenge.aspx'" style="cursor:pointer;">
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
                                    <div class="challenge-card" onclick="location.href='ChallengeDetails.aspx?id=<%# Eval("Challenge_ID") %>'" style="cursor:pointer;">

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
                                                OnClick="lnkEdit_Click">
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
