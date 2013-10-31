namespace ManagementSystem.Models
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class Plan
    {
        public Plan()
        {
            this.Participants = new HashSet<Participant>();
            this.Todos = new HashSet<Todo>();
        }

        // TODO: Validation
        public int Id { get; set; }

        [AllowHtml]
        public string Title { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public virtual ApplicationUser Owner { get; set; }

        public virtual ICollection<Participant> Participants { get; set; }

        public virtual ICollection<Todo> Todos { get; set; }
    }
}
