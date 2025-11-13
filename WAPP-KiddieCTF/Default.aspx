<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WAPP_KiddieCTF.Default" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width,initial-scale=1,maximum-scale=1">
    <link rel="shortcut icon" type="image/png" href="Images/Logo.png">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.1/css/all.min.css">
    <title>KIDDIE CTF</title>
    <link rel="stylesheet" href="Default.css">
    <style>
        html {
            scroll-behavior: smooth;
        }

        #closeModalBtn {
            position: absolute;
            top: 10px;
            right: 15px;
            font-size: 24px;
            font-weight: bold;
            color: #333;
            cursor: pointer;
        }

        #closeModalBtn:hover {
            color: #ff0000;
    }
    </style>
</head>
<body>
    <div class="head">
        <div class="left">
            <img src="Images/Logo.png" alt="">
        </div>
        <div class="spacer"></div>
        <div class="right">
            <div class="head-nav">
                <a href="#home"><h1>HOME</h1></a>
                <a href="#ctf"><h1>CTF</h1></a>
                <a href="#courses"><h1>OUR COURSES</h1></a>
                <a href="#challenges"><h1>OUR CHALLENGES</h1></a>
                <a href="LogIn.aspx"><h1 class="login-btn">LOGIN</h1></a>
            </div>
        </div>
    </div>

    <div id="home" class="hero">
        <img src="Images/hero.JPG" alt="">
        <div class="overlay"></div>
        <div class="text">
            <h1>DECODE THE FUTURE</h1>
            <h3>Fuel your curiosity, build your skills, and uncover the hidden world of cybersecurity</h3>
            <h2>“one code, one flag, one discovery at a time”</h2>
        </div>
    </div>

    <div id="ctf" class="section">
        <div class="left">
            <h2>What is Capture The Flag (CTF)?</h2>
            <p>Capture The Flag (CTF) is a fun and safe way to learn cybersecurity through hands-on challenges.</p>
            <p>Each challenge hides a flag — a secret code you must find by solving puzzles, analyzing code, or breaking through digital defenses.</p>
            <p>Every flag you capture earns you points and new skills!</p>
        </div>
        <div class="right">
            <img src="Images/ctf.png" alt="">
        </div>
    </div>

    <div id="courses" class="course-section">
        <h2>Course Categories</h2>
        <div style="margin: 20px;"></div>
        <div class="section">
            <div class="left" style="text-align:center;">
                <img src="../Images/Course.png" style="width: 500px;"/>
            </div>
            <div class="right">
                <p style="text-align:justify; font-size: 20px;">Whether you’re decoding secret messages, solving puzzles, or uncovering hidden flags, every challenge helps you build real cybersecurity skills step by step.
                    <br />
                    🎓 You can now enrol in Kiddie CTF courses through your lecturers — learn at your own pace, compete with friends, and discover the thrill of ethical hacking in a safe, guided environment.
                    <br />
                    It’s time to decode the future — starting today! 🚀</p>
            </div>
        </div>
    </div>

    <div id="challenges" class="course-section">
        <h2>Challenges</h2>
        <div class="section">
            <div class="challenge">
                <div class="each-challenge">
                    <h6>1. Pick a Challenge</h6>
                    <p>Choose a beginner-friendly challenge in web, crypto, or forensics. Each has a description and difficulty level.</p>
                </div>
                <div class="each-challenge">
                    <h6>2. Analyze and Solve</h6>
                    <p>Use hints, tutorials, and your curiosity to explore and figure out the puzzle.</p>
                </div>
                <div class="each-challenge">
                    <h6>3. Capture the Flag</h6>
                    <p>Once you find the secret code (e.g., FLAG{you_did_it}), submit it to earn points.</p>
                </div>
            </div>
            <div class="img">
                <img src="../Images/Challenge.png" style="width: 500px;"/>
            </div>
        </div>
        <div class="button">
            <button class="btn" onclick="window.location.href='Student/Challenges.aspx'">START YOUR FIRST CHALLENGE NOW!</button>
        </div>
    </div>

    <footer class="footer">
        <div class="left">
            <img src="Images/Logo.png" alt="">
            <h3>KIDDIE CTF</h3>
            <p>“one code, one flag, one discovery at a time”</p>
        </div>
        <div class="right">
            <div class="contact">
                <h3>Address:</h3>
                <p>No. 27, Jalan Setia Perdana U13/25, <br>
                    Setia Alam, 40170 Shah Alam, <br>
                    Selangor, Malaysia.
                </p>
            </div>
            <div class="contact">
                <h3>Contact:</h3>
                <p>016-123 4567</p>
            </div>
            <div class="contact">
                <h3>Email:</h3>
                <p>info@kiddiectf.com</p>
            </div>
        </div>
    </footer>
    <div class="copyright">
        <p>© Copyright 2025. All Rights Reserved | KIDDIECTF</p>
    </div>
</body>
</html>
