/// <reference path="_references.js" />

angular.module("store", [])
	.config(["$routeProvider", function ($routeProvider) {
	    $routeProvider
			.when("/", {
			    templateUrl: "scripts/partials/home.html"
			})
            .when("/logout", {
                templateUrl: "scripts/partials/logout.html",
                controller: controllers.getAdminController
            })
            .when("/admin/login", {
                templateUrl: "scripts/partials/admin-login-form.html",
                controller: controllers.getAdminController
            })
            .when("/admin/register", {
                templateUrl: "scripts/partials/admin-register-form.html",
                controller: controllers.getAdminController
            })
            .when("/users", {
                templateUrl: "scripts/partials/all-users.html",
                controller: controllers.getUsersController
            })
             //.when("/category/create", {
             //    templateUrl: "scripts/partials/create-category.html",
             //    controller: controllers.getCategoryController
             //})
             .when("/categories", {
                 templateUrl: "scripts/partials/create-category.html",
                 controller: controllers.getCategoryController
             })
			.otherwise({ redirectTo: "/" });
	}]);


