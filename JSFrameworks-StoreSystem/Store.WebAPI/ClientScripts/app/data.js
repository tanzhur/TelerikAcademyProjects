window.persisters = (function () {
    var sessionKey = "";
    var bashUsername = "";
    function getJSON(requestUrl, headers) {
        var promise = new RSVP.Promise(function (resolve, reject) {
            $.ajax({
                url: requestUrl,
                type: "GET",
                dataType: "json",
                headers: headers,
                success: function (data) {
                    resolve(data);
                },
                error: function (err) {
                    reject(err);
                }
            });
        });
        return promise;
    }

    function postJSON(requestUrl, data, headers) {
        var promise = new RSVP.Promise(function (resolve, reject) {
            $.ajax({
                url: requestUrl,
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify(data),
                dataType: "json",
                headers: headers,
                success: function (data) {
                    resolve(data);
                },
                error: function (err) {
                    reject(err);
                }
            });
        });
        return promise;
    }

    function putJSON(requestUrl, data, headers) {
        var promise = new RSVP.Promise(function (resolve, reject) {
            $.ajax({
                url: requestUrl,
                type: "PUT",
                contentType: "application/json",
                data: JSON.stringify(data),
                dataType: "json",
                headers: headers,
                success: function (data) {
                    resolve(data);
                },
                error: function (err) {
                    reject(err);
                }
            });
        });
        return promise;
    }

    function createHeader() {
        var headers = {
            "X-sessionKey": sessionKey
        };

        return headers;
    }

    var UsersPersister = Class.create({
        init: function (apiUrl) {
            this.apiUrl = apiUrl;
        },
        login: function (username, password) {
            var user = {
                username: username,
                authCode: CryptoJS.SHA1(password).toString()
            };
            return postJSON(this.apiUrl + "login", user)
				.then(function (data) {
				    //save to localStorage
				    sessionKey = data.sessionKey;
				    bashUsername = data.displayName;

				    localStorage.setItem("sessionkey", sessionKey);
				    localStorage.setItem("nickname", bashUsername);
				});
        },
        register: function (username, password) {
            var user = {
                username: username,
                authCode: CryptoJS.SHA1(password).toString()
            };
            return postJSON(this.apiUrl + "register", user)
				.then(function (data) {
				    //save to localStorage
				    sessionKey = data.sessionKey;
				    bashUsername = data.displayName;
				    localStorage.setItem("sessionkey", sessionKey);
				    localStorage.setItem("nickname", bashUsername);
				    return data.displayName;
				});
        },
        logout: function () {
            if (!sessionKey) {
                //gyrmi
            }

            return putJSON(this.apiUrl + "logout", "", createHeader());
        },
        currentUser: function () {
            bashUsername = localStorage.getItem("nickname");
            sessionKey = localStorage.getItem("sessionkey");

            if (sessionKey === null || sessionKey == "null" || sessionKey == ""
            || bashUsername == "null" || bashUsername == "") {
                return "";
            }

            return bashUsername;
        },
        getNickname: function () {
            return bashUsername;
        },
        clearUserData: function () {
            localStorage.removeItem("sessionkey");
            localStorage.removeItem("nickname");
            bashUsername = "";
            sessionKey = "";
        }
    });

    //TODO: orderProduct in localStorage?
    //TODO: Sending to Database
    var BasketPersister = Class.create({
        init: function (apiUrl) {
            this.apiUrl = apiUrl;
            this.orderProducts = [];
        },
        getProductsInBasket: function () {
            return this.orderProducts;
        },
        addProductToBasket: function (product) {
            //TODO: forbid ordering same product multiple times
            this.orderProducts.push(product);
        },
        checkout: function () {
            var orders = [];
            for (var i = 0; i < this.orderProducts.length; i++) {
                var order = {
                    id: this.orderProducts[i].data.Id,
                    product: this.orderProducts[i].data,
                    quantity: this.orderProducts[i].data.Quantity
                }
                orders.push(order);
            }

            //Clearing basket after checkout
            this.orderProducts = [];

            return postJSON(this.apiUrl, orders, createHeader());
        },
        clearBasket: function () {
            this.orderProducts = [];
            console.log("Basket cleared!");
        },
    });


    var CategoriesPersister = Class.create({
        init: function (apiUrl) {
            this.apiUrl = apiUrl;
        },
        all: function () {
            return getJSON(this.apiUrl, createHeader());
        },
        getProductsFromCategory: function (id) {
            return getJSON(this.apiUrl + id + "/products", createHeader());
        }
    });

    var ProductsPersister = Class.create({
        init: function (apiUrl) {
            this.apiUrl = apiUrl;
        },
        all: function () {
            return getJSON(this.apiUrl, createHeader());
        },
        searchByName: function (name) {
            return getJSON(this.apiUrl + "?searchName=" + name, createHeader());
        },
        getById: function (id) {
            return getJSON(this.apiUrl  + id, createHeader())
        }
    });

    var DataPersister = Class.create({
        init: function (apiUrl) {
            this.apiUrl = apiUrl;
            this.users = new UsersPersister(apiUrl + "users/");
            this.basket = new BasketPersister(apiUrl + "orders/");
            this.categories = new CategoriesPersister(apiUrl + "categories/");
            this.products = new ProductsPersister(apiUrl + "products/");
        }
    });


    return {
        get: function (apiUrl) {
            return new DataPersister(apiUrl);
        }
    }
}());