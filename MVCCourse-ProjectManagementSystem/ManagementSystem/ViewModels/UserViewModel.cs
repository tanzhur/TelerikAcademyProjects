namespace ManagementSystem.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using ManagementSystem.Models;

    public class UserViewModel
    {
        public string Id { get; set; }

        public ICollection<Plan> MyPlans { get; set; }

        public ICollection<Plan> AllPlans { get; set; }

        public ProfileViewModel Profile { get; set; }
    }
}