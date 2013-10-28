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
    public class ImagesController : ApiController
    {
        private readonly IRepository<User> repository;

        public ImagesController(IRepository<User> repository)
        {
            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }

            this.repository = repository;
        }

        // POST api/images/?userid=1
        public string Post(int userId)
        {
            var httpRequest = HttpContext.Current.Request;

            if (httpRequest.Files.Count > 0)
            {
                string shareUrl = null;
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    shareUrl = Dropbox.UploadInDropbox(postedFile.FileName, postedFile.InputStream, postedFile.ContentLength, true);
                }

                var user = this.repository.Get(userId);
                user.ImageUrl = shareUrl;
                this.repository.Update(userId, user);
                return shareUrl;
            }
            else
            {
                throw new ArgumentException("File not uploaded.");
            }
        }
    }
}