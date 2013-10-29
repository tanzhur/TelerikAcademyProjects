using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ForumApp.Models;
using System.Data.Entity;
namespace ForumApp
{
    public partial class Profile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        // The id parameter should match the DataKeyNames value set on the control
        // or be decorated with a value provider attribute, e.g. [QueryString]int id
        public ForumApp.Models.ApplicationUser FormViewProfileInformation_GetItem()
        {
            try
            {
                var userId = Request.Params["userId"];
                var context = new ApplicationDbContext();
                var user = context.Users.FirstOrDefault(u => u.Id == userId);
                if (user != null)
                {
                    return user;
                }
                else
                {
                    throw new ArgumentException("User not found");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string GetAnswers()
        {
            try
            {
                var userId = Request.Params["userId"];
                var context = new ApplicationDbContext();
                var user = context.Users.FirstOrDefault(u => u.Id == userId);
                if (user != null)
                {
                    var contextForum = new ForumEntities();
                    int answerCount = contextForum.Answers.Count(a => a.AuthorId == userId);
                    if (answerCount == 0)
                    {
                        return "No Questions";
                    }
                    else if (answerCount == 1)
                    {
                        return "1 answer";
                    }
                    else
                    {
                        return answerCount + " answers";
                    }
                }
                else
                {
                    throw new ArgumentException("User not found");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string GetQuestions()
        {
            try
            {
                var userId = Request.Params["userId"];
                var context = new ApplicationDbContext();
                var user = context.Users.FirstOrDefault(u => u.Id == userId);
                if (user != null)
                {
                    var contextForum = new ForumEntities();
                    int questionsCount = contextForum.Questions.Count(a => a.AuthorId == userId);
                    if (questionsCount == 0)
                    {
                        return "No Questions";
                    }
                    else if (questionsCount == 1)
                    {
                        return "1 question";
                    }
                    else
                    {
                        return questionsCount + " questions";
                    }
                }
                else
                {
                    throw new ArgumentException("User not found");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        // The return type can be changed to IEnumerable, however to support
        // paging and sorting, the following parameters must be added:
        //     int maximumRows
        //     int startRowIndex
        //     out int totalRowCount
        //     string sortByExpression
        public IQueryable<ForumApp.Models.Question> ListViewQuestionsByUser_GetData()
        {
            try
            {
                var userId = Request.Params["userId"];
                var context = new ApplicationDbContext();
                var user = context.Users.FirstOrDefault(u => u.Id == userId);
                if (user != null)
                {
                    var contextForum = new ForumEntities();
                    var activity = contextForum.Questions
                        .Include("Answers")
                        .Where(q => q.AuthorId == userId || q.Answers.Any(a => a.AuthorId == userId))
                        .OrderByDescending(q => q.PostedDate);

                    return activity;
                }
                else
                {
                    throw new ArgumentException("User not found");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string GetTimeDifference(DateTime posted)
        {
            string difference = (DateTime.Now - posted).Days >= 1 ? (DateTime.Now - posted).Days + " days"
                           : (DateTime.Now - posted).Hours < 1 ? (DateTime.Now - posted).Minutes + " minutes"
                           : (DateTime.Now - posted).Hours + " hours";

            return difference;
        }

        // The id parameter name should match the DataKeyNames value set on the control
        public void FormViewProfileInformation_UpdateItem(string id)
        {
            var context = new ApplicationDbContext();
            ForumApp.Models.ApplicationUser item = context.Users.FirstOrDefault(u => u.Id == id);
            // Load the item here, e.g. item = MyDataLayer.Find(id);
            if (item == null)
            {
                // The item wasn't found
                ModelState.AddModelError("", String.Format("Item with id {0} was not found", id));
                return;
            }

            TryUpdateModel(item);
            var existingUser = context.Users.FirstOrDefault(u => u.UserName == item.UserName);
            if (existingUser != null)
            {
                throw new ArgumentException("Username is already taken");
            }
            if (ModelState.IsValid)
            {
                // Save changes here, e.g. MyDataLayer.SaveChanges();
                context.Entry(item).State = EntityState.Modified;
                context.SaveChanges();
            }
        }
    }
}