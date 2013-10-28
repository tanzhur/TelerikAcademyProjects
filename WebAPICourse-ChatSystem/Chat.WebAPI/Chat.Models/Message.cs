using System;

namespace ChatSystem.Models
{
    public class Message
    {
        public int Id { get; set; }

        public virtual User Sender { get; set; }

        public string Content { get; set; }

        public DateTime Time { get; set; }
    }
}