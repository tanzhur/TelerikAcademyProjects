/// <reference path="http-requester.js" />
/// <reference path="class.js" />
/// <reference path="sha1.js" />

var persisters = (function () {
    var nickname = null;
    var nickID = null;
    var chatID = null;
    var userProfilePicture = null;

    function saveUserData(userData) {
        nickID = userData.Id;
        nickname = userData.Username;
        userProfilePicture = userData.ImageUrl;
    }

    function saveChatID(id) {
        chatID = id;
    }

    function clearUserData() {
        nickname = null;
        nickID = null;
        chatID = null;
        userProfilePicture = null;

    }

    var MainPersister = Class.create({
        init: function (rootUrl) {
            this.rootUrl = rootUrl;
            this.user = new UserPersister(this.rootUrl);
            this.chat = new ChatPersister(this.rootUrl);
            this.messages = new MessagesPersister(this.rootUrl);

            // online users
            this.users = new UsersPersister(this.rootUrl);
        },
        isUserLoggedIn: function () {
            var isLoggedIn = nickname != null && nickname != "undefined";
            return isLoggedIn;
        },
        nickname: function () {
            return nickname;
        },
        nickID: function () {
            return nickID;
        },
        chatID: function () {
            return chatID;
        },
        userProfilePicture: function () {
            return userProfilePicture;
        }

    });

    var UsersPersister = Class.create({
        init: function (rootUrl) {
            this.rootUrl = rootUrl + "users/";
        },
        all: function (success, error) {
            var url = this.rootUrl;
            httpRequester.getJSON(url, success, error);
        }
    });

    var UserPersister = Class.create({
        init: function (rootUrl) {
            this.rootUrl = rootUrl + "users/";
        },
        login: function (user, success, error) {
            var url = this.rootUrl + "?username=" + user.username;
            var userData = {
                username: user.username,
            };

            httpRequester.getJSON(url,
                 function (data) {
                     saveUserData(data);
                     success(data);
                 }, error);
        },
        logout: function (success, error) {
            var url = this.rootUrl + nickID;
            httpRequester.putJSON(url, {}, function (data) {
                clearUserData();
                success(data);
            }, error)
        },
    });

    var ChatPersister = Class.create({
        init: function (url) {
            this.rootUrl = url + "chats/";
        },
        postMessage: function (chatId, messageModel, success, error) {
            var url = this.rootUrl;
            httpRequester.postJSON(url + chatId, messageModel,
                function (data) {
                    success(data);
                }
            , error);
        },
        // Add participants
        getChat: function (user1ID, user2ID, success, error) {
            var chatData = {
                'Participants':
                    [{
                        'id': user1ID
                    },
                    {
                        'id': user2ID
                    }]
            };
            var self = this;
            var url = this.rootUrl;
            httpRequester.postJSON(url, chatData,
                function (data) {
                    saveChatID(data.Id);
                    success(data);
                }
            , error);
        },
    });

    var MessagesPersister = Class.create({
        init: function (url) {
            this.rootUrl = url + "chats/";
        },
        byChatId: function (chatID, success, error) {
            var url = this.rootUrl + chatID;

            httpRequester.getJSON(url, success, error);
        }
    });

    return {
        get: function (url) {
            return new MainPersister(url);
        }
    };
}());