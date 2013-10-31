namespace ManagementSystem.Data
{
    using System;
    using System.Data.Entity;
    using ManagementSystem.Models;
using Microsoft.AspNet.Identity.EntityFramework;
    
    public interface IDataContext : IDisposable
    {
        IDbSet<Plan> Plans { get; set; }
        IDbSet<Todo> Todos { get; set; }
        IDbSet<User> AppUsers { get; set; }
        IDbSet<Participant> Participants { get; set; }

        int SaveChanges();
    }
}
