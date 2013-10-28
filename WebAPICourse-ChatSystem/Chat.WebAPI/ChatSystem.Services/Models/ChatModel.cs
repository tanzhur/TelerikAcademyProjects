using System.Collections.Generic;
using System.Linq;
using ChatSystem.Models;

namespace ChatSystem.Services.Models
{
    public class ChatModel
    {
        public ChatModel(int id, ICollection<UserModel> userModels)
        {
            this.Id = id;
            this.Participants = userModels;
        }

        public int Id { get; set; }

        public ICollection<UserModel> Participants { get; set; }

        public static ChatModel ParseChat(Chat chat)
        {
            return new ChatModel(chat.Id, chat.Participants.Select(p => UserModel.ParseUser(p)).ToList());
        }
    }
}