<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditChallenge.aspx.cs" Inherits="WAPP_KiddieCTF.Admin.InnerFunction.EditChallenge" %>
<%@ Register Src="~/Admin/SideBar.ascx" TagPrefix="uc" TagName="SideBar" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title>Edit Challenge</title>

  <!-- Fonts / Icons -->
  <link href="https://fonts.googleapis.com/css2?family=Teko:wght@400;500;600&display=swap" rel="stylesheet" />
  <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.1/css/all.min.css" />

  <!-- Sidebar + Page CSS -->
  <link href="../css/sidebar.css" rel="stylesheet" />
  <link href="../css/css2/editChallenge.css" rel="stylesheet" />
</head>
<body>
<form id="form1" runat="server">
  <asp:ScriptManager ID="ScriptManager1" runat="server" />

  <div class="page-wrapper">
    <!-- fixed 250px sidebar kept outside the card -->
    <uc:SideBar ID="SidebarControl" runat="server" />

    <!-- main content -->
    <div class="main">
      <div class="page-header">
        <h1 class="page-title">Edit Challenge</h1>
        <div class="pill">Challenge ID: <asp:Label ID="lblChallengeID" runat="server" /></div>
      </div>

      <div class="card">
        <!-- back button -->
        <button type="button" class="btn-back" onclick="window.location.href='../Challenges.aspx'">
          <i class="fa-solid fa-arrow-left"></i> Back
        </button>

        <!-- form grid -->
        <div class="grid">
          <div class="field">
            <label class="label" for="txtName">Challenge Name</label>
            <asp:TextBox ID="txtName" runat="server" CssClass="input" />
          </div>

          <div class="field">
            <label class="label" for="ddlCategory">Category</label>
            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="select" AppendDataBoundItems="true" />
          </div>

          <div class="field">
            <label class="label" for="ddlDifficulty">Difficulty</label>
            <asp:DropDownList ID="ddlDifficulty" runat="server" CssClass="select">
              <asp:ListItem Text="Easy" Value="Easy" />
              <asp:ListItem Text="Medium" Value="Medium" />
              <asp:ListItem Text="Hard" Value="Hard" />
            </asp:DropDownList>
          </div>

          <div class="field">
            <label class="label" for="ddlLecturer">Assigned Lecturer</label>
            <asp:DropDownList ID="ddlLecturer" runat="server" CssClass="select" AppendDataBoundItems="true" />
          </div>

          <div class="field field-span">
            <label class="label" for="txtDescription">Description</label>
            <asp:TextBox ID="txtDescription" runat="server" CssClass="textarea" TextMode="MultiLine" />
          </div>

          <div class="field field-span">
            <label class="label" for="fuAttachment">Attachment (optional)</label>
            <asp:FileUpload ID="fuAttachment" runat="server" CssClass="file" />
            <!-- keep existing file name (if any) -->
            <asp:HiddenField ID="hfExistingFile" runat="server" />
            <asp:Literal ID="litExistingFile" runat="server" />
          </div>
        </div>

        <!-- actions row -->
        <div class="actions">
          <asp:Button ID="btnDelete" runat="server" CssClass="btn btn-muted" Text="DELETE"
                      OnClick="btnDelete_Click"
                      OnClientClick="return confirm('Delete this challenge?');" />
          <span class="flex-spacer"></span>
          <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="DONE" OnClick="btnSave_Click" />
        </div>

        <!-- inline message area (below title, centered like your spec) -->
        <asp:PlaceHolder ID="phMessage" runat="server" Visible="false">
          <div class="notice"><asp:Literal ID="litMessage" runat="server" /></div>
        </asp:PlaceHolder>
      </div>
    </div>
  </div>

  <!-- fix sidebar links for inner folder like your Tools page -->
  <script>
      document.addEventListener("DOMContentLoaded", function () {
          const links = document.querySelectorAll(".sidebar .nav a");
          links.forEach(a => {
              const href = a.getAttribute("href");
              if (href && !href.startsWith("/") && !href.startsWith("http")) {
                  a.setAttribute("href", "../" + href);
              }
          });
      });
  </script>
</form>
</body>
</html>
