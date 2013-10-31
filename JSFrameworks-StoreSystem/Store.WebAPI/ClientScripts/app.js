/// <reference path="libs/_references.js" />


(function () {
    var appLayout =
		new kendo.Layout('<div id="logging" class="loginform"></div><div id="main-content"></div>');
    var data = persisters.get("api/");
    vmFactory.setPersister(data);

    var router = new kendo.Router();
    router.route("/", function () {
        var bashUser = data.users.currentUser();
        if (bashUser == "") {
            viewsFactory.getLoginView()
                .then(function (loginViewHtml) {

                    var loginVM = vmFactory.getLoginVM(function () {
                        viewsFactory.getAlreadyLoggedUser()
                         .then(function (loggedUserHtml) {
                             var loggedUserVM = vmFactory.getLoggedUserVM();
                             var view = new kendo.View(loggedUserHtml, { model: loggedUserVM });
                             appLayout.showIn("#logging", view);

                         })
                    });
                    var view = new kendo.View(loginViewHtml, { model: loginVM });
                    appLayout.showIn("#logging", view);

                });
        }
        else {
            viewsFactory.getAlreadyLoggedUser()
                .then(function (loggedUserHtml) {
                    var loggedUserVM = vmFactory.getLoggedUserVM();
                    var view = new kendo.View(loggedUserHtml, { model: loggedUserVM });
                    appLayout.showIn("#logging", view);
                    $('#main-content').html("");
                })
        }
    });

    router.route("/login", function () {
        if (data.users.currentUser()) {
            router.navigate("/");
        }
        else {
            viewsFactory.getLoginView()
				.then(function (loginViewHtml) {
				    var loginVm = vmFactory.getLoginVM(
						function () {
						    router.navigate("/");
						});
				    var view = new kendo.View(loginViewHtml,
						{ model: loginVm });
				    appLayout.showIn("#logging", view);
				});
        }
    });

    router.route("/categories", function () {
        data.users.currentUser();
        $('#main-content').html("");
        viewsFactory.getCategoriesView()
            .then(function (categoriesContainer) {
                $('#main-content').append(categoriesContainer);

                data.categories.all()
                    .then(function (categories) {

                        for (var i in categories) {
                            categories[i].Id = "#/categories/" + categories[i].Id + "/products";
                        }

                        var categoriesDataSource = new kendo.data.DataSource({
                            data: categories,
                            pageSize: 5
                        });

                        categoriesDataSource.read();

                        $('#categoryContainer').kendoGrid({
                            dataSource: categoriesDataSource,
                            columns: [
                                { field: 'Name', title: 'Name', width: 220 },
                                { field: 'Description', title: 'Description' },
                                {
                                    field: 'Id', title: 'Link',
                                    template: "<a  href=#=Id#>Go To</a>"
                                },
                                {
                                    field: 'ImageSource', title: 'Image',
                                    template: "<img width=20px src=#=ImageSource# />"
                                }
                            ],
                            selectable: "row",
                            pageable: true,
                            height: 250,
                            scrollable: true,
                            //change: function () {
                            //    alert("LOL");
                            //}
                        });

                    }, function (err) {
                        console.log(err);
                    });

            }, function (err) {
                console.log(err);
            });

    });

	router.route("/basket", function () {
	    data.users.currentUser();
	    $('#main-content').html("");

	    viewsFactory.getBasketCheckoutView()
            .then(function (basketCheckoutView) {
                var obs = vmFactory.getBasketCheckoutVM();
                var view = new kendo.View(basketCheckoutView, { model: obs });

                $('#main-content').append(view.render());

            }, function (err) {
                console.log(err);
            });

	    viewsFactory.getBasketView()
            .then(function (basketView) {
                var products = data.basket.getProductsInBasket();
                for (var i = 0; i < products.length; i++) {
                    var obs = vmFactory.getProductInBasketVM(products[i].data);
                    var view = new kendo.View(basketView, { model: obs });
                    $('#main-content').append(view.render());
                }


            }, function (err) {
                console.log(err);
            });

	});


	router.route("/categories/:id/products", function (id) {
	    data.users.currentUser();
        $('#main-content').html("");

        viewsFactory.getProductsFromCategoryView()
        .then(function (productsView) {
            data.categories.getProductsFromCategory(id)
            .then(function (data) {
                renderProducts(data, productsView, '#main-content');
            }, function (err) {
                console.log(err);
            });
        }, function (err) {
            console.log(err);
        });
    });

	router.route("/products/:id", function (id) {
	    data.users.currentUser();
        $('#main-content').html("");

        data.products.getById(id).then(
           function (product) {
               var productsViewModel = vmFactory.getProductVM(product);
               viewsFactory.getProductsView().then(
                   function (htmlView) {
                       var view = new kendo.View(htmlView, { model: productsViewModel });
                       appLayout.showIn("#main-content", view);
                   })
           }, function (error) { console.log(error)})
    })

    router.route("/products", function () {
        data.users.currentUser();
        $('#main-content').html('');

        viewsFactory.getProductsSearchView()
        .then(function (searchView) {

            var obs = vmFactory.getProductsSearchVM();
            var view = new kendo.View(searchView, { model: obs });

            appLayout.showIn("#main-content", view);

        }, function (err) {
            console.log(err);
        });

    });

    router.route("/about", function () {
        $('#main-content').html('');
        viewsFactory.getAboutPage()
        .then(function (aboutView) {

            
            var view = new kendo.View(aboutView);

            appLayout.showIn("#main-content", view);

        }, function (err) {
            console.log(err);
        });
    });

    //only for registered users
    router.route("/special", function () {
        if (!data.users.currentUser()) {
            router.navigate("/login");
        }
    });

    $(function () {
        appLayout.render("#app");
        router.start();
    });
}());

function renderProducts(data, productsView, selector) {
    for (var i in data) {
        var obs = vmFactory.getProductsByCatVM(data[i]);
        var view = new kendo.View(productsView, { model: obs });
        $(selector).append(view.render());
    }
}