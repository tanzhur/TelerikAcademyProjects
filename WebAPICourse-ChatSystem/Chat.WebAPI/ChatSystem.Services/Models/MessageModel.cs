using System;
using ChatSystem.Models;

namespace ChatSystem.Services.Models
{
    public class MessageModel
    {
        public MessageModel(int id, UserModel sender, string content, DateTime time)
        {
            this.Id = id;
            this.Sender = sender;
            this.Content = content;
            this.Time = time;
        }

        public int Id { get; set; }

        public UserModel Sender { get; set; }

        public string Content { get; set; }

        public DateTime Time { get; set; }

        public static MessageModel ParseMessage(Message message)
        {
            return new MessageModel(
                message.Id,
                UserModel.ParseUser(message.Sender),
                message.Content,
                message.Time);
        }
    }
}