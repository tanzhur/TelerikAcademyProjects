using System;
using System.IO;
using DropNet;

namespace ChatSystem.Services.Models
{
    public class Dropbox
    {
        private const string DropboxAppKey = "ritksr5ck41bnv1";
        private const string DropboxAppSecret = "fj2x4dpbhei8qzm";
        private const string DropboxUserKey = "ep0elglw4b3m13ex";
        private const string DropboxUserSecret = "ad2xzh34rkkcu5a";

        public static string UploadInDropbox(string fileName, Stream stream, int fileSize, bool isUserPicture = false)
        {
            DropNetClient client = new DropNetClient(DropboxAppKey, DropboxAppSecret, DropboxUserKey, DropboxUserSecret);
            client.UseSandbox = true;

            var bytes = new byte[fileSize];
            stream.Read(bytes, 0, fileSize);

            var currentTicks = DateTime.Now.Ticks;
            client.UploadFile("/", currentTicks + fileName, bytes);

            if (isUserPicture)
            {
                return client.GetMedia(string.Format("/{0}", currentTicks + fileName)).Url;
            }
            else
            {
                return client.GetShare(string.Format("/{0}", currentTicks + fileName)).Url;
            }
        }
    }
}