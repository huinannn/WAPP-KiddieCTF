<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="WAPP_KiddieCTF.Student.Dashboard" %>
<%@ Register Src="~/Student/SideBar.ascx" TagPrefix="uc" TagName="SideBar" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Student Dashboard</title>
    <link href="css/dashboard.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Teko:wght@400;500;600&display=swap" rel="stylesheet"/>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0-beta3/css/all.min.css" rel="stylesheet"/>
    <link href="~/Images/Logo.png" rel="shortcut icon" type="image/x-icon" />
</head>
<body>
   <form id="form1" runat="server">
        <uc:SideBar ID="SidebarControl" runat="server" />

        <div class="main-content">
            <div class="dashboard-title">Dashboard</div>

            <div class="dashboard-container">
               <div class="recent-courses-container">
                    <div class="section-title-container">
                        <div class="section-title">Recently Accessed Courses</div>
                        <div class="arrows-container">
                            <div class="arrow-left" onclick="scrollCourses('left')">&#10094;</div>
                            <div class="arrow-right" onclick="scrollCourses('right')">&#10095;</div>
                        </div>
                    </div>
                    <div class="courses-container">
                        <!-- Course Cards -->
                        <div class="courses">
                            <div class="course-card">
                                <i class="fas fa-book icon"></i>
                                <div class="course-name">Cybersecurity Fundamentals</div>
                                <div class="lecturer-id">Lecturer ID: L001</div>
                            </div>
                            <div class="course-card">
                                <i class="fas fa-network-wired icon"></i>
                                <div class="course-name">Networking Basics</div>
                                <div class="lecturer-id">Lecturer ID: L002</div>
                            </div>
                            <div class="course-card">
                                <i class="fas fa-certificate icon"></i>
                                <div class="course-name">Digital Transformation</div>
                                <div class="lecturer-id">Lecturer ID: L003</div>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Right Section: Upcoming Tasks -->
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

            <!-- Bottom Section: Total Competed Courses, Total Certificates, Challenge Progress -->
            <div class="total-info-container">
                <div class="total-info">
                    <!-- Courses Info Card -->
                    <div class="info-card">
                        <i class="fas fa-book icon"></i> <!-- Course Icon -->
                        <div class="info-number">10</div>
                        <div class="info-label">Total Completed Courses</div>
                    </div>

                    <!-- Certificates Info Card -->
                    <div class="info-card">
                        <i class="fas fa-certificate icon"></i> <!-- Certificate Icon -->
                        <div class="info-number">5</div>
                        <div class="info-label">Total Certificates</div>
                    </div>

                    <!-- Challenges Progress Info Card (Circular Progress Chart) -->
                     <div class="info-card">
                         <div class="chart-container">
                             <svg width="100" height="100" viewBox="0 0 100 100">
                                 <circle cx="50" cy="50" r="45" stroke="#4FC3F7" stroke-width="5" fill="none"/>
                                 <circle cx="50" cy="50" r="45" stroke="#9BA0A6" stroke-width="5" fill="none" stroke-dasharray="283" stroke-dashoffset="141.5"></circle>
                             </svg>
                             <div class="chart-label">Completed 5/10 Challenges</div>
                         </div>
                     </div>
                </div>
            </div>

        </div>

    </form>

    <script>
        let currentIndex = 0;
        const totalCards = document.querySelectorAll('.course-card').length;
        const coursesContainer = document.querySelector('.course-cards');
        const cardsToShow = 2; // Number of courses to show at a time
        const scrollAmount = 250 * cardsToShow + 20 * (cardsToShow - 1); // Calculate the scroll amount based on card width and margin

        // Function to scroll left or right
        function scrollCourses(direction) {
            if (direction === 'right' && currentIndex < totalCards - cardsToShow) {
                currentIndex++;
                coursesContainer.style.transform = `translateX(-${currentIndex * scrollAmount}px)`;
            } else if (direction === 'left' && currentIndex > 0) {
                currentIndex--;
                coursesContainer.style.transform = `translateX(-${currentIndex * scrollAmount}px)`;
            }
        }


    </script>
</body>
</html>
