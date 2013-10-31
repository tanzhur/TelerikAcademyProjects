using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Forum.WebAPI.Controllers;
using Store.Data;
using Store.WebAPI.Models;
using System.Web.Http.ValueProviders;
using Forum.Services.Attributes;
using Store.Models;
using System.Net.Http.Formatting;

namespace Store.WebAPI.Controllers
{
    public class OrdersController : BaseApiController
    {
        private const int SessionKeyLength = 50;
        private const string Pending = "Pending";
        private const string Done = "Done";

        //GET api/orders/
        public HttpResponseMessage GetAll([ValueProvider(typeof(HeaderValueProviderFactory<string>))]string sessionKey)
        {
            var responseMsg = this.PerformOperationAndHandleExceptions<HttpResponseMessage>(
              () =>
              {
                  using (var context = new StoreContext())
                  {
                      this.ValidateSessionKey(sessionKey);

                      var admin = context.Users.FirstOrDefault(u => u.SessionKey == sessionKey);
                      if (admin == null)
                      {
                          throw new ArgumentException("Invalid SessionKey or user is already logouted");
                      }
                      else if (admin.IsAdmin != true)
                      {
                          throw new ArgumentException("Unauthorized Access");
                      }

                      var orders = context.Orders;
                      var resultOrdersModel = from order in orders
                                              select new OrderModel
                      {
                          Id = order.Id,
                          UserId = order.User.Id,
                          Username = order.User.Username,
                          SingleOrders = from singleOrder in order.SingleOrders
                                         select new SingleOrderModel
                                     {
                                         Id = singleOrder.Id,
                                         Quantity = singleOrder.Quantity,
                                         Product = new ProductModel
                                         {
                                             Id = singleOrder.Product.Id,
                                             Name = singleOrder.Product.Name,
                                             Description = singleOrder.Product.Description,
                                             ImageSource = singleOrder.Product.ImageSource,
                                             CategoryId = singleOrder.Product.Category.Id,
                                             CategoryName = singleOrder.Product.Category.Name,
                                             Price = singleOrder.Product.Price
                                         }
                                     }
                      };

                      var response = this.Request.CreateResponse(HttpStatusCode.OK,
                                          resultOrdersModel.ToList());
                      return response;
                  }
              });

            return responseMsg;
        }

        //GET api/orders/{orderID}
        public HttpResponseMessage GetOrder(int orderId, [ValueProvider(typeof(HeaderValueProviderFactory<string>))]string sessionKey)
        {
            var responseMsg = this.PerformOperationAndHandleExceptions<HttpResponseMessage>(
              () =>
              {
                  using (var context = new StoreContext())
                  {
                      this.ValidateSessionKey(sessionKey);

                      var admin = context.Users.FirstOrDefault(u => u.SessionKey == sessionKey);
                      if (admin == null)
                      {
                          throw new ArgumentException("Invalid SessionKey or user is already logouted");
                      }
                      else if (admin.IsAdmin != true)
                      {
                          throw new ArgumentException("Unauthorized Access");
                      }

                      var order = context.Orders.FirstOrDefault(c => c.Id == orderId);
                      if (order == null)
                      {
                          throw new ArgumentException("Category not found");
                      }

                      OrderModel resultCategoryModel = new OrderModel
                      {
                          Id = order.Id,
                          UserId = order.User.Id,
                          Username = order.User.Username,
                          SingleOrders = from singleOrder in order.SingleOrders
                                         select new SingleOrderModel
                                     {
                                         Id = singleOrder.Id,
                                         Quantity = singleOrder.Quantity,
                                         Product = new ProductModel
                                         {
                                             Name = singleOrder.Product.Name,
                                             Description = singleOrder.Product.Description,
                                             ImageSource = singleOrder.Product.ImageSource,
                                             CategoryId = singleOrder.Product.Category.Id,
                                             CategoryName = singleOrder.Product.Category.Name,
                                             Price = singleOrder.Product.Price
                                         }
                                     }
                      };

                      var response = this.Request.CreateResponse(HttpStatusCode.OK,
                                          resultCategoryModel);
                      return response;
                  }
              });

            return responseMsg;
        }

        //Post api/orders/
        public HttpResponseMessage PostOrders(ICollection<SingleOrder> orders, [ValueProvider(typeof(HeaderValueProviderFactory<string>))]string sessionKey)
        {
            var responseMsg = this.PerformOperationAndHandleExceptions<HttpResponseMessage>(
             () =>
             {
                 using (var context = new StoreContext())
                 {
                     this.ValidateSessionKey(sessionKey);

                     var user = context.Users.FirstOrDefault(u => u.SessionKey == sessionKey);
                     if (user == null)
                     {
                         throw new ArgumentException("Invalid SessionKey or user is already logouted");
                     }

                     //Adding main order
                     var order = new Order()
                     {
                         User = user,
                         Status = Pending
                     };
                     context.Orders.Add(order);
                     context.SaveChanges();

                     //Adding orders for individual products
                     foreach (var singleOrder in orders)
                     {
                         var product = context.Products.FirstOrDefault(p => p.Name == singleOrder.Product.Name);
                         if (product.Quantity == 0)
                         {
                             throw new InvalidOperationException("Current product is out of stock!");
                         }
                         product.Quantity--;

                         var newSingleOrder = new SingleOrder
                         {
                             Quantity = 1,
                             Product = product,
                             Order = order,
                         };

                         context.SingleOrders.Add(newSingleOrder);
                         context.SaveChanges();
                     }

                     var response = this.Request.CreateResponse(HttpStatusCode.OK, JsonMediaTypeFormatter.DefaultMediaType);
                     return response;
                 }
             });

            return responseMsg;
        }

        private void ValidateSessionKey(string sessionKey)
        {
            if (sessionKey == null || sessionKey.Length != SessionKeyLength)
            {
                throw new ArgumentOutOfRangeException("Invalid SessionKey");
            }
        }
    }
}
