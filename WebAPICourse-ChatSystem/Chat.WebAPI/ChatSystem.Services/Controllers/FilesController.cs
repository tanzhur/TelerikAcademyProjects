using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ChatSystem.Models;
using ChatSystem.Repositories;
using ChatSystem.Services.Models;

namespace ChatSystem.Services.Controllers
{
    public class FilesController : ApiController
    {
        private readonly IRepository<User> usersRepository;
        private readonly IRepository<SentFile> sentFilesRepository;

        public FilesController(IRepository<User> usersRepository, IRepository<SentFile> sentFilesRepository)
        {
            if (usersRepository == null)
            {
                throw new ArgumentNullException("usersRepository");
            }

            if (sentFilesRepository == null)
            {
                throw new ArgumentNullException("sentFilesRepository");
            }

            this.usersRepository = usersRepository;
            this.sentFilesRepository = sentFilesRepository;
        }

        // GET api/images/5
        public SentFileModel Get(int id)
        {
            return SentFileModel.ParseSentFile(this.sentFilesRepository.Get(id));
        }

        // POST api/images/?senderid=1&receiverid=3
        //public HttpResponseMessage Post(int senderId, int receiverId)
        //{
        //    HttpResponseMessage result = null;
        //    var httpRequest = HttpContext.Current.Request;

        //    if (httpRequest.Files.Count > 0)
        //    {
        //        string shareUrl = null;
        //        foreach (string file in httpRequest.Files)
        //        {
        //            var postedFile = httpRequest.Files[file];
        //            shareUrl = Dropbox.UploadInDropbox(postedFile.FileName, postedFile.InputStream, postedFile.ContentLength);
        //        }

        //        var sender = this.usersRepository.Get(senderId);
        //        var receiver = this.usersRepository.Get(receiverId);
        //        var sentFile = new SentFile
        //        {
        //            Sender = sender,
        //            Receiver = receiver,
        //            Url = shareUrl
        //        };

        //        this.sentFilesRepository.Add(sentFile);

        //        string jsonObject = "{ \"SentFileId\":\"" + sentFile.Id + "\"}";
        //        PubnubController.Push("SentFile", jsonObject);

        //        result = this.Request.CreateResponse(HttpStatusCode.Created);
        //    }
        //    else
        //    {
        //        result = this.Request.CreateResponse(HttpStatusCode.BadRequest);
        //    }

        //    return result;
        //}

        // POST api/files/5
        public string Post(int id)
        {
            var httpRequest = HttpContext.Current.Request;

            if (httpRequest.Files.Count > 0)
            {
                string shareUrl = null;
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    shareUrl = Dropbox.UploadInDropbox(postedFile.FileName, postedFile.InputStream, postedFile.ContentLength);
                }

                return shareUrl;
            }
            else
            {
                throw new ArgumentException("File not uploaded.");
            }
        }
    }
}