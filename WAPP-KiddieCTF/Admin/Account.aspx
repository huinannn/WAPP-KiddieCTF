<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Account.aspx.cs" Inherits="WAPP_KiddieCTF.Admin.Account" %>
<%@ Register Src="~/Admin/SideBar.ascx" TagPrefix="uc" TagName="SideBar" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Kiddie CTF | Accounts</title>
    <link href="css/Account.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0-beta3/css/all.min.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Teko:wght@400;500;600&display=swap" rel="stylesheet" />
</head>

<body>
    <form id="form1" runat="server">
        <div class="sidebar">
            <uc:SideBar ID="SidebarControl" runat="server" />
        </div>

        <div class="main">
            <!-- Header -->
            <div class="header">
                <h1>Accounts</h1>
                <div class="search-container">
                    <asp:TextBox ID="txtSearch" runat="server" CssClass="search-box" placeholder="Search ID"></asp:TextBox>
                    <asp:Button ID="btnFilter" runat="server" CssClass="filter-btn" Text="Search" OnClick="btnFilter_Click" />
                    <asp:Button ID="btnAddStudent" runat="server" CssClass="add-btn" Text="Add New Student" OnClick="btnAddStudent_Click" />
                </div>
            </div>

            <!-- Tabs -->
            <div class="tab-container">
                <asp:Button ID="btnStudentTab" runat="server" CssClass="tab active" Text="Student" OnClick="btnStudentTab_Click" />
                <asp:Button ID="btnLecturerTab" runat="server" CssClass="tab" Text="Lecturer" OnClick="btnLecturerTab_Click" />
                <asp:Button ID="btnIntakeTab" runat="server" CssClass="tab" Text="Intake" OnClick="btnIntakeTab_Click" />
            </div>

            <!-- GridView -->
            <asp:GridView ID="gvAccounts" runat="server" CssClass="account-table" AutoGenerateColumns="false"
                OnRowCommand="gvAccounts_RowCommand">
            </asp:GridView>
        </div>

        <!-- ========== MODAL (Add / Edit) ========== -->
        <asp:Panel ID="pnlModal" runat="server" Visible="false" CssClass="modal-overlay">
            <div class="modal-box">
                <!-- error/info -->
                <asp:Label ID="lblModalMessage" runat="server" CssClass="modal-error" Visible="false"></asp:Label>

                <!-- Add Student Panel -->
                <asp:Panel ID="pnlAddStudent" runat="server" Visible="false" CssClass="modal-section">
                    <h2 class="modal-title">Add New Student</h2>
                    <div class="modal-field">
                        <label for="txtNewStudentID">Student ID</label>
                        <asp:TextBox ID="txtNewStudentID" runat="server" CssClass="modal-input"></asp:TextBox>
                    </div>
                    <div class="modal-field">
                        <label for="txtNewStudentName">Student Name</label>
                        <asp:TextBox ID="txtNewStudentName" runat="server" CssClass="modal-input"></asp:TextBox>
                    </div>
                    <div class="modal-field">
                        <label for="ddlStudentIntake">Intake Code</label>
                        <asp:DropDownList ID="ddlStudentIntake" runat="server" CssClass="modal-input"></asp:DropDownList>
                    </div>
                    <div class="modal-actions">
                        <asp:Button ID="btnSaveStudent" runat="server" Text="Add" CssClass="modal-btn" OnClick="btnSaveStudent_Click" />
                        <asp:Button ID="btnCancelStudent" runat="server" Text="Clear" CssClass="modal-btn cancel" OnClick="btnCancel_Click" CausesValidation="false" />
                    </div>
                </asp:Panel>

                <!-- Add Lecturer Panel -->
                <asp:Panel ID="pnlAddLecturer" runat="server" Visible="false" CssClass="modal-section">
                    <h2 class="modal-title">Add New Lecturer</h2>
                    <div class="modal-field">
                        <label for="txtNewLecturerID">Lecturer ID</label>
                        <asp:TextBox ID="txtNewLecturerID" runat="server" CssClass="modal-input"></asp:TextBox>
                    </div>
                    <div class="modal-field">
                        <label for="txtNewLecturerName">Lecturer Name</label>
                        <asp:TextBox ID="txtNewLecturerName" runat="server" CssClass="modal-input"></asp:TextBox>
                    </div>
                    <div class="modal-actions">
                        <asp:Button ID="btnSaveLecturer" runat="server" Text="Add" CssClass="modal-btn" OnClick="btnSaveLecturer_Click" />
                        <asp:Button ID="btnCancelLecturer" runat="server" Text="Clear" CssClass="modal-btn cancel" OnClick="btnCancel_Click" CausesValidation="false" />
                    </div>
                </asp:Panel>

                <!-- Add Intake Panel -->
                <asp:Panel ID="pnlAddIntake" runat="server" Visible="false" CssClass="modal-section">
                    <h2 class="modal-title">Add New Intake</h2>

                    <div class="modal-field">
                        <label for="txtNewIntakeCode">Intake Code</label>
                        <asp:TextBox ID="txtNewIntakeCode" runat="server" CssClass="modal-input"></asp:TextBox>
                    </div>

                    <div class="modal-field">
                        <label for="txtNewIntakeName">Intake Name</label>
                        <asp:TextBox ID="txtNewIntakeName" runat="server" CssClass="modal-input"></asp:TextBox>
                    </div>

                    <div class="modal-field modal-field-row">
                        <div style="flex:1;">
                            <label for="ddlIntakeMonth">Intake Month</label>
                            <asp:DropDownList ID="ddlIntakeMonth" runat="server" CssClass="modal-input"></asp:DropDownList>
                        </div>
                        <div style="flex:1;">
                            <label for="ddlIntakeYear">Intake Year</label>
                            <asp:DropDownList ID="ddlIntakeYear" runat="server" CssClass="modal-input"></asp:DropDownList>
                        </div>
                    </div>

                    <div class="modal-actions">
                        <asp:Button ID="btnSaveIntake" runat="server" Text="Add" CssClass="modal-btn" OnClick="btnSaveIntake_Click" />
                        <asp:Button ID="btnCancelIntake" runat="server" Text="Clear" CssClass="modal-btn cancel" OnClick="btnCancel_Click" CausesValidation="false" />
                    </div>
                </asp:Panel>
            </div>
        </asp:Panel>

        <!-- ========== SUCCESS POPUP (separate panel) ========== -->
        <asp:Panel ID="pnlSuccess" runat="server" Visible="false" CssClass="success-overlay">
            <div class="success-box">
                <h3 id="lblSuccessTitle" runat="server">Success!</h3>
                <asp:Label ID="lblSuccessMessage" runat="server" Text=""></asp:Label>
                <div class="modal-actions" style="margin-top:20px;">
                    <asp:Button ID="btnSuccessClose" runat="server" Text="Exit" CssClass="modal-btn" OnClick="btnSuccessClose_Click" />
                </div>
            </div>
        </asp:Panel>

    </form>
</body>
</html>
