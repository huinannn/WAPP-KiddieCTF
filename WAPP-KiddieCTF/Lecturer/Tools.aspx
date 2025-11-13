<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tools.aspx.cs" Inherits="WAPP_KiddieCTF.Lecturer.Tools" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Lecturer Tools</title>
    <link href="css/tools.css" rel="stylesheet" />
    <link href="~/Images/Logo.png" rel="shortcut icon" type="image/x-icon" />
    <!-- Reusable Sidebar CSS -->
    <link href="css/sidebar.css" rel="stylesheet" runat="server" />

    <!-- Add New Course Page CSS -->
    <link href="css/addNewCourse.css" rel="stylesheet" runat="server" />

    <!-- Google Font: Teko -->
    <link href="https://fonts.googleapis.com/css2?family=Teko:wght@400;500;600&display=swap" rel="stylesheet"/>

    <!-- Alert -->
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <style>
        a {
            text-decoration: none;
        }

        a:hover, a:focus, a:visited {
            text-decoration: none;
        }

        .mb-4 {
            display: flex;
            flex-direction: row;
        }

        .category-btn {
            background-color: #455066;
            opacity: .7;
            border: none;
            color: #9BA0A6;
            margin: 5px;
            margin-bottom: 13px;
            min-width: 73px;
            padding: 8px 16px;
            border-radius: 8px;
            transition: 0.3s;
            font-size: 18px;
            font-weight: 500;
            cursor: pointer;
        }

        .category-btn.active, .category-btn:hover {
            background-color: #133B5C;
            color: white;
        }

        .tool-card {
            background-color: #455066;
            opacity: .7;
            width: 250px;
            height: 173px;
            padding: 20px 5px 20px 20px;
            border-radius: 10px;
            transition: 0.3s ease;
        }

        .tool-card:hover {
            box-shadow: 0 4px 20px rgba(0, 0, 0, 0.7);
            transform: translateY(-3px);
        }

        .tool-card small {
            font-size: 16px;
            font-weight: 500;
            color: #9BA0A6;
            margin-bottom: 10px;
        }

        .tool-card h5 {
            font-size: 24px;
            color: white;
            margin-bottom: 10px;
        }

        .tool-card p {
            color: white;
            font-size: 15px;
            width: 100%;
            word-wrap: anywhere;
        }

        .row {
            display: flex;
            flex-wrap: wrap;
            align-items: flex-start;
            justify-content: center;
            gap: 20px;
            background-color: #1B263B;
            border-radius: 20px;
            padding: 30px 40px;
        }

        .category-edit {
            display: flex;
            flex-direction: row;
            justify-content: space-between;
            align-items: center;
        }


        .create-btn {
            background-color: #455066;
            opacity: .7;
            border: none;
            color: white;
            margin: 5px;
            margin-bottom: 13px;
            min-width: 73px;
            padding: 8px 16px;
            border-radius: 8px;
            transition: 0.3s;
            font-size: 18px;
            font-weight: 500;
            display: flex;
            flex-direction: row;
            gap: 0 10px;
            align-items: center;
            cursor: pointer;
        }

        .create-btn img {
            width: 20px;
            height: 20px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <!-- === SIDEBAR (Same as others) === -->
        <div class="sidebar">
            <img class="logo" src="images/logo.png" alt="Logo" />
            <nav class="nav">
                <a href="Dashboard.aspx" class="nav-item"><span class="icon dashboard"></span><span class="label">DASHBOARD</span></a>
                <a href="Courses.aspx" class="nav-item"><span class="icon courses"></span><span class="label">Courses</span></a>
                <a href="Challenges.aspx" class="nav-item "><span class="icon challenges"></span><span class="label">Challenges</span></a>
                <a href="Tools.aspx" class="nav-item active"><span class="icon tools"></span><span class="label">Tools</span></a>
            </nav>
            <div class="divider"></div>
            <div class="user-profile">
                    <div class="avatar" onclick="window.location='Profile.aspx'" style="cursor:pointer;">
                        <img src="images/profile.png" alt="Profile" />
                    </div>
                <div class="user-info">
                    <div class="name">
                        <asp:Label ID="lblLecturerName" runat="server" Text=""></asp:Label>
                    </div>
                    <div class="id">
                        <asp:Label ID="lblLecturerID" runat="server" Text=""></asp:Label>
                    </div>
                </div>
            </div>
            <a href="../LogOut.aspx" class="logout">
                <img src="images/logout.png" alt="Logout" class="logout-img" />
                <span class="label">LOG OUT</span>
            </a>
        </div>
        <div class="main-content" style="margin-left: 250px; padding: 40px;">
            <h2 style="color: white;">Tools</h2>

            <div class="mb-4" style="margin-top: 20px;">
                <button type="button" class="category-btn active" style="font-weight: bold;">All</button>
                <button type="button" class="category-btn">OSINT</button>
                <button type="button" class="category-btn">CRYPTOGRAPHY</button>
                <button type="button" class="category-btn">STEGANOGRAPHY</button>
                <button type="button" class="category-btn">REVERSE ENGINEERING</button>
                <div class="spacer"></div>
                <button type="button" class="create-btn"  onclick="window.location.href='AddTools.aspx'"><img src="../Images/icons/add.png"/>ADD NEW TOOLS</button>
            </div>

            <div class="row">
                <asp:Repeater ID="rptTools" runat="server">
                    <ItemTemplate>
                        <div class="col-md-4">
                            <div class="tool-card">
                                <div class="category-edit">
                                    <small><%# Eval("Category_Name").ToString().ToUpper() %></small>
                                    <button type="button" class="create-btn"
                                        style="text-align: right; background-color: #9BA0A6; color: #1B263B; font-size: 15px; min-width: 50px; height: 20px; padding: 15px;"
                                        onclick="window.location.href='EditTools.aspx?Tool_ID=<%# Eval("Tool_ID") %>'">
                                        <img src="../Images/icons/pencil.png" style="width: 12px; height: 12px;" />Edit
                                    </button>
                                </div>
                                <h5><%# Eval("Tool_Name") %></h5>
                                <a href='<%# Eval("Tool_Description") %>' target="_blank"><p><%# Eval("Tool_Description") %></p></a>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </form>
    <script>
        document.addEventListener("DOMContentLoaded", function () {
            const buttons = document.querySelectorAll(".category-btn");
            const cols = document.querySelectorAll(".col-md-4"); 
            buttons.forEach(btn => {
                btn.addEventListener("click", function () {
                    buttons.forEach(b => b.classList.remove("active"));
                    this.classList.add("active");

                    const category = this.textContent.trim();

                    cols.forEach(col => {
                        const smallText = col.querySelector(".tool-card small").textContent.trim(); 

                        if (category === "All" || smallText === category) {
                            col.style.display = "block";
                        } else {
                            col.style.display = "none";
                        }
                    });
                });
            });
        });
    </script>
</body>
</html>
