<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChallengeDetails.aspx.cs" Inherits="WAPP_KiddieCTF.Admin.InnerFunction.ChallengeDetails" %>
<%@ Register Src="~/Admin/SideBar.ascx" TagPrefix="uc" TagName="SideBar" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Kiddie CTF - Challenge Details</title>

    <link href="../css/sidebar.css" rel="stylesheet" />
    <link href="../css/css2/challengeDetails.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Teko:wght@400;500;600&family=Inter:wght@400&display=swap" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="sidebar">
            <uc:SideBar ID="SidebarControl" runat="server" />
        </div>

        <div class="main-content">
            <div class="content-header">
                <h1 class="page-title">Challenge Details</h1>
            </div>

            <div class="details-container">
                <div class="details-card">
                    <div class="panel-header">
                        <a href="../Challenges.aspx" class="back-btn">
                            <img src="../images/back_icon.png" alt="Back" />
                        </a>

                        <asp:Button ID="btnEdit" runat="server" Text="EDIT" CssClass="edit-btn" OnClick="btnEdit_Click" CausesValidation="false" />
                    </div>

                    <div class="details-grid">
                        <div class="detail-item">
                            <label>Category:</label>
                            <asp:Label ID="lblCategory" runat="server" Text="Cryptography"></asp:Label>
                        </div>

                        <div class="detail-item">
                            <label>Challenge Name:</label>
                            <asp:Label ID="lblChallengeName" runat="server" Text="Winie The Pooo"></asp:Label>
                        </div>

                        <div class="detail-item">
                            <label>Difficulty:</label>
                            <span id="difficultyBox" runat="server" class="difficulty-box">HARD</span>
                        </div>

                        <div class="detail-item">
                            <label>Lecturer ID:</label>
                            <asp:Label ID="lblLecturerID" runat="server" Text="L001"></asp:Label>
                        </div>
                    </div>

                    <div class="inner-panel">
                        <asp:Label ID="lblDescription" runat="server" Text="Poooo" CssClass="desc-text"></asp:Label>
                    </div>

                    <div class="files-section">
                        <div class="file-line"></div>
                        
                        <span class="files-text" style="margin-top: 20px; padding-top:20px;">Files Attached</span>
                        <div class="file-btn">
                            <asp:Label ID="lblFileName" runat="server" Text="CH001_Certificate_Ali (2).pdf"></asp:Label>
                        </div>
                        <div class="file-line"></div>
                    </div>
                </div>
            </div>
        </div>
    </form>

    <script>
        document.addEventListener("DOMContentLoaded", function () {
            const navLinks = document.querySelectorAll(".sidebar .nav a");

            navLinks.forEach(function (link) {
                const href = link.getAttribute("href");
                if (!href) return;
                if (href.startsWith("/") || href.startsWith("http")) return;
                if (href.startsWith("../")) return;

                link.setAttribute("href", "../" + href);
            });

            const path = window.location.pathname.toLowerCase();
            const isChallengeInner =
                path.includes("addchallenge") ||
                path.includes("editchallenge") ||
                path.includes("challengedetails");

            if (isChallengeInner) {
                document.querySelectorAll(".sidebar .nav a").forEach(a => a.classList.remove("active"));
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
