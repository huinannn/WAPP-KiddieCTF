<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tools.aspx.cs" Inherits="WAPP_KiddieCTF.Admin.Tools" %>
<%@ Register Src="~/Admin/SideBar.ascx" TagPrefix="uc" TagName="SideBar" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Admin Tools</title>

    <!-- Styles -->
    <link href="css/css2/tools.css" rel="stylesheet" />
    <link href="~/Images/Logo.png" rel="shortcut icon" type="image/x-icon" />
    <link href="css/addNewCourse.css" rel="stylesheet" runat="server" />

    <!-- Google Font -->
    <link href="https://fonts.googleapis.com/css2?family=Teko:wght@400;500;600&display=swap" rel="stylesheet" />

    <!-- SweetAlert -->
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <!-- === SIDEBAR === -->
        <uc:SideBar ID="SidebarControl" runat="server" />

        <!-- === MAIN CONTENT === -->
        <div class="main-content">
            <h2>Tools</h2>

            <div class="top-bar">
                <div class="category-group">
                    <button type="button" class="category-btn active">All</button>
                    <button type="button" class="category-btn">OSINT</button>
                    <button type="button" class="category-btn">CRYPTOGRAPHY</button>
                    <button type="button" class="category-btn">STEGANOGRAPHY</button>
                    <button type="button" class="category-btn">REVERSE ENGINEERING</button>
                </div>
                <button type="button" class="create-btn" onclick="window.location.href='InnerFunction/AddTools.aspx'">
                    <img src="../Images/icons/add.png" />ADD NEW TOOLS
                </button>
            </div>

            <div class="row">
                <asp:Repeater ID="rptTools" runat="server">
                    <ItemTemplate>
                        <div class="tool-card">
                            <div class="category-edit">
                                <small><%# Eval("Category_Name").ToString().ToUpper() %></small>
                                <button type="button" class="edit-btn"
                                    onclick="window.location.href='InnerFunction/EditTools.aspx?Tool_ID=<%# Eval("Tool_ID") %>'">
                                    <img src="../Images/icons/pencil.png" />Edit
                                </button>
                            </div>
                            <h5><%# Eval("Tool_Name") %></h5>
                            <a href='<%# Eval("Tool_Description") %>' target="_blank">
                                <p><%# Eval("Tool_Description") %></p>
                            </a>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </form>

    <script>
        document.addEventListener("DOMContentLoaded", function () {
            const buttons = document.querySelectorAll(".category-btn");
            const cards = document.querySelectorAll(".tool-card");

            buttons.forEach(btn => {
                btn.addEventListener("click", function () {
                    buttons.forEach(b => b.classList.remove("active"));
                    this.classList.add("active");

                    const category = this.textContent.trim();

                    cards.forEach(card => {
                        const smallText = card.querySelector("small").textContent.trim();
                        card.style.display = (category === "All" || smallText === category) ? "block" : "none";
                    });
                });
            });
        });
    </script>
</body>
</html>
