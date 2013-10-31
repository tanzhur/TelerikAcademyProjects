using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Forum.WebAPI.Controllers;
using Store.Data;
using Store.Models;
using Store.WebAPI.Models;
using System.Web.Http.ValueProviders;
using Forum.Services.Attributes;

namespace Store.WebAPI.Controllers
{
    public class ProductsController : BaseApiController
    {
        private const int SessionKeyLength = 50;

        public HttpResponseMessage GetProduct([ValueProvider(typeof(HeaderValueProviderFactory<string>))]string sessionKey)
        {
            var responseMsg = this.PerformOperationAndHandleExceptions<HttpResponseMessage>(
              () =>
              {
                  using (var context = new StoreContext())
                  {
                      this.ValidateSessionKey(sessionKey);

                      var found = (from product in context.Products.Include("Categories")
                                   select new ProductModel
                                   {
                                       Id = product.Id,
                                       Name = product.Name,
                                       Price = product.Price,
                                       CategoryName = product.Category.Name,
                                       CategoryId = product.Category.Id,
                                       Description = product.Description,
                                       ImageSource = product.ImageSource,
                                       Quantity = product.Quantity
                                   }).ToList();

                      var response = this.Request.CreateResponse(HttpStatusCode.OK,
                                          found);
                      return response;
                  }
              });

            return responseMsg;
        }

        [HttpGet]
        public HttpResponseMessage SearchProductsByName(string searchName, [ValueProvider(typeof(HeaderValueProviderFactory<string>))]string sessionKey)
        {
            var responseMsg = this.PerformOperationAndHandleExceptions<HttpResponseMessage>(
              () =>
              {
                  using (var context = new StoreContext())
                  {
                      this.ValidateSessionKey(sessionKey);

                      var found = (from product in context.Products.Include("Categories")
                                   where product.Name.Contains(searchName)
                                   select new ProductModel
                                   {
                                       Id = product.Id,
                                       Name = product.Name,
                                       Price = product.Price,
                                       CategoryName = product.Category.Name,
                                       CategoryId = product.Category.Id,
                                       Description = product.Description,
                                       ImageSource = product.ImageSource,
                                       Quantity = product.Quantity
                                   }).ToList();

                      var response = this.Request.CreateResponse(HttpStatusCode.OK,
                                          found);
                      return response;
                  }
              });

            return responseMsg;
        }

        //GET api/products/{productId}
        public HttpResponseMessage GetProduct(int productId, [ValueProvider(typeof(HeaderValueProviderFactory<string>))]string sessionKey)
        {
            var responseMsg = this.PerformOperationAndHandleExceptions<HttpResponseMessage>(
              () =>
              {
                  using (var context = new StoreContext())
                  {
                      this.ValidateSessionKey(sessionKey);

                      var product = context.Products.Include("Category").FirstOrDefault(p => p.Id == productId);
                      if (product == null)
                      {
                          throw new ArgumentException("Product not found");
                      }

                      var resultProductModel = new ProductModel
                      {
                          Id = product.Id,
                          Name = product.Name,
                          Price = product.Price,
                          CategoryName = product.Category.Name,
                          CategoryId = product.Category.Id,
                          Description = product.Description,
                          ImageSource = product.ImageSource,
                          Quantity = product.Quantity
                      };

                      var response = this.Request.CreateResponse(HttpStatusCode.OK,
                                          resultProductModel);
                      return response;
                  }
              });

            return responseMsg;
        }

        //POST api/products
        public HttpResponseMessage PostProduct([FromBody]ProductModel model, [ValueProvider(typeof(HeaderValueProviderFactory<string>))]string sessionKey)
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

                      //TODO: Validate Escaping

                      var category = context.Categories.Find(model.CategoryId);

                      if (category == null)
                      {
                          throw new ArgumentException("Category not found");
                      }

                      var product = new Product
                      {
                          Name = model.Name,
                          Description = model.Description,
                          ImageSource = model.ImageSource,
                          Category = category,
                          Price = model.Price,
                          Quantity = model.Quantity,
                      };

                      context.Products.Add(product);
                      context.SaveChanges();
                  }

                  var response = new HttpResponseMessage(HttpStatusCode.Created);
                  return response;
              });

            return responseMsg;
        }

        //PUT api/products/{productId}
        public HttpResponseMessage PutProduct([FromBody]ProductModel model, int productId, [ValueProvider(typeof(HeaderValueProviderFactory<string>))]string sessionKey)
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

                      //TODO: Validate Escaping

                      var existingProduct = context.Products.FirstOrDefault(p => p.Id == productId);

                      if (existingProduct == null)
                      {
                          throw new ArgumentException("Product not found");
                      }

                      var category = context.Categories.FirstOrDefault(c => c.Id == model.CategoryId);

                      if (category == null)
                      {
                          throw new ArgumentException("Category not found");
                      }

                      if (model.Name != null)
                      {
                          existingProduct.Name = model.Name;
                      }

                      if (model.Description != null)
                      {
                          existingProduct.Description = model.Description;
                      }

                      existingProduct.Category = category;

                      if (model.Price != 0)
                      {
                          existingProduct.Price = model.Price;
                      }

                      //TODO: Should have an option to set the quantity 0 
                      //default quantity-to is 1 = 0;
                      if (model.Quantity != 0)
                      {
                          existingProduct.Quantity = model.Quantity;
                      }
                      var product = new Product
                      {
                          Name = model.Name,
                          Description = model.Description,
                          ImageSource = model.ImageSource,
                          Category = category,
                          Price = model.Price,
                          Quantity = model.Quantity,
                      };

                      context.SaveChanges();
                  }

                  var response = new HttpResponseMessage(HttpStatusCode.Created);
                  return response;
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
