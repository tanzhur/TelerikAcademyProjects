/// <reference path="jquery-2.0.2.js" />
var ui = (function () {

    function buildLoginForm() {
        var html =
            '<div id="loginform">' +
                '<form action="index.html" method="post">' +
                    '<p>Please enter your name to continue:</p>' +
                    '<label for="name">Name:</label>' +
                    '<input type="text" name="name" id="name" />' +
                    '<input class="jqueryui-button" type="submit" name="enter" id="enter" value="Enter" />' +
                '</form>' +
            '</div>';

        return html;
    }

    function buildProfileField(userProfilePicture, username) {
        var html =
            '<div id="profile-field" style="border: 1px solid black">';
        if (userProfilePicture != null && userProfilePicture != undefined) {
            html += '<img id="profilePicture" src="'+userProfilePicture+'" alt="Profile picture" />';
        }
            
            html+='<span id="username">'+username+'</span>' +
            '<a class="jqueryui-button" id="logoutbutton">Logout</a>' +
            '<span>UploadImage</span>' +
            '<form style="display: inline-block" id="uploadImage" enctype="multipart/form-data">' +
                '<input name="file" type="file" />' +
                '<input class="jqueryui-button" type="button" id="upload" value="Upload" />' +
            '</form>' +
        '</div>';
        return html;
    }

    function buildChatUI() {
        var html =
            '<div id="users-div" class="clearfix"></div>' +
            '<div id="chat-div"></div>' +
            '<div id="#error-container"><p id="error-messages"></p></div>';

        return html;
    }

    function buildChatWindow(nickname, chatModel) {

        var messagesHtml = "";

        for (var i = 0; i < chatModel.Messages.length; i++) {
            var message = chatModel.Messages[i];
            messagesHtml += '<p>' + message.Time + ": " + message.Sender.Username + ': ' + message.Content + '</p>';
        }

        var html =
           '<div id="chatwindow" data-chat-id=' + chatModel.Id + '>' +
               '<div id="menu">' +
                   '<p class="welcome">Chat with: ' + nickname + '<b></b></p>' +
                   '<p class="logout"><a id="exit" href="#">Exit Chat</a></p>' +
                   '<div style="clear:both"></div>' +
               '</div>' +

               '<div id="chatbox">' + messagesHtml + '</div>' +

               '<form name="message" action="">' +
                   '<input name="usermsg" type="text" id="usermsg" size="63" />' +
                   '<input class="jqueryui-button" name="submitmsg" type="submit" id="submitmsg" value="Send" />' +
                   '</form>' +
                    '<form style="display: inline-block" id="uploadFileForm" enctype="multipart/form-data">' +
                '<input name="file" type="file" />' +
                '<input class="jqueryui-button" type="button" id="uploadFile" value="Upload" />' +
            '</form>'
           '</div>';

        return html;
    }

    function buildUserList(users, loggedUserId) {
        var list = '<ul id="user-list"><li class="user-element"><h3>Online Users</h3></li> ';
        for (var i = 0; i < users.length; i++) {
            var user = users[i];
            if (user.Id != loggedUserId) {

                list +=
                    '<li class="user-element" data-user-id="' + user.Id + '" data-user-name="' + user.Username + '">';
                if (user.ImageUrl != null) {
                    list += '<img class="userlistProfilePicture" src="' + user.ImageUrl + '" />';
                }
                    
                       list+= '<a href="#" >' +
                            $("<div />").html(user.Username).text() +
                        '</a>' +
                    '</li>';
            }
        }
        list += "</ul>";

        return list;
    }

    return {
        ChatUI: buildChatUI,
        ChatWindowUI: buildChatWindow,
        loginForm: buildLoginForm,
        userList: buildUserList,
        chatWindow: buildChatWindow,
        profileField: buildProfileField
    }
}());