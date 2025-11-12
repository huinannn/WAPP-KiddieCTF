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
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <!-- Admin Sidebar Control -->
        <uc:SideBar ID="SidebarControl" runat="server" />

        <!-- Main Content -->
        <div class="main">
            <h1 class="page-title">Add New Challenge</h1>

            <div class="content-panel">
                <!-- Back Button -->
                <button type="button" class="back-btn" onclick="window.location.href='../Challenges.aspx'">
                    <img src="../images/back_icon.png" alt="Back" />
                </button>

                <!-- Challenge ID -->
                <div class="challenge-id">
                    Challenge ID: <asp:Label ID="lblChallengeID" runat="server" Text=""></asp:Label>
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

                <!-- Assigned Lecturer -->
                <div class="input-group">
                    <label>Assigned Lecturer</label>
                    <asp:DropDownList ID="ddlLecturer" runat="server" CssClass="input-box">
                        <asp:ListItem Text="- Select Lecturer -" Value=""></asp:ListItem>
                    </asp:DropDownList>
                </div>

                <!-- Done Button (centered) -->
                <div class="btn-center">
                    <asp:Button ID="btnDone" runat="server" CssClass="done-btn" Text="DONE" OnClick="btnDone_Click" />
                </div>
            </div>
        </div>

        <script>
            // Update filename when file chosen
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
</body>
</html>

<style>
/* === Base Setup === */
body {
    background: #111827;
    margin: 0;
    font-family: 'Teko', sans-serif;
}

/* === Main Layout === */
/* === Adjust Main Content Area === */
.main {
    margin-left: 250px; /* Set margin to the width of the sidebar */
    width: calc(100% - 250px); /* Ensure the main content adjusts to sidebar width */
    min-height: 100vh;
    padding: 40px 50px;
    box-sizing: border-box;
    background: #111827;
    overflow-x: hidden; /* Prevent horizontal scrolling */
}

/* === Page Title === */
.page-title {
    font-size: 50px;
    font-weight: 600;
    letter-spacing: 2.5px;
    color: #fff;
    margin-bottom: 20px;
    align-self: flex-start;
    margin-left: 40px;
}

/* === Content Panel === */
.content-panel {
    width: 1060px;
    min-height: 766px;
    background: #1B263B;
    border-radius: 20px;
    padding: 40px;
    position: relative;
}

/* === Back Button === */
.back-btn {
    position: absolute;
    top: 25px;
    left: 25px;
    background: none;
    border: none;
    cursor: pointer;
}

    .back-btn img {
        width: 47px;
        height: 47px;
    }

/* === Challenge ID === */
.challenge-id {
    color: white;
    font-size: 30px;
    font-weight: 600;
    letter-spacing: 1.5px;
    text-align: center;
    margin-bottom: 30px;
    margin-top: 20px;
}

/* === Input Groups === */
.input-group {
    display: flex;
    flex-direction: column;
    margin-bottom: 25px;
}

    .input-group label {
        color: white;
        font-size: 20px;
        font-weight: 600;
        letter-spacing: 1px;
        margin-bottom: 8px;
    }

.input-box, .desc-box {
    background: rgba(69, 80, 101, 0.7);
    border: none;
    border-radius: 10px;
    color: #9BA0A6;
    font-family: 'Teko', sans-serif;
    font-size: 18px;
    font-weight: 600;
    letter-spacing: 0.9px;
    padding: 10px 20px;
    height: 60px;
    outline: none;
    resize: none;
}

.desc-box {
    height: 180px;
}

/* === Row Layout === */
.row {
    display: flex;
    justify-content: space-between;
    align-items: stretch;
    width: 100%;
    gap: 25px;
}

.flex-1 {
    flex: 1;
    display: flex;
    flex-direction: column;
    justify-content: space-between;
}

    .flex-1 .input-group:first-child {
        margin-bottom: 0; /* no big gap */
    }

.flex-2 {
    flex: 2;
}

/* === File Upload === */
.file-upload-box {
    position: relative;
    background: rgba(69, 80, 101, 0.7);
    border-radius: 10px;
    height: 60px;
    display: flex;
    align-items: center;
    padding: 0 20px;
    margin-top: 5px;
}

.file-upload {
    opacity: 0;
    position: absolute;
    width: 100%;
    height: 100%;
    cursor: pointer;
}

.upload-icon {
    position: absolute;
    right: 20px;
}

    .upload-icon img {
        width: 36px;
        height: 36px;
    }

.file-name {
    color: #9BA0A6;
    font-size: 18px;
    font-weight: 600;
    letter-spacing: 0.9px;
}

/* === Done Button === */
.btn-center {
    display: flex;
    justify-content: center;
    margin-top: 20px;
}

.done-btn {
    width: 134px;
    height: 55px;
    background: #9BA0A6;
    border: 2px solid #9BA0A6;
    border-radius: 10px;
    color: #1B263B;
    font-size: 32px;
    font-weight: 600;
    letter-spacing: 1.6px;
    cursor: pointer;
    font-family: 'Teko', sans-serif;
    transition: all 0.25s ease;
}

    .done-btn:hover {
        background: transparent;
        color: #9BA0A6;
        border: 4px solid #9BA0A6;
        transform: translateY(-2px);
    }
</style>
