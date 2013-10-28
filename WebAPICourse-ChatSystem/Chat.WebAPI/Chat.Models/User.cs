using System.Collections.Generic;

namespace ChatSystem.Models
{
    public class User
    {
        public User()
        {
            this.Chats = new HashSet<Chat>();
        }

        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string ImageUrl { get; set; }

        public virtual string UserStatus { get; set; }

        public virtual ICollection<Chat> Chats { get; set; }
    }
}