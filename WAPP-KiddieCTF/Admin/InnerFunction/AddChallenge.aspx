<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddChallenge.aspx.cs" Inherits="WAPP_KiddieCTF.Admin.InnerFunction.AddChallenge" %>
<%@ Register Src="~/Admin/SideBar.ascx" TagPrefix="uc" TagName="SideBar" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Add New Challenge</title>

    <link href="../css/sidebar.css" rel="stylesheet" />
    <link href="../css/css2/AddChallenge.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Teko:wght@400;500;600&display=swap" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
</head>
<body>
<form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />

    <!-- Admin Sidebar Control -->
    <uc:SideBar ID="SidebarControl" runat="server" />

    <div class="main-content edit-challenge">
        <h2>Add New Challenge</h2>

        <div class="toolbar">
            <button type="button" class="btn-back" onclick="window.location.href='../Challenges.aspx'">
                <img src="../../Images/icons/back.png" alt="" />Back
            </button>

            <div class="id-pill">
                Challenge ID:&nbsp;<asp:Label ID="lblChallengeID" runat="server" />
            </div>
        </div>

        <div class="card">
            <div class="grid-2">
                <div class="field">
                    <label class="label">Challenge Name</label>
                    <asp:TextBox runat="server" id="txtName" CssClass="input" placeholder="Challenge Name" required />
                </div>

                <div class="field">
                    <label class="label">Category</label>
                    <asp:DropDownList ID="ddlCategory" CssClass="input" runat="server" AppendDataBoundItems="true" />
                </div>
            </div>

            <div class="field">
                <label class="label">Challenge Flag</label>
                <asp:TextBox runat="server" id="txtFlag" CssClass="input" placeholder="Flag" required />
            </div>

            <div class="field">
                <label class="label">Challenge Description</label>
                <asp:TextBox runat="server" id="txtDescription" CssClass="input" placeholder="Challenge Description" required />
            </div>

            <div class="field">
                <label class="label">Difficulty</label>
                <asp:DropDownList ID="ddlDifficulty" CssClass="input" runat="server">
                    <asp:ListItem Text="- Select -" Value=""></asp:ListItem>
                    <asp:ListItem Text="Easy" Value="Easy"></asp:ListItem>
                    <asp:ListItem Text="Medium" Value="Medium"></asp:ListItem>
                    <asp:ListItem Text="Hard" Value="Hard"></asp:ListItem>
                </asp:DropDownList>
            </div>

            <div class="field">
                <label class="label">Lecturer</label>
                <asp:DropDownList ID="ddlLecturer" CssClass="input" runat="server">
                    <asp:ListItem Text="- Select Lecturer -" Value=""></asp:ListItem>
                </asp:DropDownList>
            </div>

            <div class="field">
                <label class="label">Upload File</label>
                <asp:FileUpload ID="fuFile" runat="server" CssClass="file-upload" />
            </div>

            <div class="actions">
                <span class="spacer"></span>
                <asp:Button ID="btnSubmit" runat="server" CssClass="btn primary" Text="DONE" OnClick="btnDone_Click" />
            </div>
        </div>
    </div>
</form>

<script>
    document.addEventListener("DOMContentLoaded", function () {
        document.querySelectorAll(".sidebar .nav a").forEach(link => {
            const href = link.getAttribute("href");
            if (href && !href.startsWith("/") && !href.startsWith("http")) {
                link.setAttribute("href", "../" + href);
            }
        });

        const challengesLink = document.querySelector('.sidebar .nav a[href*="Challenges.aspx"]');
        if (challengesLink) {
            document.querySelectorAll('.sidebar .nav a').forEach(a => a.classList.remove('active'));
            challengesLink.classList.add('active');
        }
    });
</script>
</body>
</html>
