<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChallengeDetails.aspx.cs" Inherits="WAPP_KiddieCTF.Admin.InnerFunction.ChallengeDetails" %>
<%@ Register Src="~/Admin/SideBar.ascx" TagPrefix="uc" TagName="SideBar" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Kiddie CTF - Challenge Details (Admin)</title>

    <!-- we are in /Admin/InnerFunction so go up one level -->
    <link href="../css/sidebar.css" rel="stylesheet" />
    <link href="../css/css2/challengeDetails.css" rel="stylesheet" />

    <link href="https://fonts.googleapis.com/css2?family=Teko:wght@400;500;600&display=swap" rel="stylesheet"/>

    <style>
        body {
            margin: 0;
            background: #000;
            font-family: 'Teko', sans-serif;
        }

        /* main sits next to the fixed 250px sidebar */
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

        /* in case your existing css doesn't have outer-panel margin */
        .outer-panel {
            background: #1B263B;
            border-radius: 20px;
            padding: 30px 35px;
        }

        .panel-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
        }

        .back-btn img {
            width: 30px;
            height: 30px;
            /* lecturer version used a non-rotated icon; keep it straight */
        }

        .edit-btn {
            background: #9BA0A6;
            color: #1B263B;
            border: none;
            border-radius: 10px;
            padding: 6px 25px;
            font-size: 20px;
            font-weight: 600;
            cursor: pointer;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <!-- sidebar user control -->
        <uc:SideBar ID="SidebarControl" runat="server" />

        <!-- MAIN CONTENT -->
        <div class="main">
            <h1 class="page-title">Challenge Details</h1>

            <div class="outer-panel">
                <div class="panel-header">
                    <!-- go back to admin Challenges -->
                    <a href="../Challenges.aspx" class="back-btn">
                        <img src="../images/back_icon.png" alt="Back" />
                    </a>

                    <asp:Button ID="btnEdit" runat="server" Text="EDIT"
                        CssClass="edit-btn" OnClick="btnEdit_Click" CausesValidation="false" />
                </div>

                <div class="category" style="margin-top:20px;">
                    <asp:Label ID="lblCategory" runat="server" Text=""></asp:Label>
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
                        Text=""
                        CssClass="desc-text"></asp:Label>

                    <div class="file-line left"></div>
                    <span class="files-text">Files Attached</span>
                    <div class="file-line right"></div>

                    <div class="file-btn">
                        <asp:Label ID="lblFileName" runat="server" Text=""></asp:Label>
                    </div>
                </div>
            </div>
        </div>
    </form>
    <script>
    document.addEventListener("DOMContentLoaded", function () {
        // we are in /Admin/InnerFunction/*
        // 1) fix sidebar nav links so they point back to /Admin/*.aspx
        const navLinks = document.querySelectorAll(".sidebar .nav a");

        navLinks.forEach(function (link) {
            const href = link.getAttribute("href");
            if (!href) return;

            // skip absolute paths or full urls
            if (href.startsWith("/") || href.startsWith("http")) return;

            // if it already starts with ../ then it's already fixed
            if (href.startsWith("../")) return;

            // otherwise, prepend ../
            link.setAttribute("href", "../" + href);
        });

        // 2) force "Challenges" to be active on challenge inner pages
        //    (addchallenge, editchallenge, challengedetails, etc.)
        const path = window.location.pathname.toLowerCase();
        const isChallengeInner =
            path.includes("addchallenge") ||
            path.includes("editchallenge") ||
            path.includes("challengedetails");

        if (isChallengeInner) {
            // remove active from all first
            document.querySelectorAll(".sidebar .nav a").forEach(a => a.classList.remove("active"));

            // find the Challenges link — after we rewrote it above it should contain "Challenges.aspx"
            const challengesLink = Array.from(document.querySelectorAll(".sidebar .nav a"))
                .find(a => (a.getAttribute("href") || "").toLowerCase().includes("challenges.aspx"));

            if (challengesLink) {
                challengesLink.classList.add("active");
            }
        }
    });
    </script>

</body>
</html>
