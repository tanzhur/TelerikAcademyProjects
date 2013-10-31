/// <reference path="../libs/_references.js" />
/// <reference path="data.js" />

window.vmFactory = (function () {
    var data = null;

    function getLoginViewModel(successCallback) {
        var viewModel = {
            username: "",
            password: "",
            errorMessage: "",
            login: function () {
                var self = this;
                data.users.login(this.get("username"), this.get("password"))
					.then(function () {
					    if (successCallback) {
					        successCallback();
					    }
					}, function (error) {
					    var errorRecieved = JSON.parse(error.responseText)
					    self.set("errorMessage", errorRecieved.Message);
					    
					});
            },
            register: function () {
                var self = this;
                data.users.register(this.get("username"), this.get("password"))
					.then(function () {
					    if (successCallback) {
					        successCallback();
					    }
					}, function (error) {
					    var errorRecieved = JSON.parse(error.responseText)
					    self.set("errorMessage", errorRecieved.Message);
					    
					});
            }
        };
        return kendo.observable(viewModel);
    };

    function getAlreadyLoggedUserViewModel() {
        var displayName = data.users.getNickname();
        var viewModel = {
            displayName: displayName,
            logout: function () {
                data.users.logout()
                .then(function (returnedData) {
                    //user is logged out, now a login page should be shown
                    window.location.assign("");
                    data.users.clearUserData();
                }, function (err) {
                    console.log(err);
                })
            }
        }
        return kendo.observable(viewModel);
    }

    //this may be reused for api/products and api/categories/:id/products
    function productsViewModel(productData) {
        return kendo.observable({
            Name: productData.Name,
            Description: productData.Description,
            ImageSource: productData.ImageSource,
            Price: productData.Price,
            Link: "#/products/"+productData.Id,
            Quantity: productData.Quantity,
            addToBasket: function (productData) {
                data.basket.addProductToBasket(productData);
            }
        });
    }

    function productsSearchViewModel() {
        return kendo.observable({
            searchInput: "",
            results:[],
            search: function () {
                var searched = this.get('searchInput');
                data.products.searchByName(searched)
                .then(function (data) {
                    viewsFactory.getProductsFromCategoryView()
                    .then(function (productsView) {
                        $('.searchResults').html("");
                        renderProducts(data, productsView, '.searchResults');
                    }, function (err) {
                        console.log(err);
                    });

                }, function (err) {
                    console.log(err);
                });
            }
        });
    }

    function productInBasketViewModel(basketData) {
        return kendo.observable({
            Name: basketData.Name,
            ImageSource: basketData.ImageSource,
            SinglePrice: basketData.Price,
        });
    }

    function basketCheckoutViewModel() {
        return kendo.observable({
            checkout: function () {
                data.basket.checkout().then(function (data) {
                    $('#main-content').html("<strong>Products ordered.</strong>");
                }, function (err) {
                    console.log(err);
                });
            },
            clearBasket: function () {
                $('.product-in-basket').remove();
                data.basket.clearBasket();
            },
        });
    }

    return {
        getLoginVM: getLoginViewModel,
        getLoggedUserVM: getAlreadyLoggedUserViewModel,
        setPersister: function (persister) {
            data = persister
        },
        getProductsByCatVM: productsViewModel,
        getProductsSearchVM: productsSearchViewModel,
        getProductInBasketVM: productInBasketViewModel,
        getBasketCheckoutVM: basketCheckoutViewModel,
        getProductVM: productsViewModel
    };
}());