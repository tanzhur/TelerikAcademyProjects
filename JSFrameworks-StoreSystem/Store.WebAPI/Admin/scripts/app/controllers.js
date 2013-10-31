/// <reference path="../_references.js" />

window.controllers = (function () {
    var baseUrl = "http://localhost:29831/api/";

    var nickname = localStorage.getItem("nickname");
    var sessionKey = localStorage.getItem("sessionKey");

    var monster = {};
    function saveUserData(userData) {
        localStorage.setItem("nickname", userData.displayName);
        localStorage.setItem("sessionKey", userData.sessionKey);
       
        nickname = userData.displayName;
        sessionKey = userData.sessionKey;
    }
    function clearUserData() {
        localStorage.removeItem("nickname");
        localStorage.removeItem("sessionKey");
        nickname = "";
        sessionKey = "";
    }
    function isUserLoggedIn() {
        var isLoggedIn = nickname != null && sessionKey != null;
        return isLoggedIn;
    }

    function AdminController($scope, $http, $location) {
        $scope.user = {
            username: "",
            displayName: "",
            authCode: ""
        };

        $scope.loginAdmin = function () {
            console.log($scope.user);
            var userToLogin = {
                username: $scope.user.username,
                authCode: CryptoJS.SHA1($scope.user.authCode).toString()
            };

            $http.post(baseUrl + "users/login", userToLogin)
                .success(function (data) {
                    saveUserData(data);
                    console.log(nickname);
                    $scope.user.authCode = "";
                }).error(function (err) {
                    $location.path("#/admin/login");
                    alert(err.Message);
                });
        }

        $scope.logoutAdmin = function () {
            var config = { headers: { "X-sessionKey": sessionKey } };
            $http.put(baseUrl + "users/logout", {}, config)
                .success(function () {
                    $scope.user = {
                        username: "",
                        displayName: "",
                        authCode: ""
                    };
                    clearUserData();
                }).error(function (err) {
                    alert(err.Message);
                });
        }

        $scope.registerAdmin = function () {
            var userToRegister = {
                username: $scope.user.username,
                displayName: $scope.user.displayName,
                authCode: CryptoJS.SHA1($scope.user.authCode).toString()
            };
            var config = { headers: { "X-sessionKey": sessionKey } };
            $http.post(baseUrl + "users/registeradmin", userToRegister, config)
                .success(function (data) {
                    saveUserData(data);
                    $scope.user.authCode = "";
                    alert("Admin is created!");
                }).error(function (err) {
                    alert(err.Message);
                });
        }
    }

    function HomeController($location) {
        if (!isUserLoggedIn()) {
            $location.path("#/admin/login");
        }
    }

    function UsersController($scope, $http) {
        $scope.users = [];
        var config = { headers: { "X-sessionKey": sessionKey } };

        $http.get(baseUrl + "users", config)
            .success(function (data) {
                for (var i in data) {
                    $scope.users.push(data[i]);
                }
            }).error(function (err) {
                alert(err.Message);
            });
    }

    function CategoryController($scope, $http, $routeParams) {
        $scope.category = {
            id: "",
            name: "",
            imageSource: "",
            description: ""
        };


        $scope.categories = [];
        var config = { headers: { "X-sessionKey": sessionKey } };

        $http.get(baseUrl + "categories", config)
        .success(function (data) {
            for (var i in data) {
                $scope.categories.push(data[i]);
            }
        }).error(function (err) {
            alert(err.Message);
        });

        $scope.createCategory = function () {
            var config = { headers: { "X-sessionKey": sessionKey } };

            $http.post(baseUrl + "categories/", $scope.category, config)
                .success(function (data) {
                    $scope.category = data;
                    console.log(data);
                    alert("Category is created!");
                }).error(function (err) {
                    alert(err.Message);
                });
        }
    }

    return {
        getAdminController: AdminController,
        getUsersController: UsersController,
        getCategoryController: CategoryController
    };
})();