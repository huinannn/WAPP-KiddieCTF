<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditTools.aspx.cs" Inherits="WAPP_KiddieCTF.Admin.InnerFunction.EditTools" %>
<%@ Register Src="~/Admin/SideBar.ascx" TagPrefix="uc" TagName="SideBar" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Edit Tools</title>

    <!-- page-specific css -->
    <link href="../css/tools.css" rel="stylesheet" />

    <!-- load sidebar css from ONE LEVEL UP so your control can use it -->
    <link href="../css/sidebar.css" rel="stylesheet" />

    <!-- other css -->
    <link href="../css/css2/addNewCourse.css" rel="stylesheet" runat="server" />
    <link href="https://fonts.googleapis.com/css2?family=Teko:wght@400;500;600&display=swap" rel="stylesheet"/>
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>

    <style>
        body {
            margin: 0;
            background: #1B263B;
            font-family: 'Teko', sans-serif;
        }

        /* push content beside fixed 250px sidebar */
        .main-content {
            margin-left: 250px;
            padding: 40px;
            min-height: 100vh;
            background: #1B263B;
        }

        .back {
            display: flex;
            flex-direction: row;
            width: 100%;
            align-items: center;
            gap: 10px;
            cursor: pointer;
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
            color: #fff;
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
            color: #fff;
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
            cursor: pointer;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <!-- your original sidebar control, untouched -->
        <uc:SideBar ID="SidebarControl" runat="server" />

        <div class="main-content">
            <h2 style="color: white; margin-bottom: 20px;">Edit Tool</h2>

            <div class="row" style="padding: 10px;">
                <div class="back" onclick="window.location.href='../Tools.aspx'">
                    <img src="../../Images/icons/back.png" alt="Back" />
                    <span style="color:white;">Back</span>
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
                        <p class="label">Tool Description (URL / Link)</p>
                        <input class="input" name="tool-description" placeholder="Tool Description" required="required" runat="server" id="txtToolDescription" />
                    </div>
                </div>
                <div class="btn">
                    <div class="spacer"></div>
                    <asp:Button ID="btnDelete" runat="server" CssClass="done" Text="DELETE"
                        OnClick="btnDelete_Click"
                        OnClientClick="return confirm('Are you sure you want to delete this tool?');" />
                    <asp:Button ID="btnSubmit" runat="server" CssClass="done" Text="DONE" OnClick="btnSubmit_Click" />
                    <div class="spacer"></div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
