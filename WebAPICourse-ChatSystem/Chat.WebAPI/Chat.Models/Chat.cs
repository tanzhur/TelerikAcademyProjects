using System.Collections.Generic;

namespace ChatSystem.Models
{
    public class Chat
    {
        public Chat()
        {
            this.Messages = new List<Message>();
            this.Participants = new HashSet<User>();
        }

        public int Id { get; set; }

        public virtual ICollection<User> Participants { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
    }
}