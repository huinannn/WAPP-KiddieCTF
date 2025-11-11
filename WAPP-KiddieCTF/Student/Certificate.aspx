<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Certificate.aspx.cs" Inherits="WAPP_KiddieCTF.Student.Certificate" %>
<%@ Register Src="~/Student/SideBar.ascx" TagPrefix="uc" TagName="SideBar" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Student Certificate</title>
    <link href="css/certificate.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Teko:wght@400;500;600&display=swap" rel="stylesheet"/>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0-beta3/css/all.min.css" rel="stylesheet" />
    <link href="~/Images/Logo.png" rel="shortcut icon" type="image/x-icon" />
</head>
<body>
    <form id="form1" runat="server">
        <uc:SideBar ID="SidebarControl" runat="server" />

        <div class="main-content">
            <div class="certificate-title">My Certificates</div>
            <div class="certificate-container">
                <a href="Dashboard.aspx" class="back-button">
                    <i class="fas fa-arrow-left back-icon"></i>
                </a>

                <asp:Label ID="NoCertificatesLabel" runat="server" Text="You do not have any certificates!" Visible="false" />

                <asp:Repeater ID="rptCertificates" runat="server">
                    <ItemTemplate>
                        <div class="certificate-item">
                            <div class="certificate-info">
                                <div class="certificate-name">
                                    Certificate for Course <%# Eval("Course_ID") %> - <%# Eval("Course_Name") %>
                                </div>
                            </div>

                            <div class="view-button">
                                <a href='<%# GenerateCertificateLink(Eval("Certificate_ID"), Eval("Student_Name"), Eval("Course_Name")) %>' class="view-link" target="_blank">
                                    View Certificate
                                </a>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </form>
</body>
</html>