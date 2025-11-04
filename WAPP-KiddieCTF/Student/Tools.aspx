<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tools.aspx.cs" Inherits="WAPP_KiddieCTF.Student.Tools" %>
<%@ Register Src="~/Student/SideBar.ascx" TagPrefix="uc" TagName="SideBar" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Student Tools</title>
    <link href="css/tools.css" rel="stylesheet" />
    <link href="~/Images/Logo.png" rel="shortcut icon" type="image/x-icon" />
    <style>
        a {
            text-decoration: none;
        }

        a:hover, a:focus, a:visited {
            text-decoration: none;
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
        }

        .category-btn.active, .category-btn:hover {
            background-color: #133B5C;
            color: white;
        }

        .tool-card {
            background-color: #455066;
            opacity: .7;
            aspect-ratio: 280/173;
            min-width: 250px;
            min-height: 173px;
            padding: 20px;
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
    </style>
</head>

<body>
    <form id="form1" runat="server">
        <uc:SideBar ID="SidebarControl" runat="server" />

        <div class="main-content" style="margin-left: 250px; padding: 40px;">
            <h2 style="color: white;">Tools</h2>

            <div class="mb-4" style="margin-top: 20px;">
                <button type="button" class="category-btn active" style="font-weight: bold;">All</button>
                <button type="button" class="category-btn">OSINT</button>
                <button type="button" class="category-btn">Cryptography</button>
                <button type="button" class="category-btn">Steganography</button>
                <button type="button" class="category-btn">Reverse Engineering</button>
            </div>

            <div class="row">
                <a href="https://gchq.github.io/CyberChef/" target="_blank">
                    <div class="col-md-4">
                        <div class="tool-card">
                            <small>OSINT</small>
                            <h5>CyberChef</h5>
                            <p>https://cyhs.github.io/CyberChef/</p>
                        </div>
                    </div>
                </a>
                <div class="col-md-4">
    <div class="tool-card">
        <small>OSINT</small>
        <h5>CyberChef</h5>
        <a href="https://cyhs.github.io/CyberChef/">https://cyhs.github.io/CyberChef/</a>
    </div>
</div>
                <div class="col-md-4">
    <div class="tool-card">
        <small>OSINT</small>
        <h5>CyberChef</h5>
        <a href="https://cyhs.github.io/CyberChef/">https://cyhs.github.io/CyberChef/</a>
    </div>
</div>
                <div class="col-md-4">
    <div class="tool-card">
        <small>OSINT</small>
        <h5>CyberChef</h5>
        <a href="https://cyhs.github.io/CyberChef/">https://cyhs.github.io/CyberChef/</a>
    </div>
</div>

                <div class="col-md-4">
                    <div class="tool-card">
                        <small>Cryptography</small>
                        <h5>Hash Analyzer</h5>
                        <a href="https://www.md5hashgenerator.com/">https://www.md5hashgenerator.com/</a>
                    </div>
                </div>

                <div class="col-md-4">
                    <div class="tool-card">
                        <small>Steganography</small>
                        <h5>StegOnline</h5>
                        <a href="https://stegonline.georgeom.net/">https://stegonline.georgeom.net/</a>
                    </div>
                </div>
                <div class="col-md-4">
    <div class="tool-card">
        <small>Steganography</small>
        <h5>StegOnline</h5>
        <a href="https://stegonline.georgeom.net/">https://stegonline.georgeom.net/</a>
    </div>
</div>

                <div class="col-md-4">
                    <div class="tool-card">
                        <small>Reverse Engineering</small>
                        <h5>Ghidra</h5>
                        <a href="https://ghidra-sre.org/">https://ghidra-sre.org/</a>
                    </div>
                </div>
                 <div class="col-md-4">
     <div class="tool-card">
         <small>Reverse Engineering</small>
         <h5>Ghidra</h5>
         <a href="https://ghidra-sre.org/">https://ghidra-sre.org/</a>
     </div>
 </div>
                 <div class="col-md-4">
     <div class="tool-card">
         <small>Reverse Engineering</small>
         <h5>Ghidra</h5>
         <a href="https://ghidra-sre.org/">https://ghidra-sre.org/</a>
     </div>
 </div>

                <div class="col-md-4">
                    <div class="tool-card">
                        <small>OSINT</small>
                        <h5>Shodan</h5>
                        <a href="https://www.shodan.io/">https://www.shodan.io/</a>
                    </div>
                </div>
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
                        const parent = card.closest(".col-md-4");

                        if (category === "All" || smallText === category) {
                            parent.style.display = "block";
                        } else {
                            parent.style.display = "none";
                        }
                    });
                });
            });
        });
    </script>
</body>
</html>
