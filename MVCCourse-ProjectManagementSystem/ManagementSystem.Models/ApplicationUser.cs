namespace ManagementSystem.Models
{
    using Microsoft.AspNet.Identity.EntityFramework;

    public class ApplicationUser : User
    {
        // TODO: Validation
        public string FullName { get; set; }

        public string Email { get; set; }

        public string AboutMe { get; set; }
    }
}