<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddDiscussions.aspx.cs" Inherits="WAPP_KiddieCTF.Student.AddDiscussions" %>
<%@ Register Src="~/Student/SideBar.ascx" TagPrefix="uc" TagName="SideBar" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>New Discussion</title>
    <link href="css/discussions.css" rel="stylesheet" />
    <style>
        .head {
            display: flex;
            flex-direction: row;
            align-items: center;
        }

        .head .back {
            background-color: #1B263B;
            color: white;
            border-radius: 5px;
            padding: 5px 10px;
            letter-spacing: 1px;
            font-weight: 500;
            min-width: 70px;
            border: none;
            text-decoration: none;
            cursor: pointer;
        }

        .discussion-container {
            background-color: #1B263B;
            border-radius: 20px;
            padding: 40px;
        }

        .each-card {
            background-color: #455066;
            opacity: .7;
            border-radius: 12px;
            margin-bottom: 10px;
        }

        .input-discussion, .input-description {
            background-color: transparent;
            width: 100%;
            border: none;
            color: white;
            border-radius: 8px;
            font-size: 15px;
            letter-spacing: 0.5px;
            outline: none;
        }

        .input-discussion::placeholder, .input-description::placeholder {
            color: #9aa5b1;
        }

        .image-label {
            display: block;
            background-color: transparent;
            color: white;
            padding: 40px 0;
            border-radius: 8px;
            cursor: pointer;
            font-weight: 600;
            letter-spacing: 1px;
            transition: background-color 0.2s;
        }

        .image-label:hover {
            background-color: #324765;
        }

        .hidden-file {
            display: none;
        }

        .post-btn {
            background-color: #ffffff;
            color: #000;
            padding: 6px 18px;
            border-radius: 6px;
            border: none;
            cursor: pointer;
            font-weight: 600;
        }

        .post-btn:hover {
            background-color: #dcdcdc;
        }

        .post-btn.active {
            background-color: #32d294;
            color: white;
            cursor: pointer;
        }


    </style>
</head>
<body>
    <form id="form1" runat="server">
        <uc:SideBar ID="SidebarControl" runat="server" />
        <div class="main-content" style="margin-left: 250px; padding: 40px;">
            <div class="head">
                <h2 style="color: white; margin-bottom: 20px;">New Discussion</h2>
                <div class="spacer"></div>
                <button class="back" type="button" onclick="window.location.href='Discussions.aspx'">Back</button>
            </div>

            <div class="discussion-container">
                <div class="each-card" style="padding: 20px;">
                    <asp:TextBox ID="txtDiscussionName" runat="server" CssClass="input-discussion" placeholder="Discussion Name"></asp:TextBox>
                </div>
                <div class="each-card" style="padding: 20px; text-align: center; position: relative;">
                    <label for="fileImage" id="imageLabel" class="image-label">ADD IMAGE</label>
                    <asp:FileUpload ID="fileImage" runat="server" CssClass="hidden-file" accept="image/*" />

                    <div id="imagePreviewContainer" style="display:none; position: relative; display: inline-block; margin-top: 10px;">
                        <img id="previewImage" src="#" alt="Preview" style="max-width:50%; border-radius:10px;" />
                        <button type="button" id="removeImageBtn"
                            style="position: absolute; top: 5px; right: 5px; background-color: rgba(0,0,0,0.6);
                                   color: white; border: none; border-radius: 50%; width: 24px; height: 24px;
                                   font-weight: bold; cursor: pointer;">×</button>
                    </div>
                </div>
                <div class="each-card" style="padding: 20px;">
                    <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Rows="5" CssClass="input-description" placeholder="Always get better by coding~"></asp:TextBox>
                </div>
                <div style="text-align:right; margin-top:15px;">
                    <asp:Button ID="btnPost" runat="server" Text="Post" CssClass="post-btn" OnClick="btnPost_Click"/>
                </div>
            </div>
        </div>
    </form>
    <script>
        const titleInput = document.getElementById('<%= txtDiscussionName.ClientID %>');
        const descInput = document.getElementById('<%= txtDescription.ClientID %>');
        const imageInput = document.getElementById('<%= fileImage.ClientID %>');
        const postBtn = document.getElementById('<%= btnPost.ClientID %>');
        const imageLabel = document.getElementById('imageLabel');
        const previewImage = document.getElementById('previewImage');
        const previewContainer = document.getElementById('imagePreviewContainer');
        const removeImageBtn = document.getElementById('removeImageBtn');

        window.addEventListener("load", function () {
            imageInput.value = "";
            previewContainer.style.display = "none";
            previewImage.src = "#";
            imageLabel.textContent = "ADD IMAGE";
            imageLabel.style.color = "white";
            validateInputs();
        });

        imageInput.addEventListener("change", function () {
            if (imageInput.files && imageInput.files[0]) {
                const reader = new FileReader();
                reader.onload = function (e) {
                    previewImage.src = e.target.result;
                    previewContainer.style.display = "inline-block";
                    imageLabel.textContent = "Image Selected ✔";
                    imageLabel.style.color = "#00ff9d";
                };
                reader.readAsDataURL(imageInput.files[0]);
            }
            validateInputs();
        });

        removeImageBtn.addEventListener("click", function () {
            imageInput.value = ""; // clear input
            previewContainer.style.display = "none";
            previewImage.src = "#";
            imageLabel.textContent = "ADD IMAGE";
            imageLabel.style.color = "white";
            validateInputs();
        });

        function validateInputs() {
            const title = titleInput.value.trim();
            const description = descInput.value.trim();
            const hasImage = imageInput.files.length > 0;

            if (title && (hasImage || description)) {
                postBtn.classList.add("active");
                postBtn.removeAttribute("disabled");
                postBtn.style.cursor = "pointer";
            } else {
                postBtn.classList.remove("active");
                postBtn.setAttribute("disabled", "disabled");
                postBtn.style.cursor = "not-allowed";
            }
        }

        titleInput.addEventListener("input", validateInputs);
        descInput.addEventListener("input", validateInputs);
    </script>
</body>
</html>
