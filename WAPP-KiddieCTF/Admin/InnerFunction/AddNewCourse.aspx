<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddNewCourse.aspx.cs" Inherits="WAPP_KiddieCTF.Admin.InnerFunction.AddNewCourse" %>
<%@ Register Src="~/Admin/SideBar.ascx" TagPrefix="uc" TagName="SideBar" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Kiddie CTF - Add New Course (Admin)</title>

    <!-- Correct Paths (we are in /Admin/InnerFunction) -->
    <link href="../css/sidebar.css" rel="stylesheet" />
    <link href="../css/css2/addNewCourse.css" rel="stylesheet" />

    <!-- Fonts & SweetAlert -->
    <link href="https://fonts.googleapis.com/css2?family=Teko:wght@400;500;600&display=swap" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>

    <!-- Fix overlapping + maintain 50px side gap -->
    <style>
        body {
            margin: 0;
            background: #000;
            font-family: 'Teko', sans-serif;
        }

        /* layout beside sidebar */
        .main {
            margin-left: 250px;             /* same width as sidebar */
            width: calc(100% - 250px);      /* remaining width */
            padding: 40px 50px;             /* top/bottom 40px, left/right 50px gap */
            min-height: 100vh;
            background: #000;
            box-sizing: border-box;
            display: block;                 /* override shared flex center */
        }

        .page-title {
            font-size: 50px;
            font-weight: 600;
            letter-spacing: 2.5px;
            color: #fff;
            margin-bottom: 20px;
            align-self: flex-start;
            margin-left: 40px;
        }

        .content-panel {
            margin: 0 auto;                 /* keep center */
            width: 1060px;
            min-height: 766px;
            border-radius: 20px;
            background: #1B263B;
            padding: 40px;
            position: relative;
            display: flex;
            justify-content: center;
        }

        .form-container {
            width: 100%;
            max-width: 935px;
            display: flex;
            flex-direction: column;
            align-items: center;
            gap: 30px;
            margin-top: 40px;
        }

        .back-btn {
            position: absolute;
            top: 20px;
            left: 20px;
            background: none;
            border: none;
            cursor: pointer;
        }

        .back-btn img {
            width: 47px;
            height: 47px;
        }

        .course-id-label {
            color: white;
            font-size: 30px;
            font-weight: 700;
            letter-spacing: 1.5px;
            margin-bottom: 20px;
        }

        .input-group {
            width: 100%;
            display: flex;
            flex-direction: column;
            gap: 10px;
        }

        .input-group label {
            color: white;
            font-size: 20px;
            font-weight: 600;
            letter-spacing: 1px;
        }

        .course-name-box {
            position: relative;
            width: 100%;
            height: 60px;
            background: rgba(69, 80, 102, 0.7);
            border-radius: 10px;
            padding: 0 20px;
            display: flex;
            align-items: center;
            box-sizing: border-box;
        }

        .course-name-input {
            background: transparent;
            font-family: 'Teko', sans-serif;
            border: none;
            color: #9BA0A6;
            font-size: 18px;
            font-weight: 600;
            letter-spacing: 0.9px;
            flex: 1;
            outline: none;
            height: 100%;
            padding: 0;
        }

        .course-name-box .placeholder-label {
            position: absolute;
            left: 20px;
            top: 50%;
            transform: translateY(-50%);
            color: #9BA0A6;
            font-size: 18px;
            font-weight: 600;
            letter-spacing: 0.9px;
            pointer-events: none;
            transition: all 0.2s ease;
            opacity: 1;
        }

        .course-name-input:not(:placeholder-shown) + .placeholder-label,
        .course-name-input:focus + .placeholder-label {
            opacity: 0;
            transform: translateY(-50%) scale(0.8);
            top: 10px;
            font-size: 14px;
        }

        .student-section {
            width: 100%;
            display: flex;
            flex-direction: column;
            gap: 15px;
            align-items: center;
        }

        .student-section label {
            color: white;
            font-size: 20px;
            font-weight: 600;
            letter-spacing: 1px;
        }

        .student-panel {
            width: 500px;
            height: 143px;
            background: rgba(69, 80, 102, 0.7);
            border-radius: 10px;
            padding: 20px;
            display: flex;
            justify-content: center;
            align-items: center;
        }

        .student-buttons {
            display: flex;
            gap: 30px;
            width: 100%;
            justify-content: center;
        }

        .student-btn-wrapper {
            position: relative;
            width: 235px;
            height: 127px;
            display: inline-block;
        }

        .student-btn {
            width: 100%;
            height: 100%;
            background: #9BA0A6;
            border: none;
            border-radius: 10px;
            cursor: pointer;
            display: flex;
            align-items: center;
            justify-content: center;
            text-decoration: none;
        }

        .student-btn .btn-content {
            display: flex;
            flex-direction: column;
            align-items: center;
            justify-content: center;
            width: 100%;
            height: 100%;
        }

        .btn-icon {
            width: 79px;
            height: 79px;
            margin-bottom: 10px;
        }

        .btn-text {
            color: #1B263B;
            font-size: 18px;
            font-weight: 600;
            letter-spacing: 0.9px;
        }

        .done-btn {
            width: 134px;
            height: 55px;
            background: #9BA0A6;
            border: 2px solid #9BA0A6;
            border-radius: 10px;
            color: #1B263B;
            font-size: 32px;
            font-weight: 600;
            letter-spacing: 1.6px;
            cursor: pointer;
            margin-top: 50px;
            font-family: 'Teko', sans-serif;
            transition: all 0.25s ease-in-out;
        }

        .done-btn:hover {
            background: transparent;
            color: #9BA0A6;
            border: 4px solid #9BA0A6;
            transform: translateY(-2px);
        }
    </style>
</head>

<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <!-- Sidebar -->
        <uc:SideBar ID="SidebarControl" runat="server" />

        <!-- Main Content -->
        <div class="main">
            <h1 class="page-title">Add New Course</h1>

            <div class="content-panel">
                <div class="form-container">

                    <!-- Back Button -->
                    <button type="button" class="back-btn" onclick="window.location='../Courses.aspx'">
                        <img src="../images/back_icon.png" alt="Back" />
                    </button>

                    <!-- Course ID -->
                    <div class="course-id-label">
                        Course ID: <asp:Label ID="lblCourseID" runat="server" Text=""></asp:Label>
                    </div>

                    <!-- Course Name -->
                    <div class="input-group">
                        <label>Course Name</label>
                        <div class="course-name-box">
                            <asp:TextBox ID="txtCourseName" runat="server" CssClass="course-name-input" placeholder=" "></asp:TextBox>
                            <label class="placeholder-label">Enter course name</label>
                        </div>
                    </div>

                    <!-- Student Enrolled -->
                    <div class="student-section">
                        <label>Student Enrolled</label>

                        <div class="student-panel">
                            <div class="student-buttons">

                                <!-- ADD STUDENTS -->
                                <div class="student-btn-wrapper">
                                    <asp:LinkButton ID="btnAddStudents" runat="server" CssClass="student-btn" OnClick="btnAddStudents_Click">
                                        <div class="btn-content">
                                            <img src="../images/addStudent.png" alt="Add" class="btn-icon" />
                                            <span class="btn-text">Add Students</span>
                                        </div>
                                    </asp:LinkButton>
                                </div>

                                <!-- VIEW STUDENTS -->
                                <div class="student-btn-wrapper">
                                    <asp:LinkButton ID="btnViewStudents" runat="server" CssClass="student-btn" OnClick="btnViewStudents_Click">
                                        <div class="btn-content">
                                            <img src="../images/viewStudent.png" alt="View" class="btn-icon" />
                                            <span class="btn-text">View Students</span>
                                        </div>
                                    </asp:LinkButton>
                                </div>

                            </div>
                        </div>
                    </div>

                    <!-- DONE -->
                    <asp:Button ID="btnDone" runat="server" CssClass="done-btn" Text="DONE" OnClick="btnDone_Click" />

                </div>
            </div>
        </div>
    </form>
</body>
</html>
