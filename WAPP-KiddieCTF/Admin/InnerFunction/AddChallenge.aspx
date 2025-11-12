<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddChallenge.aspx.cs" Inherits="WAPP_KiddieCTF.Admin.InnerFunction.AddChallenge" %>
<%@ Register Src="~/Admin/SideBar.ascx" TagPrefix="uc" TagName="SideBar" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Kiddie CTF - Add New Challenge (Admin)</title>

    <!-- we are in /Admin/InnerFunction so go up one level for css/images -->
    <link href="../css/sidebar.css" rel="stylesheet" />
    <link href="../css/css2/addChallenge.css" rel="stylesheet" />

    <link href="https://fonts.googleapis.com/css2?family=Teko:wght@400;500;600&display=swap" rel="stylesheet"/>
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>

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

        /* just in case your addChallenge.css doesn't set panel bg */
        .content-panel {
            background: #1B263B;
            border-radius: 20px;
            padding: 35px;
        }

        .back-btn img {
            width: 30px;
            height: 30px;
            
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <!-- admin sidebar control -->
        <uc:SideBar ID="SidebarControl" runat="server" />

        <!-- MAIN CONTENT -->
        <div class="main">
            <h1 class="page-title">Add New Challenge</h1>

            <div class="content-panel">
                <!-- Back button: go back to admin Challenges -->
                <button type="button" class="back-btn" onclick="window.location.href='../Challenges.aspx'">
                    <img src="../images/back_icon.png" alt="Back" />
                </button>

                <!-- Challenge ID -->
                <div class="challenge-id">
                    Challenge ID:
                    <asp:Label ID="lblChallengeID" runat="server" Text=""></asp:Label>
                </div>

                <!-- Challenge Name + Category -->
                <div class="row">
                    <div class="input-group flex-2">
                        <label>Challenge Name</label>
                        <asp:TextBox ID="txtName" runat="server" CssClass="input-box"></asp:TextBox>
                    </div>

                    <div class="input-group flex-1">
                        <label>Category</label>
                        <asp:DropDownList ID="ddlCategory" runat="server" CssClass="input-box">
                            <asp:ListItem Text="- Select -" Value=""></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>

                <!-- Challenge Flag -->
                <div class="input-group">
                    <label>Challenge Flag</label>
                    <asp:TextBox ID="txtFlag" runat="server" CssClass="input-box"></asp:TextBox>
                </div>

                <!-- Difficulty + Description -->
                <div class="row">
                    <div class="input-group flex-1">
                        <label>Difficulty</label>
                        <asp:DropDownList ID="ddlDifficulty" runat="server" CssClass="input-box">
                            <asp:ListItem Text="- Select -" Value=""></asp:ListItem>
                            <asp:ListItem Text="Easy" Value="Easy"></asp:ListItem>
                            <asp:ListItem Text="Medium" Value="Medium"></asp:ListItem>
                            <asp:ListItem Text="Hard" Value="Hard"></asp:ListItem>
                        </asp:DropDownList>

                        <label>File Attached</label>
                        <div class="file-upload-box">
                            <asp:FileUpload ID="fuFile" runat="server" CssClass="file-upload" />
                            <div class="upload-icon"><img src="../images/attachFile_icon.png" alt="Upload" /></div>
                            <asp:Label ID="lblFileName" runat="server" CssClass="file-name" Text="No file chosen"></asp:Label>
                        </div>
                    </div>

                    <div class="input-group flex-2">
                        <label>Challenge Description</label>
                        <asp:TextBox ID="txtDescription" runat="server" CssClass="desc-box" TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>

                <!-- Done Button (centered) -->
                <div class="btn-center">
                    <asp:Button ID="btnDone" runat="server" CssClass="done-btn" Text="DONE" OnClick="btnDone_Click" />
                </div>
            </div>
        </div>

        <script>
            // update filename when file chosen
            document.addEventListener("DOMContentLoaded", function () {
                var fu = document.querySelector('.file-upload');
                var lbl = document.querySelector('.file-name');
                if (fu && lbl) {
                    fu.addEventListener('change', function () {
                        const name = this.files[0]?.name || 'No file chosen';
                        lbl.textContent = name;
                    });
                }
            });
        </script>
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
