<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditTools.aspx.cs" Inherits="WAPP_KiddieCTF.Lecturer.EditTools" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <title>Edit Tools</title>
     <link href="css/tools.css" rel="stylesheet" />
     <link href="~/Images/Logo.png" rel="shortcut icon" type="image/x-icon" />
     <!-- Reusable Sidebar CSS -->
     <link href="css/sidebar.css" rel="stylesheet" runat="server" />

     <!-- Add New Course Page CSS -->
     <link href="css/addNewCourse.css" rel="stylesheet" runat="server" />

     <!-- Google Font: Teko -->
     <link href="https://fonts.googleapis.com/css2?family=Teko:wght@400;500;600&display=swap" rel="stylesheet"/>

     <!-- Alert -->
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <style>
        .back {
            display: flex;
            flex-direction: row;
            width: 100%;
        }

        .back img {
            width: 30px;
            height: 30px;
            transform: rotate(180deg);
        }

        .row {
            display: block;
        }

        .title {
            text-align: center;
            font-size: 30px;
            font-weight: 500;
            letter-spacing: 1px;
            margin: 50px auto;
        }

        .first-row {
            display: flex;
            flex-direction: row;
            gap: 20px;
            margin: 10px 20px;
        }

        .second-row {
            margin: 10px 20px;
        }

        .each-input {
            width: 100%;
            margin-bottom: 10px;
        }

        .each-input .label {
            margin-left: 10px;
        }

        .each-input .input {
            width: 100%;
            background-color: #455066;
            opacity: .7;
            border-radius: 10px;
            padding: 10px 20px;
            color: white;
            border: none;
            outline: none;
        }

        .each-input .input:focus {
            outline: none;
        }

        .btn {
            width: 100%;
            display: flex;
            flex-direction: row;
            gap: 20px;
        }

        .btn .done {
            background-color: #9BA0A6;
            color: #1B263B;
            padding: 5px 10px;
            text-align: center;
            font-size: 25px;
            font-weight: 600;
            border-radius: 10px;
            border: none;
            margin: 50px auto;
            min-width: 130px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <!-- === SIDEBAR (Same as others) === -->
        <div class="sidebar">
            <img class="logo" src="images/logo.png" alt="Logo" />
            <nav class="nav">
                <a href="Dashboard.aspx" class="nav-item"><span class="icon dashboard"></span><span class="label">DASHBOARD</span></a>
                <a href="Courses.aspx" class="nav-item"><span class="icon courses"></span><span class="label">Courses</span></a>
                <a href="Challenges.aspx" class="nav-item"><span class="icon challenges"></span><span class="label">Challenges</span></a>
                <a href="Tools.aspx" class="nav-item active"><span class="icon tools"></span><span class="label">Tools</span></a>
                <a href="Dashboard.aspx" class="nav-item"><span class="icon dashboard"></span><span class="label">DASHBOARD</span></a>
            </nav>
            <div class="divider"></div>
            <div class="user-profile">
                    <div class="avatar" onclick="window.location='Profile.aspx'" style="cursor:pointer;">
                        <img src="images/profile.png" alt="Profile" />
                    </div>
                <div class="user-info">
                    <div class="name">Wong Xin Yee</div>
                    <div class="id">LC123456</div>
                </div>
            </div>
            <a href="Login.aspx" class="logout"><span class="icon logout-icon"></span><span class="label">LOG OUT</span></a>
        </div>

        <div class="main-content" style="margin-left: 250px; padding: 40px;">
            <h2 style="color: white; margin-bottom: 20px;">Edit Tool</h2>

            <div class="row" style="padding: 10px;">
                <div class="back" style="cursor:pointer" onclick="window.location.href='Tools.aspx'">
                    <img src="../Images/icons/back.png" />
                    <div class="spacer"></div>
                </div>
                <div class="title">
                    <h3>Tool ID: <asp:Label ID="lblToolID" runat="server" Text=""></asp:Label></h3>
                </div>
                <div class="first-row">
                    <div class="each-input" style="flex: 1 1 70%;">
                        <p class="label">Tool Name</p>
                        <input class="input" name="tool-name" placeholder="Tool Name" required="required" runat="server" id="txtToolName" />
                    </div>
                    <div class="each-input" style="flex: 1 1 30%;">
                        <p class="label">Category</p>
                        <asp:DropDownList ID="ddlCategory" CssClass="input" runat="server" AppendDataBoundItems="true">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="second-row">
                    <div class="each-input">
                        <p class="label">Tool Description</p>
                        <input class="input" name="tool-description" placeholder="Tool Description" required="required" runat="server" id="txtToolDescription" />
                    </div>
                </div>
                <div class="btn">
                    <div class="spacer"></div>
                    <asp:Button ID="btnDelete" runat="server" CssClass="done" Text="DELETE" OnClick="btnDelete_Click" />
                    <asp:Button ID="btnSubmit" runat="server" CssClass="done" Text="DONE" OnClick="btnSubmit_Click" />
                    <div class="spacer"></div>
                </div>
            </div>
        </div>
    </form>
    <script>
        document.addEventListener('DOMContentLoaded', function() {
            const deleteBtn = document.querySelector('#<%= btnDelete.ClientID %>');

            deleteBtn.addEventListener('click', function(e) {
                e.preventDefault(); // prevent default postback

                Swal.fire({
                    title: 'Are you sure?',
                    text: "This tool will be permanently deleted!",
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonColor: '#3085d6',
                    cancelButtonColor: '#d33',
                    confirmButtonText: 'Yes, delete it!',
                    cancelButtonText: 'Cancel'
                }).then((result) => {
                    if (result.isConfirmed) {
                        __doPostBack('<%= btnDelete.UniqueID %>', '');
                    }
                });
            });
        });
    </script>
</body>
</html>
