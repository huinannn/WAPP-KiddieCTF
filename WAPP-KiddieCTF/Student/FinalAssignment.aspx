<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FinalAssignment.aspx.cs" Inherits="WAPP_KiddieCTF.Student.FinalAssignment" %>
<%@ Register Src="~/Student/SideBar.ascx" TagPrefix="uc" TagName="SideBar" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Final Assignment</title>
    <link href="css/courses.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Teko:wght@400;500;600&display=swap" rel="stylesheet"/>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0-beta3/css/all.min.css" rel="stylesheet" />
    <link href="~/Images/Logo.png" rel="shortcut icon" type="image/x-icon" />
</head>
<body>
    <form id="form1" runat="server" enctype="multipart/form-data">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        
        <uc:SideBar ID="SidebarControl" runat="server" />
        
        <div class="main-content">
            <div class="courses-title">Final Assignment</div>
            <div class="course-container">

                <a id="backButton" href="#" class="back-button">
                    <i class="fas fa-arrow-left back-icon"></i>
                </a>

                <div class="final-container">
                    <div class="assignment-title">
                        <asp:Label ID="FinalAssignmentLabel" runat="server" />
                    </div>
                    <div class="due-date">
                        <asp:Label ID="DeadlineLabel" runat="server" Text="Due Date: " />
                    </div>

                    <div class="final">
                        <div class="file-download">
                            <p>Assignment File</p>
                            <div class="file-box">
                                <asp:LinkButton ID="FileDownloadLink" runat="server"
                                                CssClass="input-field download-link"
                                                OnClick="DownloadFile"
                                                Visible="false">
                                    <i class="fas fa-download download-icon"></i> <span>Download Assignment</span>
                                </asp:LinkButton>
                            </div>
                        </div>
                       
                        <div class="file-download">
                            <p>Add Submission</p>
                            <div class="file-box">
                                <label for="<%= FileUpload1.ClientID %>" class="file-label" id="fileLabel">
                                    <i class="fas fa-upload upload-icon"></i> No file chosen
                                </label>
                                <asp:FileUpload ID="FileUpload1" runat="server" 
                                              CssClass="file-input" 
                                              onchange="updateFileName()" 
                                              Style="display: none;" />
                            </div>
                        </div>

                        <div class="uploaded-file">
                            <asp:Label ID="UploadedFileLabel" runat="server" />
                        </div>
                       
                        <div class="submit-btn">
                           <asp:Button ID="SubmitAssignmentButton" runat="server" 
                                        Text="Submit" OnClick="SubmitAssignmentButton_Click" 
                                        CssClass="submit-button" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>

    <script>
        function updateFileName() {
            var fileInput = document.getElementById('<%= FileUpload1.ClientID %>');
            var label = document.getElementById('fileLabel');
            if (fileInput.files && fileInput.files[0]) {
                label.innerHTML = '<i class="fas fa-upload upload-icon"></i> ' + fileInput.files[0].name;
            } else {
                label.innerHTML = '<i class="fas fa-upload upload-icon"></i> No file chosen';
            }
        }
    </script>
</body>
</html>