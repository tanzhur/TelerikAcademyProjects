using System.Data.Entity;
using ChatSystem.Models;

namespace ChatSystem.Data
{
    public class ChatSystemContext : DbContext
    {
        public ChatSystemContext() : base("ChatSystem")
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Chat> Chats { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<SentFile> SentFiles { get; set; }
    }
}