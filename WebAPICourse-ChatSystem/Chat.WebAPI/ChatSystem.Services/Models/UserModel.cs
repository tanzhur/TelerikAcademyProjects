using ChatSystem.Models;

namespace ChatSystem.Services.Models
{
    public class UserModel
    {
        public UserModel(int id, string username, string userStatus, string imageUrl)
        {
            this.Id = id;

            this.Username = username;
            this.UserStatus = userStatus;
            this.ImageUrl = imageUrl;
        }

        public int Id { get; set; }

        public string Username { get; set; }

        public string UserStatus { get; set; }

        public string ImageUrl { get; set; }

        public static UserModel ParseUser(User user)
        {
            return new UserModel(
                user.Id,
                user.Username,
                user.UserStatus,
                user.ImageUrl);
        }
    }
}