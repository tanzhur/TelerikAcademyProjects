using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using ChatSystem.Data;
using ChatSystem.Models;
using ChatSystem.Repositories;
using ChatSystem.Services.Controllers;

namespace ChatSystem.Services.DependencyResolvers
{
    public class DbDependencyResolver : IDependencyResolver
    {
        public IDependencyScope BeginScope()
        {
            return this;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(UsersController))
            {
                return new UsersController(new EfRepository<User>(new ChatSystemContext()));
            }
            else if (serviceType == typeof(ChatsController))
            {
                var context = new ChatSystemContext();
                return new ChatsController(
                    new EfRepository<Chat>(context),
                    new EfRepository<User>(context));
            }
            else if (serviceType == typeof(ImagesController))
            {
                return new ImagesController(new EfRepository<User>(new ChatSystemContext()));
            }
            else if (serviceType == typeof(FilesController))
            {
                var context = new ChatSystemContext();
                return new FilesController(
                    new EfRepository<User>(context),
                    new EfRepository<SentFile>(context));
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return new List<object>();
        }

        public void Dispose()
        {
        }
    }
}