
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Forum.WebAPI.Controllers;
using Store.Data;
using Store.Models;
using Store.WebAPI.Models;
using System.Web.Http.ValueProviders;
using Forum.Services.Attributes;

namespace Forum.WebApi.Controllers
{
    public class UsersController : BaseApiController
    {
        private const int MinNameLength = 6;
        private const int MaxNamLength = 30;
        private const string ValidUsernameCharacters =
            "qwertyuioplkjhgfdsazxcvbnmQWERTYUIOPLKJHGFDSAZXCVBNM1234567890_.";
        private const string ValidDisplayNamCharacters =
            "qwertyuioplkjhgfdsazxcvbnmQWERTYUIOPLKJHGFDSAZXCVBNM1234567890_. -";
        private const string SessionKeyChars =
            "qwertyuioplkjhgfdsazxcvbnmQWERTYUIOPLKJHGFDSAZXCVBNM";
        private const int SessionKeyLength = 50;
        private const int Sha1Length = 40;

        private static readonly Random rand = new Random();

        /*
        {  
          "username": "DonchoMinkov",
          "nickname": "Doncho Minkov",
          "authCode":   "b1cee3eb7599a7efb89a6e5fb3c9efb133d0a33e" 
        }
       */

        //GET api/users
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

                      var users = context.Users;

                      var userModels = from model in users
                                       select new UserModel { 
                                            DisplayName = model.DisplayName,
                                            Username = model.Username,
                                            IsAdmin = model.IsAdmin,
                                            ImageSource = model.ImageSource
                                       };
                      var resultUserModels = userModels.ToList();
                      var response =
                          this.Request.CreateResponse(HttpStatusCode.OK,
                                          resultUserModels);
                      return response;
                  }
              });

            return responseMsg;
        }

        //POST api/users/register
        [ActionName("register")]
        public HttpResponseMessage PostRegister([FromBody]UserModel model)
        {
            var responseMsg = this.PerformOperationAndHandleExceptions<HttpResponseMessage>(
               () =>
               {
                   using (var context = new StoreContext())
                   {
                       // trqbva da validirame userite 
                       model.DisplayName = model.Username;
                       this.ValidateUsername(model.Username);
                       this.ValidateDisplayNam(model.DisplayName);
                       this.ValidateAuthCode(model.AuthCode);

                       //sravnqvame usernames ToLower(), taka gi vkarvame i v bazata danni
                       // dokato nicknames pak gi sravnqvame v ToLower() no v bazata si ostavat
                       // nepromeneni za da moje da se pishat taka naprimer PyMeH
                       var usernameToLower = model.Username.ToLower();
                       var displayNameToLower = model.DisplayName.ToLower();
                       var user = context.Users.FirstOrDefault(
                           u => u.Username == usernameToLower ||
                               u.DisplayName == displayNameToLower);

                       if (user != null)
                       {
                           throw new InvalidOperationException("User with this username or displayname exists");
                       }

                       user = new User
                       {
                           Username = usernameToLower,
                           DisplayName = model.DisplayName,
                           AuthCode = model.AuthCode
                       };

                       context.Users.Add(user);
                       context.SaveChanges();

                       user.SessionKey = this.GenerateSessionKey(user.Id);
                       context.SaveChanges();

                       var loggedModel = new LoggedUserModel
                       {
                           DisplayName = user.DisplayName,
                           SessionKey = user.SessionKey,
                           IsAdmin = user.IsAdmin
                       };

                       var response =
                            this.Request.CreateResponse(HttpStatusCode.Created,
                                            loggedModel);
                       return response;
                   }
               });

            return responseMsg;
        }

        //POST api/users/registeradmin
        [ActionName("registeradmin")]
        public HttpResponseMessage PostRegisterAdmin([FromBody]UserModel model, [ValueProvider(typeof(HeaderValueProviderFactory<string>))]string sessionKey)
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

                       // trqbva da validirame userite 
                       this.ValidateUsername(model.Username);
                       this.ValidateDisplayNam(model.DisplayName);
                       this.ValidateAuthCode(model.AuthCode);

                       //sravnqvame usernames ToLower(), taka gi vkarvame i v bazata danni
                       // dokato nicknames pak gi sravnqvame v ToLower() no v bazata si ostavat
                       // nepromeneni za da moje da se pishat taka naprimer PyMeH
                       var usernameToLower = model.Username.ToLower();
                       var displayNameToLower = model.DisplayName.ToLower();
                       var user = context.Users.FirstOrDefault(
                           u => u.Username == usernameToLower ||
                               u.DisplayName == displayNameToLower);

                       if (user != null)
                       {
                           throw new InvalidOperationException("User with this username or displayname exists");
                       }

                       user = new User
                       {
                           Username = usernameToLower,
                           DisplayName = model.DisplayName,
                           AuthCode = model.AuthCode,
                           IsAdmin = model.IsAdmin
                       };

                       if (model.IsAdmin == true)
                       {
                           user.IsAdmin = true;
                       }
                       else
                       {
                           user.IsAdmin = false;
                       }

                       context.Users.Add(user);
                       context.SaveChanges();

                       user.SessionKey = this.GenerateSessionKey(user.Id);
                       context.SaveChanges();

                       var loggedModel = new LoggedUserModel
                       {
                           DisplayName = user.DisplayName,
                           SessionKey = user.SessionKey
                       };

                       var response =
                            this.Request.CreateResponse(HttpStatusCode.Created,
                                            loggedModel);
                       return response;
                   }
               });

            return responseMsg;
        }

        //PUT api/users/update
        [ActionName("update")]
        public HttpResponseMessage PutUpdateAdmin([FromBody]UserModel model, int userId, [ValueProvider(typeof(HeaderValueProviderFactory<string>))]string sessionKey)
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
                           if (admin.Id != userId)
                           {
                               throw new ArgumentException("Unauthorized Access");
                           }
                       }

                       var user = context.Users.FirstOrDefault(u => u.Id == userId);
                       if (user == null)
                       {
                           throw new ArgumentException("User Not Found");
                       }

                       // trqbva da validirame userite 
                       if (model.Username != null)
                       {
                           this.ValidateUsername(model.Username);
                           user.Username = model.Username.ToLower();

                       }

                       if (model.DisplayName != null)
                       {
                           this.ValidateDisplayNam(model.DisplayName);
                           user.DisplayName = model.DisplayName;
                       }

                       if (model.IsAdmin == true)
                       {
                           if (admin.IsAdmin == true)
                           {
                               user.IsAdmin = true;
                           }
                           else
                           {
                               throw new ArgumentException("Unauthorized Access");
                           }
                       }
                       else
                       {
                           user.IsAdmin = false;
                       }

                       if (model.AuthCode != null)
                       {
                           this.ValidateAuthCode(model.AuthCode);
                           user.AuthCode = model.AuthCode;
                       }

                       //context.Users.Add(user);
                       context.SaveChanges();

                       //user.SessionKey = this.GenerateSessionKey(user.Id);
                       //context.SaveChanges();

                       //var loggedModel = new LoggedUserModel
                       //{
                       //    DisplayName = user.DisplayName,
                       //    SessionKey = user.SessionKey
                       //};

                       var response =
                            this.Request.CreateResponse(HttpStatusCode.OK, "");

                       return response;
                   }
               });

            return responseMsg;
        }

        //POST api/users/login
        [ActionName("login")]
        public HttpResponseMessage PostLogin(UserModel model)
        {

            var responseMsg = this.PerformOperationAndHandleExceptions<HttpResponseMessage>(
             () =>
             {
                 using (var context = new StoreContext())
                 {
                     // trqbva da validirame userite 
                     this.ValidateUsername(model.Username);
                     this.ValidateAuthCode(model.AuthCode);

                     //sravnqvame usernames ToLower(), taka gi vkarvame i v bazata danni
                     // dokato nicknames pak gi sravnqvame v ToLower() no v bazata si ostavat
                     // nepromeneni za da moje da se pishat taka naprimer PyMeH
                     var usernameToLower = model.Username.ToLower();
                     var user = context.Users.FirstOrDefault(
                         usr => usr.Username == usernameToLower
                         && usr.AuthCode == model.AuthCode);

                     if (user == null)
                     {
                         throw new InvalidOperationException("Wrong username or password");
                     }

                     if (user.SessionKey == null)
                     {
                         user.SessionKey = this.GenerateSessionKey(user.Id);
                         context.SaveChanges();
                     }

                     var loggedModel = new LoggedUserModel
                     {
                         DisplayName = user.DisplayName,
                         SessionKey = user.SessionKey
                     };

                     var response =
                          this.Request.CreateResponse(HttpStatusCode.OK,
                                          loggedModel);
                     return response;
                 }
             });

            return responseMsg;
        }

        //PUT api/users/logout
        [ActionName("logout")]
        [HttpPut]
        public HttpResponseMessage PutLogoutUser([ValueProvider(typeof(HeaderValueProviderFactory<string>))]string sessionKey)
        {
            var responseMsg = this.PerformOperationAndHandleExceptions<HttpResponseMessage>(
             () =>
             {
                 using (var context = new StoreContext())
                 {
                     this.ValidateSessionKey(sessionKey);

                     var user = context.Users.FirstOrDefault(u => u.SessionKey == sessionKey);
                     if (user != null)
                     {
                         user.SessionKey = null;
                         context.SaveChanges();
                     }
                     else
                     {
                         throw new ArgumentException("Invalid SessionKey or user is already logouted");
                     }

                     var response = new HttpResponseMessage(HttpStatusCode.NoContent);
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

        private string GenerateSessionKey(int userId)
        {
            StringBuilder skeyBuilder = new StringBuilder(SessionKeyLength);
            skeyBuilder.Append(userId);
            while (skeyBuilder.Length < SessionKeyLength)
            {
                var index = rand.Next(SessionKeyChars.Length);
                skeyBuilder.Append(SessionKeyChars[index]);
            }
            return skeyBuilder.ToString();
        }

        private void ValidateAuthCode(string authCode)
        {
            if (authCode == null || authCode.Length != Sha1Length)
            {
                throw new ArgumentOutOfRangeException(string.Format("Password should be encrypted and it's length should be {0}", Sha1Length));
            }
        }

        private void ValidateDisplayNam(string nickname)
        {
            if (nickname == null)
            {
                throw new ArgumentNullException("DisplayName cannot be null");
            }
            else if (nickname.Length < MinNameLength)
            {
                throw new ArgumentException(string.Format("DisplayName should be at least {0} characters", MinNameLength));
            }
            else if (nickname.Length > MaxNamLength)
            {
                throw new ArgumentException(string.Format("DisplayName should be less than {0} characters", MaxNamLength));
            }
            else if (nickname.Any(ch => !ValidDisplayNamCharacters.Contains(ch)))
            {
                throw new ArgumentException(string.Format("DisplayName should contains only these characters {0}", ValidUsernameCharacters));
            }
        }

        private void ValidateUsername(string username)
        {
            if (username == null)
            {
                throw new ArgumentNullException("Username cannot be null");
            }
            else if (username.Length < MinNameLength)
            {
                throw new ArgumentException(string.Format("Username should be at least {0} characters", MinNameLength));
            }
            else if (username.Length > MaxNamLength)
            {
                throw new ArgumentException(string.Format("Username should be less than {0} characters", MaxNamLength));
            }
            else if (username.Any(ch => !ValidUsernameCharacters.Contains(ch)))
            {
                throw new ArgumentException(string.Format("Username should contains only these characters {0}", ValidUsernameCharacters));
            }
        }
    }
}
