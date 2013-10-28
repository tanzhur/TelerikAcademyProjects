using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ChatSystem.Models;
using ChatSystem.Repositories;
using ChatSystem.Services.Models;

namespace ChatSystem.Services.Controllers
{
    public class UsersController : ApiController
    {
        private const string OnlineStatus = "Online";
        private const string OfflineStatus = "Offline";

        private readonly IRepository<User> repository;

        public UsersController(IRepository<User> repository)
        {
            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }

            this.repository = repository;
        }

        // GET api/users
        public IEnumerable<UserModel> Get()
        {
            var users = this.repository.All().ToList();
            var models = users.Where(u => u.UserStatus == OnlineStatus).Select(u => UserModel.ParseUser(u));
            return models;
        }

        // GET api/users/5
        public UserModel Get(int id)
        {
            var user = this.repository.Get(id);
            var model = UserModel.ParseUser(user);
            return model;
        }

        // GET api/users/?username=pesho
        public UserFullModel Get(string username)
        {
            var user = this.repository
                           .All()
                           .Where(u => u.Username == username)
                           .FirstOrDefault();

            if (user == null)
            {
                user = new User
                {
                    Username = username
                };

                this.repository.Add(user);
            }

            user.UserStatus = OnlineStatus;
            this.repository.Update(user.Id, user);
            string jsonObject = "{ \"UserId\":\"" + user.Id + "\", \"Username\":\"" + user.Username + "\", \"Status\":\"" + user.UserStatus + "\" }";
            PubnubController.Push("UserStatusChanged", jsonObject);
            return UserFullModel.ParseUser(user);
        }

        // PUT api/users/5
        public void Put(int id)
        {
            var user = this.repository.Get(id);
            user.UserStatus = OfflineStatus;
            this.repository.Update(id, user);
            string jsonObject = "{ \"UserId\":\"" + user.Id + "\", \"Username\":\"" + user.Username + "\", \"Status\":\"" + user.UserStatus + "\" }";
            PubnubController.Push("UserStatusChanged", jsonObject);
        }
    }
}









