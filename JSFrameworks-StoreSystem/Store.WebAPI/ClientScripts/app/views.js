/// <reference path="../libs/_references.js" />


window.viewsFactory = (function () {
    var rootUrl = "ClientScripts/partials/";

    var templates = {};

    function getTemplate(name) {
        var promise = new RSVP.Promise(function (resolve, reject) {
            if (templates[name]) {
                resolve(templates[name])
            }
            else {
                $.ajax({
                    url: rootUrl + name + ".html",
                    type: "GET",
                    success: function (templateHtml) {
                        templates[name] = templateHtml;
                        resolve(templateHtml);
                    },
                    error: function (err) {
                        reject(err)
                    }
                });
            }
        });
        return promise;
    }

    function getHomeView() {
        return getTemplate("home");
    }

    function getLoginView() {
        return getTemplate("login-form");
    }

    function getBasketView() {
        return getTemplate("basket");
    }

    function getBasketCheckoutView() {
        return getTemplate("basket-checkout");
    }

    function getAlreadyLoggedUser() {
        return getTemplate("logged-user");
    }

    function getCategoriesView() {
        return getTemplate("categories");
    }

    function getProductsFromCategoryView() {
        return getTemplate("productsFromCategory");
    }

    function getProductsView() {
        return getTemplate("products-description");
    }

    function getProductsSearchView() {
        return getTemplate("productsSearchView");
    }

    function getAboutPage() {
        return getTemplate('aboutView');
    }

    return {
        getLoginView: getLoginView,
        getBasketView: getBasketView,
        getBasketCheckoutView: getBasketCheckoutView,
        getHomeView: getHomeView,
        getAlreadyLoggedUser: getAlreadyLoggedUser,
        getCategoriesView: getCategoriesView,
        getProductsFromCategoryView: getProductsFromCategoryView,
        getProductsSearchView: getProductsSearchView,
        getAboutPage: getAboutPage,
        getProductsView: getProductsView
	};
}());