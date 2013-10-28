using System.Collections.Generic;
using System.Linq;
using ChatSystem.Models;

namespace ChatSystem.Services.Models
{
    public class UserFullModel : UserModel
    {
        public UserFullModel(int id, string username, string userStatus, string imageUrl, ICollection<ChatModel> chatModels)
            : base(id, username, userStatus, imageUrl)
        {
            this.ChatModels = chatModels;
        }

        public ICollection<ChatModel> ChatModels { get; set; }

        public new static UserFullModel ParseUser(User user)
        {
            return new UserFullModel(
                user.Id,
                user.Username,
                user.UserStatus,
                user.ImageUrl,
                user.Chats.Select(c => ChatModel.ParseChat(c)).ToList());
        }
    }
}