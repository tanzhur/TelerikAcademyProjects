using System;
using System.Collections.Generic;
using System.Linq;
using ChatSystem.Models;

namespace ChatSystem.Services.Models
{
    public class ChatFullModel : ChatModel
    {
        public ChatFullModel(int id, ICollection<UserModel> userModels, ICollection<MessageModel> messages)
            : base(id, userModels)
        {
            this.Messages = messages;
        }

        public ICollection<MessageModel> Messages { get; set; }

        public new static ChatFullModel ParseChat(Chat chat, int messageId)
        {
            ICollection<MessageModel> messages;
            if (messageId > -1)
            {
                messages = chat.Messages.Where(m => m.Id == messageId).Select(m => MessageModel.ParseMessage(m)).ToList();
            }
            else
            {
                messages = chat.Messages.Skip(Math.Max(chat.Messages.Count() - 10, 0)).Select(m => MessageModel.ParseMessage(m)).ToList();
            }

            return new ChatFullModel(
                chat.Id,
                chat.Participants.Select(p => UserModel.ParseUser(p)).ToList(),
                messages);
        }
    }
}