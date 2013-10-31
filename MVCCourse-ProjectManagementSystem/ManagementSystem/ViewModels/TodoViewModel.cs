namespace ManagementSystem.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using ManagementSystem.Models;

    public class TodoViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public Priority Priority { get; set; }

        [Required]
        public State State { get; set; }

        [Required]
        public int PlanId { get; set; }
    }
}