<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="WAPP_KiddieCTF.Admin.Dashboard" %>
<%@ Register Src="~/Admin/SideBar.ascx" TagPrefix="uc" TagName="SideBar" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Admin Dashboard</title>

    <!-- Fonts -->
    <link href="https://fonts.googleapis.com/css2?family=Teko:wght@400;500;600&family=Inter:wght@400&display=swap" rel="stylesheet" />

    <!-- Icons -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.1/css/all.min.css" />

    <link href="css/Dashboard.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">

        <!-- Sidebar -->
        <div class="sidebar">
            <uc:SideBar ID="SidebarControl" runat="server" />
        </div>

        <!-- Dashboard Main -->
        <div class="dashboard-main">
            <div class="dashboard-content">

                <!-- Header -->
                <header class="dashboard-header">
                    <h1>Dashboard</h1>
                </header>

                <!-- Top Cards -->
                <section class="top-row">

                    <!-- Students -->
                    <div class="card-rect kpi-card">
                        <div class="kpi-icon">
                            <i class="fas fa-graduation-cap"></i>
                        </div>
                        <div class="kpi-label">STUDENTS</div>
                        <asp:Label ID="lblStudentsCount" runat="server" Text="100" CssClass="kpi-value"></asp:Label>
                    </div>

                    <!-- Lecturer -->
                    <div class="card-rect kpi-card">
                        <div class="kpi-icon">
                            <i class="fas fa-chalkboard-user"></i>
                        </div>
                        <div class="kpi-label">LECTURER</div>
                        <asp:Label ID="lblLecturerCount" runat="server" Text="100" CssClass="kpi-value"></asp:Label>
                    </div>

                    <!-- Latest Intake -->
                    <div class="card-rect intake-card">
                        <h2>Latest Intake</h2>
                        <div class="intake-line"></div>
                        <div class="intake-list">
                            <asp:Repeater ID="rptLatestIntake" runat="server">
                                <ItemTemplate>
                                    <div class="intake-item"><%# Eval("IntakeName") %></div>
                                </ItemTemplate>
                            </asp:Repeater>

                            <!-- Placeholder -->
                            <div class="intake-item placeholder">Intake 1</div>
                            <div class="intake-item placeholder">Intake 2</div>
                            <div class="intake-item placeholder">Intake 3</div>
                            <div class="intake-item placeholder">Intake 4</div>
                            <div class="intake-item placeholder">Intake 5</div>
                        </div>
                    </div>

                </section>

                <!-- Bottom Section -->
                <section class="bottom-row">

                    <!-- Charts -->
                    <div class="charts-stack">
                        <div class="card-rect chart-card">
                            <h2>User Login (Today)</h2>
                            <div class="chart-inner" id="loginChartPlaceholder" runat="server"></div>
                        </div>

                        <div class="card-rect chart-card">
                            <h2>User Logout (Today)</h2>
                            <div class="chart-inner" id="logoutChartPlaceholder" runat="server"></div>
                        </div>
                    </div>

                    <!-- Most Accessed -->
                    <div class="card-rect accessed-card">
                        <div class="accessed-title">
                            <span>Most Accessed</span><br />
                            <span class="sub">(Course/Challenges)</span>
                        </div>

                        <asp:Repeater ID="rptMostAccessed" runat="server">
                            <ItemTemplate>
                                <div class="accessed-item">
                                    <p class="accessed-name"><%# Eval("Name") %></p>
                                    <p class="accessed-detail">Total Student Accessed: <%# Eval("TotalAccessed") %></p>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>

                        <!-- Placeholder -->
                        <div class="accessed-item placeholder">
                            <p class="accessed-name">Course Name 1</p>
                            <p class="accessed-detail">Total Student Accessed: 100</p>
                        </div>
                        <div class="accessed-item placeholder">
                            <p class="accessed-name">Challenge Name 1</p>
                            <p class="accessed-detail">Total Student Accessed: 90</p>
                        </div>
                        <div class="accessed-item placeholder">
                            <p class="accessed-name">Course Name 2</p>
                            <p class="accessed-detail">Total Student Accessed: 50</p>
                        </div>
                        <div class="accessed-item placeholder">
                            <p class="accessed-name">Challenge Name 2</p>
                            <p class="accessed-detail">Total Student Accessed: 30</p>
                        </div>
                    </div>

                </section>

            </div>
        </div>

    </form>
</body>
</html>
