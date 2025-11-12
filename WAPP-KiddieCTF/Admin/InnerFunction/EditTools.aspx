<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditTools.aspx.cs" Inherits="WAPP_KiddieCTF.Admin.InnerFunction.EditTools" %>
<%@ Register Src="~/Admin/SideBar.ascx" TagPrefix="uc" TagName="SideBar" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Edit Tool</title>

    <!-- Shared sidebar + page styles -->
    <link href="../css/sidebar.css" rel="stylesheet" />
    <link href="../css/css2/AddEditTools.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Teko:wght@400;500;600&display=swap" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
</head>
<body>
<form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />

    <!-- fixed sidebar (no wrapper) -->
    <uc:SideBar ID="SidebarControl" runat="server" />

    <!-- main -->
    <div class="main-content edit-tools">
        <h2>Edit Tool</h2>

        <div class="toolbar">
            <button type="button" class="btn-back" onclick="window.location.href='../Tools.aspx'">
                <img src="../../Images/icons/back.png" alt="" />Back
            </button>

            <div class="id-pill">
                Tool ID:&nbsp;<asp:Label ID="lblToolID" runat="server" />
            </div>
        </div>

        <div class="card">
            <div class="grid-2">
                <div class="field">
                    <label class="label">Tool Name</label>
                    <input class="input" runat="server" id="txtToolName" placeholder="Tool Name" required />
                </div>

                <div class="field">
                    <label class="label">Category</label>
                    <asp:DropDownList ID="ddlCategory" CssClass="input" runat="server" AppendDataBoundItems="true" />
                </div>
            </div>

            <div class="field">
                <label class="label">Tool Description (URL / Link)</label>
                <input class="input" runat="server" id="txtToolDescription" placeholder="https://…" required />
            </div>

            <div class="actions">
                <asp:Button ID="btnDelete" runat="server" CssClass="btn danger" Text="DELETE"
                    OnClick="btnDelete_Click"
                    OnClientClick="return confirm('Are you sure you want to delete this tool?');" />

                <span class="spacer"></span>

                <asp:Button ID="btnSubmit" runat="server" CssClass="btn primary" Text="DONE" OnClick="btnSubmit_Click" />
            </div>
        </div>
    </div>
</form>

<script>
    document.addEventListener("DOMContentLoaded", function () {
        // fix sidebar links (this page is /Admin/InnerFunction/*)
        document.querySelectorAll(".sidebar .nav a").forEach(link => {
            const href = link.getAttribute("href");
            if (href && !href.startsWith("/") && !href.startsWith("http")) {
                link.setAttribute("href", "../" + href);
            }
        });

        // keep Tools highlighted
        const toolsLink = document.querySelector('.sidebar .nav a[href*="Tools.aspx"]');
        if (toolsLink) {
            document.querySelectorAll('.sidebar .nav a').forEach(a => a.classList.remove('active'));
            toolsLink.classList.add('active');
        }
    });
</script>
</body>
</html>
