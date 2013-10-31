using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ManagementSystem.Models;

namespace ManagementSystem.Data
{

    public interface IRepository<T> where T : class
    {
        IQueryable<T> All();

        T Find(int id);

        void Add(T entity);

        void Update(T entity);

        void Delete(T entity);

        void Delete(int id);

        void Detach(T entity);
    }
}
