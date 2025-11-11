<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditAssignment.aspx.cs" Inherits="WAPP_KiddieCTF.Admin.InnerFunction.EditAssignment" %>
<%@ Register Src="~/Admin/SideBar.ascx" TagPrefix="uc" TagName="SideBar" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Kiddie CTF - Edit Final Assignment (Admin)</title>

    <!-- we are in /Admin/InnerFunction, so go up one level -->
    <link href="../css/sidebar.css" rel="stylesheet" />
    <link href="../css/css2/editAssignment.css" rel="stylesheet" />

    <link href="https://fonts.googleapis.com/css2?family=Teko:wght@400;500;600&display=swap" rel="stylesheet"/>
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>

    <style>
        body {
            margin: 0;
            background: #000;
            font-family: 'Teko', sans-serif;
        }

        /* keep content to the right of the fixed 250px sidebar */
        .main {
            margin-left: 250px;
            width: calc(100% - 250px);
            padding: 40px 50px;
            min-height: 100vh;
            background: #000;
            box-sizing: border-box;
        }

        .page-title {
            font-size: 50px;
            font-weight: 600;
            letter-spacing: 2.5px;
            color: #fff;
            margin-bottom: 30px;
        }

        .content-panel {
            background: #1B263B;
            border-radius: 20px;
            padding: 40px;
        }

        /* fix back button direction */
        .back-btn img {
            width: 30px;
            height: 30px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <!-- shared admin sidebar -->
        <uc:SideBar ID="SidebarControl" runat="server" />

        <!-- MAIN -->
        <div class="main">
            <h1 class="page-title">Edit Final Assignment</h1>

            <div class="content-panel">
                <div class="form-container">
                    <!-- go back to course details -->
                    <button type="button" class="back-btn"
                            onclick="window.location.href='../CourseDetails.aspx?id=<%= Request.QueryString["courseid"] %>'">
                        <img src="../images/back_icon.png" alt="Back" />
                    </button>

                    <div class="assignment-id-label">
                        Assignment ID: <asp:Label ID="lblFAID" runat="server" Text=""></asp:Label>
                    </div>

                    <div class="input-group">
                        <label>Assignment Name</label>
                        <div class="input-box">
                            <asp:TextBox ID="txtFAName" runat="server" CssClass="input-field" placeholder=" "></asp:TextBox>
                            <label class="placeholder">Enter assignment name</label>
                        </div>
                    </div>

                    <div class="row">
                        <div class="input-group half">
                            <label>File Attached</label>
                            <div class="file-upload-box">
                                <asp:FileUpload ID="fuFile" runat="server" CssClass="file-upload" />
                                <div class="upload-icon">
                                    <img src="../images/attachFile_icon.png" alt="Upload" />
                                </div>
                                <asp:Label ID="lblFileName" runat="server" CssClass="file-name" Text=""></asp:Label>
                            </div>
                        </div>

                        <div class="input-group half">
                            <label>Deadline</label>
                            <div class="input-box">
                                <asp:TextBox ID="txtDeadline" runat="server" CssClass="input-field" TextMode="Date"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <div class="action-buttons">
                        <asp:Button ID="btnDelete" runat="server" CssClass="delete-btn" Text="DELETE"
                                    OnClick="btnDelete_Click"
                                    OnClientClick="return confirm('Delete this assignment?');" />
                        <asp:Button ID="btnEdit" runat="server" CssClass="edit-btn" Text="EDIT"
                                    OnClick="btnEdit_Click" />
                    </div>
                </div>
            </div>
        </div>
    </form>

    <script>
        // update label when file chosen
        document.addEventListener("DOMContentLoaded", function () {
            var fu = document.querySelector('.file-upload');
            var nameLabel = document.querySelector('.file-name');
            if (fu && nameLabel) {
                fu.addEventListener('change', function () {
                    const name = this.files[0] ? this.files[0].name : nameLabel.textContent;
                    nameLabel.textContent = name;
                });
            }
        });
    </script>

</body>
</html>
