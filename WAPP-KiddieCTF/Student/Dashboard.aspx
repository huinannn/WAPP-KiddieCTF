<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="WAPP_KiddieCTF.Student.Dashboard" %>
<%@ Register Src="~/Student/SideBar.ascx" TagPrefix="uc" TagName="SideBar" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Student Dashboard</title>
    <link href="css/dashboard.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Teko:wght@400;500;600&display=swap" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0-beta3/css/all.min.css" rel="stylesheet" />
    <link href="~/Images/Logo.png" rel="shortcut icon" type="image/x-icon" />
</head>
<body>
    <form id="form1" runat="server">
        <uc:SideBar ID="SidebarControl" runat="server" />

        <div class="main-content">
            <div class="dashboard-title">Dashboard</div>

            <!-- TOP ROW -->
            <div class="dashboard-container">

                <!-- LEFT: RECENTLY ACCESSED -->
                <div class="recent-courses-container">
                    <div class="section-title-container">
                        <div class="section-title">Recently Accessed Courses/Challenges</div>
                        <div class="arrows-container">
                            <div class="arrow-left" onclick="scrollCourses('left')">&#10094;</div>
                            <div class="arrow-right" onclick="scrollCourses('right')">&#10095;</div>
                        </div>
                    </div>

                    <!-- scrolling wrapper (we scroll this) -->
                    <div class="courses-container" id="recentScroll">
                        <!-- flex row (repeater will output cards here) -->
                        <div class="courses">
                            <asp:Repeater ID="rptRecent" runat="server">
                                <ItemTemplate>
                                    <div class="course-card">
                                        <!-- icon: book for course, flag for challenge -->
                                        <i class='fas <%# Eval("ItemType").ToString() == "COURSE" ? "fa-book" : "fa-flag" %> icon'></i>
                                        <div class="course-name"><%# Eval("ItemName") %></div>

                                        <!-- 👇 show Lecturer ID for both Course and Challenge -->
                                        <div class="lecturer-id">
                                            Lecturer ID: <%# Eval("LecturerID") %>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>

                        </div>
                    </div>
                </div>

                <!-- RIGHT: UPCOMING EVENTS -->
                <div class="upcoming-tasks-container">
                    <div class="section-title">Upcoming Events</div>
                    <div class="tasks-box">
                        <div class="task-card">
                            <div class="task-title">Assignment Submission Deadlines</div>
                            <div class="task-row">
                                <div class="task-subtitle">Cybersecurity Fundamentals</div>
                                <div class="task-date">1 Nov</div>
                            </div>
                            <div class="divider"></div>
                            <div class="task-title">Assignment Submission Deadlines</div>
                            <div class="task-row">
                                <div class="task-subtitle">System Administration</div>
                                <div class="task-date">2 Nov</div>
                            </div>
                            <div class="divider"></div>
                            <div class="task-title">Assignment Submission Deadlines</div>
                            <div class="task-row">
                                <div class="task-subtitle">Networking</div>
                                <div class="task-date">5 Nov</div>
                            </div>
                        </div>
                    </div>
                </div>

            </div>

            <!-- BOTTOM: INFO CARDS -->
            <div class="total-info-container">
                <div class="total-info">

                    <!-- Total Completed Courses (actually: total final assignments done) -->
                    <div class="info-card">
                        <i class="fas fa-book icon"></i>
                        <div class="info-number">
                            <asp:Label ID="lblCompletedFA" runat="server" Text="0"></asp:Label>
                        </div>
                        <div class="info-label">Total Completed Final Assessment</div>
                    </div>

                    <!-- Total Certificates -->
                    <div class="info-card">
                        <i class="fas fa-certificate icon"></i>
                        <div class="info-number">
                            <asp:Label ID="lblCertificates" runat="server" Text="0"></asp:Label>
                        </div>
                        <div class="info-label">Total Certificates</div>
                    </div>

                    <!-- Challenge progress -->
                    <div class="info-card">
                        <div class="chart-container">
                            <svg width="100" height="100" viewBox="0 0 100 100">
                                <!-- you can keep this static for now -->
                                <circle cx="50" cy="50" r="45" stroke="#4FC3F7" stroke-width="5" fill="none"></circle>
                                <circle cx="50" cy="50" r="45" stroke="#9BA0A6" stroke-width="5" fill="none" stroke-dasharray="283" stroke-dashoffset="141.5"></circle>
                            </svg>
                            <div class="chart-label">
                                <asp:Label ID="lblChallengeProgress" runat="server" Text="Completed 0/0 Challenges"></asp:Label>
                            </div>
                        </div>
                    </div>


                </div>
            </div>


        </div>
    </form>

    <script>
        // simple horizontal scroll by card width
        function scrollCourses(direction) {
            const wrapper = document.getElementById('recentScroll');
            if (!wrapper) return;
            const scrollAmount = 290; // card width + gap
            if (direction === 'right') {
                wrapper.scrollLeft += scrollAmount;
            } else {
                wrapper.scrollLeft -= scrollAmount;
            }
        }
    </script>
</body>
</html>
