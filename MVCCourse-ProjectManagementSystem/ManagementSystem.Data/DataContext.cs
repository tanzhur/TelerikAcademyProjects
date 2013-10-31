namespace ManagementSystem.Data
{
    using System.Data.Entity;
    using ManagementSystem.Models;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class DataContext : IdentityDbContextWithCustomUser<ApplicationUser>, IDataContext
    {
        public IDbSet<Plan> Plans { get; set; }
        public IDbSet<Todo> Todos { get; set; }
        public IDbSet<User> AppUsers { get; set; }
        public IDbSet<Participant> Participants { get; set; }
    }
}
