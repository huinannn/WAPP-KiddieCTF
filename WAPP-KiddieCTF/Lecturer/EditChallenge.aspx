<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditChallenge.aspx.cs" Inherits="WAPP_KiddieCTF.Lecturer.EditChallenge" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Kiddie CTF - Edit Challenge</title>

    <!-- Reusable Sidebar CSS -->
    <link href="css/sidebar.css" rel="stylesheet" runat="server" />

    <!-- Page CSS -->
    <link href="css/editChallenge.css" rel="stylesheet" runat="server" />

    <!-- Font + Alert -->
    <link href="https://fonts.googleapis.com/css2?family=Teko:wght@400;500;600&display=swap" rel="stylesheet"/>
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <!-- SIDEBAR -->
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
            <h1 class="page-title">Edit Challenge</h1>

            <div class="content-panel">
                <button type="button" class="back-btn" onclick="history.back()">
                    <img src="images/back_icon.png" alt="Back" />
                </button>

                <div class="challenge-id">
                    Challenge ID: <asp:Label ID="lblChallengeID" runat="server" Text="CH001"></asp:Label>
                </div>

                <div class="row">
                    <div class="input-group flex-2">
                        <label>Challenge Name</label>
                        <asp:TextBox ID="txtName" runat="server" CssClass="input-box"></asp:TextBox>
                    </div>

                    <div class="input-group flex-1">
                        <label>Category</label>
                        <asp:DropDownList ID="ddlCategory" runat="server" CssClass="input-box"></asp:DropDownList>
                    </div>
                </div>

                <div class="input-group">
                    <label>Challenge Answer</label>
                    <asp:TextBox ID="txtAnswer" runat="server" CssClass="input-box"></asp:TextBox>
                </div>

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
                            <div class="upload-icon"><img src="images/attachFile_icon.png" alt="Upload" /></div>
                            <asp:Label ID="lblFileName" runat="server" CssClass="file-name" Text="No file chosen"></asp:Label>
                        </div>
                    </div>

                    <div class="input-group flex-2">
                        <label>Challenge Description</label>
                        <asp:TextBox ID="txtDescription" runat="server" CssClass="desc-box" TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>

                <div class="action-buttons">
                    <asp:Button ID="btnDelete" runat="server" CssClass="delete-btn" Text="DELETE"
                        OnClick="btnDelete_Click" OnClientClick="return confirm('Are you sure to delete this challenge?');" />
                    <asp:Button ID="btnEdit" runat="server" CssClass="edit-btn" Text="EDIT" OnClick="btnEdit_Click" />
                </div>
            </div>
        </div>

        <script>
            document.querySelector('.file-upload').addEventListener('change', function () {
                const name = this.files[0]?.name || '<%= lblFileName.Text %>';
                document.querySelector('.file-name').textContent = name;
            });
        </script>
    </form>
</body>
</html>

