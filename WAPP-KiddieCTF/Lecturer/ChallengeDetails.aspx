<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChallengeDetails.aspx.cs" Inherits="WAPP_KiddieCTF.Lecturer.ChallengeDetails" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Kiddie CTF - Challenge Details</title>

    <!-- Reusable Sidebar CSS -->
    <link href="css/sidebar.css" rel="stylesheet" runat="server" />

    <!-- Challenges Details Page CSS -->
    <link href="css/challengeDetails.css" rel="stylesheet" runat="server" />

    <!-- Google Font: Teko -->
    <link href="https://fonts.googleapis.com/css2?family=Teko:wght@400;500;600&display=swap" rel="stylesheet"/>

</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <!-- SIDEBAR (unchanged) -->
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
                <div class="avatar"><img src="images/profile.png" alt="Profile" /></div>
                <div class="user-info">
                    <div class="name"><asp:Label ID="lblLecturerName" runat="server" /></div>
                    <div class="id"><asp:Label ID="lblLecturerID" runat="server" /></div>
                </div>
            </div>

            <a href="../LogOut.aspx" class="logout">
                <img src="images/logout.png" alt="Logout" class="logout-img" />
                <span class="label">LOG OUT</span>
            </a>
        </div>

        <!-- MAIN CONTENT -->
        <div class="main">
            <h1 class="page-title">Challenge Details</h1>

            <!-- === BIG PANEL === -->
            <div class="outer-panel">
                <div class="panel-header">
                    <a href="javascript:history.back()" class="back-btn">
                        <img src="images/back_icon.png" alt="Back" />
                    </a>

                    <asp:Button ID="btnEdit" runat="server" Text="EDIT"
                        CssClass="edit-btn" OnClick="btnEdit_Click" CausesValidation="false" />
                </div>
                <div class="category">
                    <asp:Label ID="lblCategory" runat="server" Text="OSINT"></asp:Label>
                </div>

                <div class="challenge-name">
                    <asp:Label ID="lblChallengeName" runat="server" Text="Challenge Name"></asp:Label>
                </div>

                <div class="difficulty">
                    <span>Difficulty:</span>
                    <span id="difficultyBox" runat="server" class="difficulty-box">HARD</span>
                </div>

                <div class="inner-panel">
                    <asp:Label ID="lblDescription" runat="server"
                        Text="Reception of Special has been cool to say the least. That's why we made an exclusive version of Special..."
                        CssClass="desc-text"></asp:Label>

                    <div class="file-line left"></div>
                    <span class="files-text">Files Attached</span>
                    <div class="file-line right"></div>

                    <div class="file-btn">
                        <asp:Label ID="lblFileName" runat="server" Text="osint.zip"></asp:Label>
                    </div>
                </div>
            </div>
        </div>

    </form>
</body>
</html>
