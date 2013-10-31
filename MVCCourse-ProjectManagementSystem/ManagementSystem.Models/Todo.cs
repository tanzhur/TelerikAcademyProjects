namespace ManagementSystem.Models
{
    using System;

    public class Todo
    {
        // TODO: Validation
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public Priority Priority { get; set; }

        public State State { get; set; }

        public DateTime DateCreated { get; set; }

        public virtual Plan Plan { get; set; }
    }
}
