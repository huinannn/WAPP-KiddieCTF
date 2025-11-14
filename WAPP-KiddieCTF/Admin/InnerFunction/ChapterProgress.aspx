<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChapterProgress.aspx.cs" Inherits="WAPP_KiddieCTF.Admin.InnerFunction.ChapterProgress" %>
<%@ Register Src="~/Admin/SideBar.ascx" TagPrefix="uc" TagName="SideBar" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Kiddie CTF - Chapter Progress</title>

    <!-- Reusable Sidebar CSS (Admin) -->
    <link href="../css/sidebar.css" rel="stylesheet" runat="server" />

    <!-- Chapter Progress Page CSS (Admin) -->
    <link href="../css/css2/chapterProgress.css" rel="stylesheet" runat="server" />

    <!-- Google Font: Teko -->
    <link href="https://fonts.googleapis.com/css2?family=Teko:wght@400;500;600&display=swap" rel="stylesheet" />

    <!-- Alert -->
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <!-- SIDEBAR (Admin Version with UC) -->
        <div class="sidebar">
            <uc:SideBar ID="SidebarControl" runat="server" />
        </div>

        <!-- MAIN -->
        <div class="main">
            <h1 class="page-title">Chapter Progress</h1>
            <div class="content-wrapper">

                <!-- TOOLBAR -->
                <div class="toolbar">
                    <button type="button" class="back-btn" onclick="history.back()">
                        <img src="../images/back_icon2.png" alt="Back" />
                    </button>

                    <asp:UpdatePanel ID="UpdatePanelSearch" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="search-box">
                                <img src="../images/search.png" alt="" />
                                <asp:TextBox ID="txtSearch" runat="server"
                                             CssClass="search-input"
                                             AutoPostBack="true"
                                             OnTextChanged="txtSearch_TextChanged">
                                </asp:TextBox>
                                <label class="placeholder-label">Search Student ID / Student Name</label>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="txtSearch" EventName="TextChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>

                <!-- TABLE PANEL -->
                <asp:UpdatePanel ID="UpdatePanelProgress" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="content-panel">
                            <div class="table-header">
                                <div class="col-id">Student ID</div>
                                <div class="col-name">Student Name</div>
                                <div class="col-progress">Total Chapters Completed</div>
                            </div>

                            <div class="table-body">
                                <asp:Repeater ID="rptProgress" runat="server">
                                    <ItemTemplate>
                                        <div class="table-row">
                                            <div class="col-id"><%# Eval("Student_ID") %></div>
                                            <div class="col-name"><%# Eval("Student_Name") %></div>
                                            <div class="col-progress"><%# Eval("Completed") %></div>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>

                                <asp:Literal ID="litNoData" runat="server"
                                             Text="<div class='no-data'>No students enrolled or no progress recorded.</div>"
                                             Visible="false"></asp:Literal>
                            </div>
                        </div>
                    </ContentTemplate>

                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="txtSearch" EventName="TextChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </form>

    <script type="text/javascript">
        // ====== Search placeholder handling ======
        function updatePlaceholder() {
            var txt = document.getElementById('<%= txtSearch.ClientID %>');
            var label = txt ? txt.parentNode.querySelector('.placeholder-label') : null;
            if (!txt || !label) return;

            if (txt.value.trim() === "") {
                label.style.opacity = '1';
                label.style.transform = 'translateY(-50%)';
                label.style.top = '50%';
                label.style.fontSize = '18px';
            } else {
                label.style.opacity = '0';
                label.style.transform = 'translateY(-50%) scale(0.8)';
                label.style.top = '10px';
                label.style.fontSize = '14px';
            }
        }

        function initPlaceholderEvents() {
            updatePlaceholder();

            var txt = document.getElementById('<%= txtSearch.ClientID %>');
            if (txt) {
                txt.addEventListener('input', updatePlaceholder);
                txt.addEventListener('keydown', function (e) {
                    if (e.key === 'Enter') {
                        setTimeout(updatePlaceholder, 200);
                    }
                });
            }
        }

        // ====== Sidebar link fix for /Admin/InnerFunction/* + active "Courses" ======
        function fixAdminInnerSidebar() {
            const navLinks = document.querySelectorAll(".sidebar .nav a");
            if (!navLinks || navLinks.length === 0) return;

            // 1) Fix href to point back to /Admin/*.aspx from /Admin/InnerFunction/*
            navLinks.forEach(function (link) {
                const href = link.getAttribute("href");
                if (!href) return;

                // skip absolute paths or full urls
                if (href.startsWith("/") || href.startsWith("http")) return;

                // already "../"
                if (href.startsWith("../")) return;

                link.setAttribute("href", "../" + href);
            });

            // 2) Force "Courses" active on chapter-related inner pages
            const path = window.location.pathname.toLowerCase();
            const isCoursesInner =
                path.includes("chapterprogress") ||
                path.includes("addnewcourse") ||
                path.includes("editcourse") ||
                path.includes("studentlist") ||
                path.includes("addstudent");

            if (isCoursesInner) {
                navLinks.forEach(function (link) {
                    link.classList.remove("active");
                });

                const coursesLink = Array.from(navLinks).find(function (link) {
                    const href = (link.getAttribute("href") || "").toLowerCase();
                    return href.indexOf("courses.aspx") !== -1;
                });

                if (coursesLink) {
                    coursesLink.classList.add("active");
                }
            }
        }

        // Run once when page first loads
        document.addEventListener("DOMContentLoaded", function () {
            initPlaceholderEvents();
            fixAdminInnerSidebar();
        });

        // Run again after every partial postback (UpdatePanel)
        Sys.Application.add_load(initPlaceholderEvents);
    </script>

</body>
</html>
