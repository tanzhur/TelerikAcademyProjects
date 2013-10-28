namespace ChatSystem.Models
{
    public class SentFile
    {
        public int Id { get; set; }

        public virtual User Sender { get; set; }

        public virtual User Receiver { get; set; }

        public string Url { get; set; }
    }
}