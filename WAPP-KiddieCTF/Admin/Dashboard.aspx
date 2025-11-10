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

    <!-- Chart.js (needed for the charts we build in code-behind) -->
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>

    <link href="css/Dashboard.css" rel="stylesheet" />
    <link href="~/Images/Logo.png" rel="shortcut icon" type="image/x-icon" />
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
                                    <div class="intake-item">
                                        <%# (Container.ItemIndex + 1).ToString() + ". " + Eval("Intake_Name") %>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>

                </section>

                <!-- Bottom Section -->
                <section class="bottom-row">

                    <!-- Charts -->
                    <div class="charts-stack">

                        <!-- Year dropdown (your code-behind binds this) -->
                        <div style="margin-bottom:6px;">
                            <asp:Label ID="lblYear" runat="server" Text="Select Year:" Style="margin-right:6px;"></asp:Label>
                            <asp:DropDownList ID="ddlYear" runat="server" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlYear_SelectedIndexChanged"
                                Style="padding:3px 6px; font-size:12px; border-radius:5px;background-color: #374151;color:#fff;border:none;">
                            </asp:DropDownList>
                        </div>

                        <div class="card-rect chart-card">
                            <h2>User Login (by Month)</h2>
                            <div class="chart-inner">
                                <canvas id="loginChart" width="1000" height="200"></canvas>
                            </div>

                        </div>

                        <div class="card-rect chart-card">
                            <h2>User Logout (by Month)</h2>
                            <div class="chart-inner">
                                <canvas id="logoutChart" width="1000" height="200"></canvas>
                            </div>

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
                                    <p class="accessed-detail">Category: <%# Eval("Category") %></p>
                                    <p class="accessed-detail">Total Student Accessed: <%# Eval("TotalAccessed") %></p>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>

                </section>

                <!-- this will get the <script> injected from code-behind -->
                <asp:Literal ID="litChartData" runat="server" EnableViewState="false"></asp:Literal>

            </div>
        </div>

    </form>
</body>
</html>
