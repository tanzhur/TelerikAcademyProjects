namespace ManagementSystem.Models
{
    using System.Collections.Generic;

    public class Participant
    {
        public Participant()
        {
            this.Plans = new HashSet<Plan>();
            this.Todos = new HashSet<Todo>();
        }

        public string Id { get; set; }

        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual ICollection<Plan> Plans { get; set; }

        public virtual ICollection<Todo> Todos { get; set; }
    }
}
