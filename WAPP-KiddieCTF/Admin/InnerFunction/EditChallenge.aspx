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

    <!-- SweetAlert2 CSS -->
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css" />

    <!-- SweetAlert2 JS -->
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
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
          <i class="fa-solid fa-arrow-left" style="color:#ffffff"></i> Back
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
            <!-- Custom file input label -->
            <label for="fuAttachment" class="file-label">
                <img src="../images/attachFile_icon.png" alt="Attach File" class="file-icon" />
                <span class="file-text">Choose File</span>
            </label>
            <asp:FileUpload ID="fuAttachment" runat="server" CssClass="file" style="display: none;" OnChange="showFileName()" />
            <!-- Display "No File Chosen" if no file is selected -->
            <span id="fileStatus" class="file-status">No file chosen</span>
            <!-- keep existing file name (if any) -->
            <asp:HiddenField ID="hfExistingFile" runat="server" />
            <asp:Literal ID="litExistingFile" runat="server" />
          </div>
        </div>

        <!-- actions row -->
        <div class="actions">
          <asp:Button ID="btnDelete" runat="server" CssClass="btn btn-muted" Text="DELETE"
                      OnClick="btnDelete_Click"
                      OnClientClick="return showDeleteConfirm();" />
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

      // JavaScript to handle file input and file name display
      function showFileName() {
          var fileInput = document.getElementById('<%= fuAttachment.ClientID %>');
          var fileStatus = document.getElementById('fileStatus');

          if (fileInput.files.length > 0) {
              // Show the selected file name
              fileStatus.textContent = fileInput.files[0].name;
              fileStatus.classList.add('file-chosen'); // Apply class to style the file name
          } else {
              // If no file is chosen, show default message
              fileStatus.textContent = 'No file chosen';
              fileStatus.classList.remove('file-chosen');
          }
      }

      // SweetAlert2 delete confirmation
      function showDeleteConfirm() {
          event.preventDefault(); // Prevent default behavior of button click

          Swal.fire({
              title: 'Are you sure?',
              text: 'Do you want to delete this challenge?',
              icon: 'warning',
              showCancelButton: true,
              confirmButtonColor: '#3085d6',
              cancelButtonColor: '#d33',
              confirmButtonText: 'Yes, delete it!',
              cancelButtonText: 'No, cancel'
          }).then((result) => {
              if (result.isConfirmed) {
                  // Proceed with the delete operation by submitting the form
                  __doPostBack('<%= btnDelete.ClientID %>', ''); // Trigger the server-side button click
              }
          });
          return false; // To prevent default form submission
      }
  </script>
</form>
</body>
</html>
