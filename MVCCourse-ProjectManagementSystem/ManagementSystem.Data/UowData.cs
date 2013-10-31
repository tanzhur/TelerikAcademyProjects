namespace ManagementSystem.Data
{
    using System;
    using System.Collections.Generic;
    using ManagementSystem.Models;
    using Microsoft.AspNet.Identity.EntityFramework;
    
    public class UowData : IUowData
    {
        private readonly IDataContext context;
        private readonly Dictionary<Type, object> repositories = new Dictionary<Type, object>();

        public UowData() 
            :this(new DataContext())
        { }

        public UowData(IDataContext context)
        {
            this.context = context;
        }

        public IRepository<Plan> Plans
        {
            get
            {
                return this.GetRepository<Plan>();
            }
        }

        public IRepository<User> Users
        {
            get
            {
                return this.GetRepository<User>();
            }
        }

        public IRepository<ApplicationUser> AppUsers
        {
            get
            {
                return this.GetRepository<ApplicationUser>();
            }
        }

        public IRepository<Todo> Todos
        {
            get
            {
                return this.GetRepository<Todo>();
            }
        }

        public IRepository<Participant> Participants
        {
            get
            {
                return this.GetRepository<Participant>();
            }
        }

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }

        public void Dispose()
        {
            if (this.context != null)
            {
                this.context.Dispose();
            }
        }

        private IRepository<T> GetRepository<T>() where T : class
        {
            if (!this.repositories.ContainsKey(typeof(T)))
            {
                var type = typeof(GenericRepository<T>);

                this.repositories.Add(typeof(T), Activator.CreateInstance(type, this.context));
            }

            return (IRepository<T>)this.repositories[typeof(T)];
        }
    }
}
