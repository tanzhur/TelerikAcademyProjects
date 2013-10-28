using ChatSystem.Models;
using ChatSystem.Repositories;
using ChatSystem.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ChatSystem.Services.Controllers
{
    public class ChatsController : ApiController
    {
        private readonly IRepository<Chat> chatsRepository;
        private readonly IRepository<User> usersRepository;

        public ChatsController(IRepository<Chat> chatsRepository, IRepository<User> usersRepository)
        {
            if (chatsRepository == null)
            {
                throw new ArgumentNullException("chatsRepository");
            }

            if (usersRepository == null)
            {
                throw new ArgumentNullException("usersRepository");
            }

            this.chatsRepository = chatsRepository;
            this.usersRepository = usersRepository;
        }

        // GET api/chats
        public IEnumerable<ChatModel> Get()
        {
            var chats = this.chatsRepository.All().ToList();
            var models = chats.Select(c => ChatModel.ParseChat(c));
            return models;
        }

        // GET api/chats/5
        public ChatFullModel Get(int id, int messageID = -1)
        {
            var chat = this.chatsRepository.Get(id);
            var model = ChatFullModel.ParseChat(chat, messageID);
            return model;
        }

        // POST api/chats
        public ChatModel Post([FromBody]ChatModel chatModel)
        {
            var allChats = chatsRepository.All();
            foreach (var user in chatModel.Participants)
            {
                allChats = allChats.Where(c => c.Participants.Select(p => p.Id).Contains(user.Id));
            }

            var existingChat = allChats.Where(c => c.Participants.Count == chatModel.Participants.Count).FirstOrDefault();

            if (existingChat == null)
            {
                Chat currentChat = new Chat();
                foreach (var user in chatModel.Participants)
                {
                    var currentUser = this.usersRepository.Get(user.Id);
                    currentChat.Participants.Add(currentUser);
                }
                chatsRepository.Add(currentChat);
               
                return ChatModel.ParseChat(currentChat);
            }

            return ChatModel.ParseChat(existingChat);
        }

        // POST api/chats/id
        public MessageModel Post(int id, [FromBody]MessageModel message)
        {
            var chat = this.chatsRepository.Get(id);
            Message currentMessage = new Message();
            
            currentMessage.Sender = this.usersRepository.Get(message.Sender.Id);
            if (currentMessage.Sender == null)
            { 
                throw new Exception(string.Format("User with ID's = {0} not found", message.Sender.Id));
            }

            currentMessage.Content = message.Content;
            currentMessage.Time = message.Time;

            chat.Messages.Add(currentMessage);
            this.chatsRepository.Update(id, chat);

            foreach (var user in chat.Participants)
            {
                if (user.Username != currentMessage.Sender.Username)
                {
                    string jsonObject = "{ \"ChatId\":\"" + id + "\", \"MessageId\":\"" + currentMessage.Id + "\",\"UserId\":\"" + currentMessage.Sender.Id + "\" }";
                    PubnubController.Push(user.Username, jsonObject);
                }
            }

            return MessageModel.ParseMessage(currentMessage);
        }
    }
}
