/// <reference path="class.js" />
/// <reference path="persister.js" />
/// <reference path="jquery-2.0.2.js" />
/// <reference path="ui.js" />

var controllers = (function () {

    var updateTimer = null;
    var rootUrl = "http://shaggychat.apphb.com/api/";

    var Controller = Class.create({
        init: function () {
            this.persister = persisters.get(rootUrl);
        },

        loadUI: function (selector) {
            if (this.persister.isUserLoggedIn()) {
                this.loadChatUI(selector);
            }
            else {
                this.loadLoginFormUI(selector);
            }
            this.attachUIEventHandlers(selector);
            var self = this;

        },

        loadLoginFormUI: function (selector) {
            var loginFormHtml = ui.loginForm()
            $(selector).html(loginFormHtml);
        },

        loadChatUI: function (selector) {
            var self = this;

            var nick = self.persister.nickname();
            var nickId = self.persister.nickID();
            // ****************************
            // PUBNUB
            // ****************************

            PUBNUB.subscribe({
                channel: self.persister.nickname(),
                callback: function (message) {
                    // Received a message --> print it in the page
                    var chat = JSON.parse(message);
                    var allMessages = httpRequester.getJSON(rootUrl + "chats/" + chat.ChatId + "?messageId=" + chat.MessageId,
					function (data) {
					    var chatWindow = $("#chatwindow");
					    if (chatWindow == null) {

					        $("#user-list").find("[data-user-id='" + chat.UserId + "']").find("a").append("<img src='./Images/message-small.png' />");
					    }
					    else {
					        var existingChatId = chatWindow.data("chat-id");
					        if (existingChatId != chat.ChatId) {
					            var anchor = $("#user-list").find("[data-user-id='" + chat.UserId + "']").find("a");
					            var image = anchor.find("img");
					            if (image.length <= 0) {
					                anchor.append("<img src='./Images/message-small.png' />");
					            }

					            return;
					        }
					        for (var i = 0; i < data.Messages.length; i++) {
					            chatWindow.find("#chatbox").append("<p>" + data.Messages[i].Time + ": " + data.Messages[i].Sender.Username + ": " + data.Messages[i].Content + '\n' + "</p>");
					        }
					    }
					});
                }
            });

            PUBNUB.subscribe({
                channel: "UserStatusChanged",
                callback: function (message) {
                    var user = JSON.parse(message);
                    var existingUser = $("#user-list").find("[data-user-id='" + user.UserId + "']");
                    if (user.Status == "Online" && existingUser.length <= 0 && user.UserId != self.persister.nickID()) {
                        var list =
                    '<li class="user-element" data-user-id="' + user.UserId + '" data-user-name="' + user.Username + '">' +
                        '<a href="#" >' +
                            user.Username
                        '</a>' +
                    '</li>';
                        $("#user-list").append(list)
                    }
                    else if (user.Status == "Offline" && existingUser.length > 0) {
                        existingUser.off();
                        existingUser.remove();
                    }
                }
            });

            var mainUI = ui.ChatUI();
            $(selector).html(mainUI);

            var userProfile = self.persister.userProfilePicture();
            this.profileField(selector, userProfile, nick);
            this.updateUI(selector);

        },

        attachUIEventHandlers: function (selector) {
            var wrapper = $(selector);
            var self = this;

            wrapper.on("click", "#enter", function () {
                var user = {
                    username: $(selector + " #name").val()
                }

                self.persister.user.login(user, function () {
                    self.loadChatUI(selector);
                }, function (err) {
                    // wrapper.find("#error-messages").text(err.responseJSON.Message);
                });

                return false;
            });

            wrapper.on("click", "#logoutbutton", function () {

                self.persister.user.logout(
                    function (data) {
                        wrapper.off();
                        self.loadUI(selector);
                    }, function (err) {
                        wrapper.find("#error-messages").text(err.responseJSON.Message);
                    });
            });

            wrapper.on("click", "#upload", function () {
                var formData = new FormData($('#uploadImage')[0]);
                $.ajax({
                    url: 'http://shaggychat.apphb.com/api/images/?userid=' + self.persister.nickID(),
                    type: 'POST',
                    data: formData,
                    cache: false,
                    contentType: false,
                    processData: false,
                    success: function (data) {
                        var currentPicture = $("#profilePicture");
                        if (currentPicture.length <= 0) {
                            $('#profile-field').prepend('<img id="profilePicture" src="' + data + '" alt="profile picture"/>');
                        }
                        else {
                            currentPicture.attr("src", data);
                        }
                    },
                    error: function () { }
                });

            });

            wrapper.on("click", "#uploadFile", function () {
                var formData = new FormData($('#uploadFileForm')[0]);
                var that = $(this);
                $.ajax({
                    url: 'http://shaggychat.apphb.com/api/files/' + self.persister.nickID(),
                    type: 'POST',
                    data: formData,
                    cache: false,
                    contentType: false,
                    processData: false,
                    success: function (data) {
                        var chatId = that.parent().parent().data("chat-id");
                            var userMessage = '<a href=' + data + ' target="_blank">' + data + '</a>';
                            var messageModel = { Content: userMessage, Time: new Date(), Sender: { Id: nickID = self.persister.nickID() } };
                            self.persister.chat.postMessage(chatId, messageModel,
                                function (data) {
                                    var chatWindow = $("#chatwindow");
                                    chatWindow.find("#chatbox").append("<p>" + data.Time + ": " + data.Sender.Username + ": " + data.Content + '\n' + "</p>");

                                }, function (err) {
                                    var chatWindow = $("#chatwindow");
                                    chatWindow.find("#chatbox").append("<p class='error'>Message Not Delivered</p>");
                                    wrapper.find("#error-messages").text(err.responseJSON.Message);
                        });
                    },
                    error: function () { }
                });

            });

            wrapper.on("click", "#user-list a", function () {
                var otherNickname = $(this).parent().data("user-name");
                var otherId = $(this).parent().data("user-id");
                var nickName = self.persister.nickname();
                var nickID = self.persister.nickID();
                var chatWindowUI = null;
                var image = $(this).find("img");
                self.persister.chat.getChat(nickID, otherId,
                    function (data) {
                        self.persister.messages.byChatId(data.Id,
                            function (chatModel) {
                                chatWindowUI = ui.ChatWindowUI(otherNickname, chatModel);
                                $(selector + " #chat-div").html(chatWindowUI);
                                if (image != null) {
                                    image.remove();
                                }
                            }, function (err) {
                                wrapper.find("#error-messages").text(err.responseJSON.Message);
                            });
                    }, function (err) {
                        wrapper.find("#error-messages").text(err.responseJSON.Message);
                    });
            });


            wrapper.on("click", "#submitmsg", function (event) {
                event.preventDefault()
                var userMessage = $(this).parent().find("#usermsg").val();
                var chatId = $(this).parent().parent().data("chat-id");
                var messageModel = { Content: userMessage, Time: new Date(), Sender: { Id: nickID = self.persister.nickID() } };
                var that = $(this);
                self.persister.chat.postMessage(chatId, messageModel,
                    function (data) {
                        var chatWindow = $("#chatwindow");
                        that.parent().find("#usermsg").val('');
                        chatWindow.find("#chatbox").append("<p>" + data.Time + ": " + data.Sender.Username + ": " + data.Content + '\n' + "</p>");

                    }, function (err) {
                        var chatWindow = $("#chatwindow");
                        chatWindow.find("#chatbox").append("<p class='error'>Message Not Delivered</p>");
                        wrapper.find("#error-messages").text(err.responseJSON.Message);
                    });

            });

            wrapper.on("click", "#exit", function () {

                var chatbox = $(this).parent().parent().parent();
                $(this).off();
                chatbox.find("form").find("#submitmsg").off();
                chatbox.remove();
                //self.loadUI(selector);
            });
        },

        updateUI: function (selector) {
            var self = this;
            // all users
            this.persister.users.all(function (users) {
                var list = ui.userList(users, self.persister.nickID());
                $(selector + " #users-div").html(list);
            });

            // Autoscrolling chat window
            //Load the file containing the chat log  
            //function loadLog() {
            //    var oldscrollHeight = $("#chatbox").attr("scrollHeight") - 20; //Scroll height before the request  
            //    $.ajax({
            //        url: "log.html",
            //        cache: false,
            //        success: function (html) {
            //            $("#chatbox").html(html); //Insert chat log into the #chatbox div     

            //            //Auto-scroll             
            //            var newscrollHeight = $("#chatbox").attr("scrollHeight") - 20; //Scroll height after the request  
            //            if (newscrollHeight > oldscrollHeight) {
            //                $("#chatbox").animate({ scrollTop: newscrollHeight }, 'normal'); //Autoscroll to bottom of div  
            //            }
            //        },
            //    });
            //}
            // We already have setInterval
            // setInterval (loadLog, 2500);
        },
        profileField: function (selector, userProfile, nick) {
            var html = ui.profileField(userProfile, nick)
            $(selector).prepend(html);
        }
    });
    return {
        get: function () {
            return new Controller();
        }
    }
}());

$(function () {
    var controller = controllers.get();
    controller.loadUI("#content");

    //slider.slide("#user-list", 20, 10, 500, 1);
});