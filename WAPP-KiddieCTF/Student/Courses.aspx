<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Courses.aspx.cs" Inherits="WAPP_KiddieCTF.Student.Courses" %>
<%@ Register Src="~/Student/SideBar.ascx" TagPrefix="uc" TagName="SideBar" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Student Courses</title>
    <link href="css/courses.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Teko:wght@400;500;600&display=swap" rel="stylesheet"/>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0-beta3/css/all.min.css" rel="stylesheet" />
    <link href="~/Images/Logo.png" rel="shortcut icon" type="image/x-icon" />
</head>
<body>
    <form id="form1" runat="server">
        <uc:SideBar ID="SidebarControl" runat="server" />
        
        <!-- 隐藏字段和按钮用于记录点击 -->
        <asp:HiddenField ID="hdnCourseId" runat="server" />
        <asp:Button ID="btnRecordClick" runat="server" OnClick="btnRecordClick_Click" Style="display: none;" />
        
        <div class="main-content">
            <div class="courses-title">Courses</div>
            <div class="search-container">
                <i class="fa fa-search search-icon"></i>
                <input type="text" id="searchTextBox" runat="server" placeholder="Search Course Name" onkeydown="handleSearch(event)" />
            </div>
            
            <asp:Label ID="NoCoursesLabel" runat="server" Text="There are no courses available!" Visible="false" Font-Bold="true" />
            <asp:Label ID="NoSearchResultsLabel" runat="server" Text="No results found for your search!" Visible="false" Font-Bold="true" />
            
            <div class="courses-container" id="coursesContainer">
                <asp:Repeater ID="CoursesRepeater" runat="server">
                    <ItemTemplate>
                        <div class="course-card" onclick="redirectToCourse('<%# Eval("Course_ID") %>')">
                            <div class="course-details">
                                <h3><%# Eval("Course_Name") %></h3>
                                <p>Course ID: <%# Eval("Course_ID") %></p>
                                <p>Lecturer Name: <%# Eval("Lecturer_Name") %></p>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </form>

    <script type="text/javascript">
        function handleSearch(event) {
            if (event.keyCode === 13) {
                var searchText = document.getElementById('<%= searchTextBox.ClientID %>').value.trim();
                if (searchText.length > 0) {
                    __doPostBack('<%= searchTextBox.ClientID %>', searchText);
                }
            }
        }

        function redirectToCourse(courseId) {
            // 设置隐藏字段并触发按钮点击
            document.getElementById('<%= hdnCourseId.ClientID %>').value = courseId;
            document.getElementById('<%= btnRecordClick.ClientID %>').click();
        }
    </script>
</body>
</html>