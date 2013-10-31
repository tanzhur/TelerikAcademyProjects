using Microsoft.AspNet.Identity.EntityFramework;

namespace ManagementSystem.Data
{
    using System;
    using ManagementSystem.Models;
    
    public interface IUowData : IDisposable
    {
        IRepository<Plan> Plans { get; }
        
        IRepository<Todo> Todos { get; }

        IRepository<ApplicationUser> AppUsers { get; }

        IRepository<Participant> Participants { get; }

        int SaveChanges();
    }
}
