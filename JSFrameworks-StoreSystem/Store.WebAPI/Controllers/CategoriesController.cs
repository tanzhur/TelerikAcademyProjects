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
    public class CategoriesController : BaseApiController
    {
        private const int SessionKeyLength = 50;

        //GET api/categories/
        public HttpResponseMessage GetAll([ValueProvider(typeof(HeaderValueProviderFactory<string>))]string sessionKey)
        {
            var responseMsg = this.PerformOperationAndHandleExceptions<HttpResponseMessage>(
              () =>
              {
                  using (var context = new StoreContext())
                  {
                      this.ValidateSessionKey(sessionKey);

                      var categories = context.Categories;
                      var resultCategoryModels = from category in categories
                                                 select new CategoryModel
                                                     {
                                                         Name = category.Name,
                                                         Id = category.Id,
                                                         Description = category.Description,
                                                         ImageSource = category.ImageSource
                                                     }
                                                     ;

                      var response = this.Request.CreateResponse(HttpStatusCode.OK,
                                          resultCategoryModels.ToList());
                      return response;
                  }
              });

            return responseMsg;
        }

        //GET api/categories/{categoryID}/products
        [HttpGet]
        [ActionName("products")]
        public HttpResponseMessage GetCategory(int categoryId, [ValueProvider(typeof(HeaderValueProviderFactory<string>))]string sessionKey)
        {
            var responseMsg = this.PerformOperationAndHandleExceptions<HttpResponseMessage>(
              () =>
              {
                  using (var context = new StoreContext())
                  {
                      this.ValidateSessionKey(sessionKey);

                      var category = context.Categories.FirstOrDefault(c => c.Id == categoryId);
                      if (category == null)
                      {
                          throw new ArgumentException("Category not found");
                      }

                      var foundProducts = (from p in context.Products.Include("Categories")
                                           where p.Category.Id == category.Id
                                           select new ProductModel
                                           {
                                               Id = p.Id,
                                               CategoryId = p.Category.Id,
                                               Name = p.Name,
                                               Description = p.Description,
                                               Price = p.Price,
                                               Quantity = p.Quantity,
                                               ImageSource = p.ImageSource
                                           }).ToList();


                      var response = this.Request.CreateResponse(HttpStatusCode.OK,
                                          foundProducts);
                      return response;
                  }
              });

            return responseMsg;
        }

        //POST api/categories
        public HttpResponseMessage PostCategory([FromBody]CategoryModel model, [ValueProvider(typeof(HeaderValueProviderFactory<string>))]string sessionKey)
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

                      var name = model.Name.ToLower();

                      var existingCategory = context.Categories.FirstOrDefault(c => c.Name.ToLower() == name);

                      if (existingCategory != null)
                      {
                          throw new ArgumentException("Category with that name already exists!");
                      }

                      var category = new Category
                        {
                            Name = model.Name,
                            Description = model.Description,
                            ImageSource = model.ImageSource
                        };

                      context.Categories.Add(category);
                      context.SaveChanges();
                  }

                  var response = new HttpResponseMessage(HttpStatusCode.Created);
                  return response;
              });

            return responseMsg;
        }

        //PUT api/categories/{categoryId}
        public HttpResponseMessage PutCategory([FromBody]CategoryModel model, int categoryId, [ValueProvider(typeof(HeaderValueProviderFactory<string>))]string sessionKey)
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

                   var category = context.Categories.FirstOrDefault(c => c.Id == categoryId);
                   if (category == null)
                   {
                       throw new ArgumentException("Category not found");
                   }

                   if (model.Name != null)
                   {
                       category.Name = model.Name;
                   }

                   if (model.Description != null)
                   {
                       category.Description = model.Description;
                   }

                   if (model.ImageSource != null)
                   {
                       category.ImageSource = model.ImageSource;
                   }

                   var response = new HttpResponseMessage(HttpStatusCode.OK);
                   return response;
               }

           });

            return responseMsg;
        }
        
        // NOT WORKING because of cascading deletion
        //DELETE api/categories/{categoryId}
        public HttpResponseMessage DeleteCategory(int categoryId, [ValueProvider(typeof(HeaderValueProviderFactory<string>))]string sessionKey)
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

                   var category = context.Categories.FirstOrDefault(c => c.Id == categoryId);
                   if (category == null)
                   {
                       throw new ArgumentException("Category not found");
                   }

                   context.Categories.Remove(category);
                   context.SaveChanges();
                   var response = new HttpResponseMessage(HttpStatusCode.OK);
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
