using ChatSystem.Models;

namespace ChatSystem.Services.Models
{
    public class SentFileModel
    {
        public SentFileModel(int id, UserModel sender, UserModel receiver, string url)
        {
            this.Id = id;
            this.Sender = sender;
            this.Receiver = receiver;
            this.Url = url;
        }

        public int Id { get; set; }

        public UserModel Sender { get; set; }

        public UserModel Receiver { get; set; }

        public string Url { get; set; }

        public static SentFileModel ParseSentFile(SentFile sentFile)
        {
            return new SentFileModel(
                sentFile.Id,
                UserModel.ParseUser(sentFile.Sender),
                UserModel.ParseUser(sentFile.Receiver),
                sentFile.Url);
        }
    }
}