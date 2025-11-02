<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SideBar.ascx.cs" Inherits="WAPP_Assignment.Student.SideBar" %>

<link href="css/sidebar.css" rel="stylesheet" />
<link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0-beta3/css/all.min.css" rel="stylesheet" />
<link href="https://fonts.googleapis.com/css2?family=Teko:wght@400;500;600&display=swap" rel="stylesheet" />

<div class="sidebar">
    <img class="logo" src="/Images/logo.png" alt="Logo" />
    <nav class="nav">
        <a href="Dashboard.aspx" class="nav-item"><span class="icon dashboard"></span><span class="label">DASHBOARD</span></a>
        <a href="Courses.aspx" class="nav-item"><span class="icon courses"></span><span class="label">Courses</span></a>
        <a href="Challenges.aspx" class="nav-item"><span class="icon challenges"></span><span class="label">Challenges</span></a>
        <a href="Tools.aspx" class="nav-item"><span class="icon tools"></span><span class="label">Tools</span></a>
        <a href="Discussions.aspx" class="nav-item"><span class="icon discussions"></span><span class="label">Discussions</span></a>
    </nav>

    <div class="divider"></div>

    <div class="user-profile">
        <a href="profile.aspx" class="avatar">
            <i class="fas fa-user"></i>
        </a>
        <div class="user-info">
            <div class="name"><asp:Label ID="lblName" runat="server" Text=""></asp:Label></div>
            <div class="id"><asp:Label ID="lblID" runat="server" Text=""></asp:Label></div>
        </div>
    </div>

    <a href="/LogOut.aspx" class="logout">
        <i class="fas fa-sign-out-alt logout-icon"></i>
        <span class="label">LOG OUT</span>
    </a>
</div>

<script>
    document.addEventListener('DOMContentLoaded', function () {
        const currentPage = window.location.pathname.split('/').pop().toLowerCase();
        const navLinks = document.querySelectorAll('.nav-item');
        navLinks.forEach(link => {
            if (link.getAttribute('href').toLowerCase().includes(currentPage)) {
                link.classList.add('active');
            } else {
                link.classList.remove('active');
            }
        });
    });
</script>