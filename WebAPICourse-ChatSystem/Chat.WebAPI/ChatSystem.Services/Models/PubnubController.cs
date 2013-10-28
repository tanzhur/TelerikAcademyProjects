namespace ChatSystem.Services.Models
{
    public class PubnubController
    {
        private static readonly PubnubAPI pubnub = new PubnubAPI(
            "pub-c-c6143d17-028a-4aa0-92b0-1ace832b0790",               // PUBLISH_KEY
            "sub-c-c9c104c8-3fb5-11e3-83cf-02ee2ddab7fe",               // SUBSCRIBE_KEY
            "sec-c-NzNmOTQ1ZjItODM0ZS00MDkyLTg0OTgtM2U0ZDliYTgzY2Fj",   // SECRET_KEY
            true);

        public static void Push(string channel, string jsonObject) 
        {
            pubnub.Publish(channel, jsonObject);
        }
    }
}