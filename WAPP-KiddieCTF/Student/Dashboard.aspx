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
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <uc:SideBar ID="SidebarControl" runat="server" />

        <div class="main-content">
            <div class="dashboard-title">Dashboard</div>

            <div class="dashboard-container">

                <div class="recent-courses-container">
                    <div class="section-title-container">
                        <div class="section-title">Recent Accessed Courses/Challenges</div>
                        <div class="arrows-container">
                            <div class="arrow-left" onclick="scrollCourses('left')">&#10094;</div>
                            <div class="arrow-right" onclick="scrollCourses('right')">&#10095;</div>
                        </div>
                    </div>

                    <div class="courses-container" id="recentScroll">
                        <div class="courses">
                            <asp:Repeater ID="rptRecent" runat="server">
                                <ItemTemplate>
                                    <div class="course-card">
                                        <i class='fas <%# Eval("ItemType").ToString() == "COURSE" ? "fa-book" : "fa-flag" %> icon'></i>
                                        <div class="course-name"><%# Eval("ItemName") %></div>
                                        <div class="lecturer-id">
                                            Lecturer ID: <%# Eval("LecturerID") %>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </div>

                <div class="upcoming-tasks-container">
                    <div class="section-title">Upcoming Deadlines</div>
                    <div class="tasks-box">
                        <div class="task-card">
                            <asp:Repeater ID="rptDeadlines" runat="server">
                                <ItemTemplate>
                                    <div class="task-title">Assignment Submission Deadlines</div>
                                    <div class="task-row">
                                        <div class="task-subtitle"><%# Eval("Course_Name") %></div>
                                        <div class="task-date"><%# Eval("FormattedDate") %></div>
                                    </div>
                                    <asp:Panel runat="server" Visible='<%# Container.ItemIndex != ((Repeater)Container.Parent).Items.Count - 1 %>'>
                                        <div class="divider"></div>
                                    </asp:Panel>
                                </ItemTemplate>
                            </asp:Repeater>

                            <asp:Panel ID="pnlNoDeadlines" runat="server" Visible="false">
                                <div class="task-title">No Upcoming Deadlines</div>
                                <div class="task-row">
                                    <div class="task-subtitle">You're all caught up!</div>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>

            </div>

            <div class="total-info-container">
                <div class="total-info">

                    <div class="info-card">
                        <i class="fas fa-book icon"></i>
                        <div class="info-number">
                            <asp:Label ID="lblCompletedFA" runat="server" Text="0"></asp:Label>
                        </div>
                        <div class="info-label">Total Completed Final Assignment</div>
                    </div>

                    <div class="info-card" onclick="goToCertificatePage()">
                        <i class="fas fa-certificate icon"></i>
                        <div class="info-number">
                            <asp:Label ID="lblCertificates" runat="server" Text="0"></asp:Label>
                        </div>
                        <div class="info-label">Total Certificates</div>
                    </div>

                    <div class="info-card">
                        <div class="chart-container">
                            <canvas id="challengeChart"></canvas>
                            <div class="chart-label">
                                <asp:Label ID="lblChallengeProgress" runat="server" Text="Completed 0/0 Challenges"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <asp:HiddenField ID="hiddenSolved" runat="server" />
        <asp:HiddenField ID="hiddenTotal" runat="server" />
    </form>

    <script>
        function scrollCourses(direction) {
            const wrapper = document.getElementById('recentScroll');
            if (!wrapper) return;
            const scrollAmount = 290;

            if (direction === 'right') {
                if (wrapper.scrollLeft + wrapper.clientWidth < wrapper.scrollWidth) {
                    wrapper.scrollLeft += scrollAmount;
                }
            } else {
                if (wrapper.scrollLeft > 0) {
                    wrapper.scrollLeft -= scrollAmount;
                }
            }
        }

        function goToCertificatePage() {
             var studentId = '<%= Session["StudentID"] %>';
             window.location.href = 'Certificate.aspx?studentId=' + studentId;
        }

        document.addEventListener('DOMContentLoaded', function () {
            var solved = parseInt('<%= hiddenSolved.Value %>') || 0;
            var total = parseInt('<%= hiddenTotal.Value %>') || 0;
            var remaining = total - solved;

            var ctx = document.getElementById('challengeChart').getContext('2d');
            if (ctx) {
                var challengeChart = new Chart(ctx, {
                    type: 'doughnut',
                    data: {
                        labels: ['Completed', 'Remaining'],
                        datasets: [{
                            data: [solved, remaining],
                            backgroundColor: [
                                '#4FC3F7',
                                '#2D3748'
                            ],
                            borderColor: [
                                '#4FC3F7', 
                                '#2D3748'
                            ],
                            borderWidth: 2,
                            cutout: '78%'
                        }]
                    },
                    options: {
                        responsive: true,
                        maintainAspectRatio: false,
                        plugins: {
                            legend: {
                                display: false
                            },
                            tooltip: {
                                callbacks: {
                                    label: function (context) {
                                        var label = context.label || '';
                                        var value = context.raw || 0;
                                        return label + ': ' + value;
                                    }
                                }
                            }
                        },
                        animation: {
                            animateScale: true,
                            animateRotate: true,
                            duration: 1000
                        }
                    }
                });

                var label = document.getElementById('<%= lblChallengeProgress.ClientID %>');
                if (label) {
                    label.textContent = "Completed " + solved + "/" + total + " Challenges";
                }
            }
        });
    </script>
</body>
</html>