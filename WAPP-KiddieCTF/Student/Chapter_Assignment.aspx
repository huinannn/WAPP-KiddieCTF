<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Chapter_Assignment.aspx.cs" Inherits="WAPP_KiddieCTF.Student.Chapter_Assignment" Async="true" %>
<%@ Register Src="~/Student/SideBar.ascx" TagPrefix="uc" TagName="SideBar" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Chapter & Final Assignment</title>
    <link href="css/courses.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Teko:wght@400;500;600&display=swap" rel="stylesheet"/>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0-beta3/css/all.min.css" rel="stylesheet" />
    <link href="~/Images/Logo.png" rel="shortcut icon" type="image/x-icon" />
</head>
<body>
    <form id="form1" runat="server">
        <uc:SideBar ID="SidebarControl" runat="server" />

        <div class="main-content">
            <div class="courses-title">
                <asp:Literal ID="coursesTitle" runat="server"></asp:Literal>
            </div>

            <div class="course-container">
                <a href="Courses.aspx" class="back-button">
                    <i class="fas fa-arrow-left back-icon"></i>
                </a>

                <div class="course">
                     <div class="certificate-btn-container">
                         <asp:Button ID="GenerateCertificateButton" runat="server" Text="Certificate"
                             CssClass="certificate-button" OnClick="GenerateCertificateButton_Click" Visible="false" />
                     </div>

                    <div class="container">
                        <div class="dropdown-title" onclick="toggleDropdown('chapterDropdown')">
                            <i id="chapterArrow" class="fas fa-chevron-right"></i> Chapters
                        </div>

                        <div id="chapterDropdown" class="dropdown-content">
                            <asp:Repeater ID="ChaptersRepeater" runat="server">
                                <ItemTemplate>
                                    <div class="chapter-card">
                                        <h4>
                                            <asp:LinkButton ID="btnChapter" runat="server" 
                                                            CommandArgument='<%# Eval("Chapter_File") %>' 
                                                            OnClick="DownloadChapter" 
                                                            style="cursor: pointer; color: inherit; text-decoration: none;">
                                                <%# Eval("Chapter_Name") %>
                                            </asp:LinkButton>
                                        </h4>

                                        <asp:LinkButton ID="btnMarkDone" class="mark-done-btn" runat="server" 
                                                        OnClick="MarkChapterAsDoneButton_Click" 
                                                        CommandArgument='<%# Eval("Chapter_ID") %>'>
                                            Mark as Done
                                        </asp:LinkButton>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>

                            <div class="no-chapter">
                                <h4>
                                    <asp:Literal ID="noChaptersMessage" runat="server" Text="No chapters available for this course!" Visible="false" />
                                </h4>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="container">
                    <div class="dropdown-title" onclick="toggleDropdown('finalAssignmentDropdown')">
                        <i id="finalAssignmentArrow" class="fas fa-chevron-right"></i> Final Assignment
                    </div>

                    <div id="finalAssignmentDropdown" class="dropdown-content">
                        <div class="assignment-card">
                            <h4>
                                <asp:LinkButton ID="FinalAssignmentLink" runat="server" 
                                                CssClass="assignment-link" 
                                                OnClick="RedirectToFinalAssignment">
                                    <%# Eval("FA_Name") %>
                                </asp:LinkButton>
                            </h4>
                        </div>

                        <div id="noFinalAssignmentMessage" runat="server" class="no-assignment" Visible="false">
                            <h4>No final assignment available for this course!</h4>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>

    <script type="text/javascript">
        function toggleDropdown(id) {
            var dropdown = document.getElementById(id);
            var arrow = document.querySelector('#' + id + 'Arrow');

            if (dropdown.style.display === "block") {
                dropdown.style.display = "none";
            } else {
                dropdown.style.display = "block";
            }
        }
    </script>
</body>
</html>