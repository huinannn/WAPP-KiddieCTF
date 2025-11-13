<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddChapter.aspx.cs" Inherits="WAPP_KiddieCTF.Lecturer.AddChapter" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Kiddie CTF - Add Chapter</title>

    <!-- Reusable Sidebar CSS -->
    <link href="css/sidebar.css" rel="stylesheet" runat="server" />

    <!-- Add Chapter Page CSS -->
    <link href="css/addChapter.css" rel="stylesheet" runat="server" />

    <!-- Google Font: Teko -->
    <link href="https://fonts.googleapis.com/css2?family=Teko:wght@400;500;600&display=swap" rel="stylesheet"/>

    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>

</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <!-- === SIDEBAR (Same as others) === -->
        <div class="sidebar">
            <img class="logo" src="images/logo.png" alt="Logo" />
            <nav class="nav">
                <a href="Dashboard.aspx" class="nav-item"><span class="icon dashboard"></span><span class="label">DASHBOARD</span></a>
                <a href="Courses.aspx" class="nav-item active"><span class="icon courses"></span><span class="label">Courses</span></a>
                <a href="Challenges.aspx" class="nav-item"><span class="icon challenges"></span><span class="label">Challenges</span></a>
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
            <h1 class="page-title">Add Chapter</h1>

            <!-- BIG PANEL -->
            <div class="content-panel">
                <div class="form-container">
                    <!-- Back Button -->
                    <button type="button" class="back-btn" 
                            onclick="window.location.replace('CourseDetails.aspx?id=<%= Request.QueryString["courseid"] %>');">
                        <img src="images/back_icon.png" alt="Back" />
                    </button>

                    <!-- Chapter ID (Auto) -->
                    <div class="chapter-id-label">
                        Chapter ID: <asp:Label ID="lblChapterID" runat="server" Text="CP001"></asp:Label>
                    </div>

                    <!-- Chapter Name -->
                    <div class="input-group">
                        <label>Chapter Name</label>
                        <div class="input-box">
                            <asp:TextBox ID="txtChapterName" runat="server" CssClass="input-field" placeholder=" "></asp:TextBox>
                            <label class="placeholder">Enter chapter name</label>
                        </div>
                    </div>

                    <!-- File Upload -->
                    <div class="input-group">
                        <label>File Attached</label>
                        <div class="file-upload-box">
                            <asp:FileUpload ID="fuChapterFile" runat="server" CssClass="file-upload" />
                            <div class="upload-icon">
                                <img src="images/attachFile_icon.png" alt="Upload" />
                            </div>
                            <asp:Label ID="lblFileName" runat="server" CssClass="file-name" Text="No file chosen"></asp:Label>
                        </div>
                    </div>

                    <!-- DONE Button -->
                    <asp:Button ID="btnDone" runat="server" CssClass="done-btn" Text="DONE" OnClick="btnDone_Click" />
                </div>
            </div>
        </div>
    </form>

    <!-- File Name Update -->
    <script>
        document.querySelector('.file-upload').addEventListener('change', function () {
            const fileName = this.files[0]?.name || 'No file chosen';
            document.querySelector('.file-name').textContent = fileName;
        });
    </script>

</body>
</html>
