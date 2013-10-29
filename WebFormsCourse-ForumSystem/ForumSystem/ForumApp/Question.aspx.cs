using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ForumApp.Models;
using System.Text.RegularExpressions;
using Error_Handler_Control;

namespace ForumApp
{
    public partial class Question : System.Web.UI.Page
    {
        protected void Page_PreRender(object sender, EventArgs e)
        {
            var answersLayoutTemplate = (this.ListViewAnswers.FindControl("LinkButtonAnswer") as LinkButton);
            if (answersLayoutTemplate != null)
            {
                answersLayoutTemplate.Visible = GetUserButtons();
            }
            else
            {
                var answersEmptyDataTemplate = (this.ListViewAnswers.Controls[0].FindControl("LinkButtonAnswer") as LinkButton);
                if (answersEmptyDataTemplate != null)
                {
                    answersEmptyDataTemplate.Visible = GetUserButtons();
                }
            }
        }


        // The return type can be changed to IEnumerable, however to support
        // paging and sorting, the following parameters must be added:
        //     int maximumRows
        //     int startRowIndex
        //     out int totalRowCount
        //     string sortByExpression
        public IQueryable<ForumApp.Models.Answer> ListViewAnswers_GetData()
        {
            try
            {
                var questionId = int.Parse(Request.Params["questionId"]);
                var context = new ForumEntities();
                var question = context.Questions.Include("Answers").FirstOrDefault(q => q.Id == questionId);
                if (question != null)
                {
                    return question.Answers.AsQueryable();
                }
                else
                {
                    throw new ArgumentException("Question not found");
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

        public bool GetUserButtons()
        {
            if (User.Identity.IsAuthenticated)
            {
                return true;
            }
            return false;
        }

        public bool GetEditButton(string username)
        {
            if (User.Identity.IsAuthenticated)
            {
                string currentUsername = User.Identity.Name;
                bool isAdmin = CheckAdminPermissions(currentUsername);
                if (currentUsername == username || isAdmin)
                {
                    return true;    
                }
            }
            return false;
        }

        private bool CheckAdminPermissions(string currentUsername)
        {
            using (var context = new ForumEntities())
            {
                var user = context.AspNetUsers.FirstOrDefault(u => u.UserName == currentUsername);
                if (user != null)
                {
                    var userAdmin = user.AspNetRoles.FirstOrDefault(r => r.Name == "Administrator");
                    if (userAdmin!=null)
                    {
                        return true;    
                    }
                    else
                    {
                        return false;
                    }
                    
                }
                else
                {
                    return false;
                }
            }
        }

        public string MatchURLS(string txt)
        {
            string result = txt;
            Regex regx =
                new Regex("http://([\\w+?\\.\\w+])+([a-zA-Z0-9\\~\\!\\@\\#\\$\\%\\^\\&amp;\\*\\(\\)_\\-\\=\\+\\\\\\/\\?\\.\\:\\;\\'\\,]*)?",
                RegexOptions.IgnoreCase);
            MatchCollection mactches = regx.Matches(result);
            foreach (Match match in mactches)
            {
                result = result.Replace(match.Value, "<a href='" + match.Value + "'>" + match.Value + "</a>");
            }

            regx =
                new Regex("https://([\\w+?\\.\\w+])+([a-zA-Z0-9\\~\\!\\@\\#\\$\\%\\^\\&amp;\\*\\(\\)_\\-\\=\\+\\\\\\/\\?\\.\\:\\;\\'\\,]*)?",
                RegexOptions.IgnoreCase);
            mactches = regx.Matches(result);
            foreach (Match match in mactches)
            {
                result = result.Replace(match.Value, "<a href='" + match.Value + "'>" + match.Value + "</a>");
            }

            regx =
                new Regex("ftp://([\\w+?\\.\\w+])+([a-zA-Z0-9\\~\\!\\@\\#\\$\\%\\^\\&amp;\\*\\(\\)_\\-\\=\\+\\\\\\/\\?\\.\\:\\;\\'\\,]*)?",
                RegexOptions.IgnoreCase);
            mactches = regx.Matches(result);
            foreach (Match match in mactches)
            {
                result = result.Replace(match.Value, "<a href='" + match.Value + "'>" + match.Value + "</a>");
            }

            return result;
        }

        protected void ButtonAddAnswer_Click(object sender, EventArgs e)
        {
            this.ListViewAnswers.InsertItemPosition = InsertItemPosition.FirstItem;
           
        }

        protected void LinkButtonAnswer_Click(object sender, EventArgs e)
        {
            this.ListViewAnswers.InsertItemPosition = InsertItemPosition.FirstItem;
        }

        public void ListViewAnswers_InsertItem()
        {
            var answer = new ForumApp.Models.Answer();
            TryUpdateModel(answer);
            if (ModelState.IsValid)
            {
                try
                {
                    var questionId = int.Parse(Request.Params["questionId"]);
                    var context = new ForumEntities();
                    var question = context.Questions.FirstOrDefault(q => q.Id == questionId);
                    if (question != null)
                    {
                        var user = context.AspNetUsers.FirstOrDefault(u => u.UserName == User.Identity.Name);
                        if (user != null)
                        {
                            answer.AspNetUser = user;
                            answer.PostedDate = DateTime.Now;

                            question.Answers.Add(answer);
                            context.SaveChanges();
                            this.ListViewAnswers.InsertItemPosition = InsertItemPosition.None;
                            ErrorSuccessNotifier.AddSuccessMessage("Ansew added successfuly!");
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Question not found");
                    }
                }
                catch (Exception ex)
                {
                    ErrorSuccessNotifier.AddErrorMessage(ex.Message);
                }

            }
        }

        protected void ListViewAnswers_ItemCanceling(object sender, ListViewCancelEventArgs e)
        {
            this.ListViewAnswers.InsertItemPosition = InsertItemPosition.None;
        }

        // The id parameter name should match the DataKeyNames value set on the control
        public void ListViewAnswers_UpdateItem(int id)
        {
            var context = new ForumEntities();
            ForumApp.Models.Answer item = context.Answers.FirstOrDefault(a=> a.Id==id);
            // Load the item here, e.g. item = MyDataLayer.Find(id);
            if (item == null)
            {
                // The item wasn't found
                ModelState.AddModelError("", String.Format("Item with id {0} was not found", id));
                return;
            }
            TryUpdateModel(item);
            if (ModelState.IsValid)
            {
                context.Entry(item).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();

            }
        }

        // The id parameter name should match the DataKeyNames value set on the control
        public void FormViewQuestion_UpdateItem(int id)
        {
           

            try
            {
                var context = new ForumEntities();
                ForumApp.Models.Question item = context.Questions.FirstOrDefault(a => a.Id == id);
                // Load the item here, e.g. item = MyDataLayer.Find(id);
                if (item == null)
                {
                    // The item wasn't found
                    ModelState.AddModelError("", String.Format("Item with id {0} was not found", id));
                    return;
                }
                TryUpdateModel(item);
                if (ModelState.IsValid)
                {
                    // Save changes here, e.g. MyDataLayer.SaveChanges();
                    context.Entry(item).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                }
                ErrorSuccessNotifier.AddInfoMessage("Question successfuly edited!");
            }
            catch (Exception ex)
            {
                ErrorSuccessNotifier.AddErrorMessage("Server Error: " + ex.Message);
            }
        }

        // The id parameter should match the DataKeyNames value set on the control
        // or be decorated with a value provider attribute, e.g. [QueryString]int id
        public ForumApp.Models.Question FormViewQuestion_GetItem()
        {
            try
            {
                var questionId = int.Parse(Request.Params["questionId"]);
                var context = new ForumEntities();
                var question = context.Questions.FirstOrDefault(q => q.Id == questionId);
                if (question != null)
                {
                    return question;
                }
                else
                {
                    throw new ArgumentException("Question not found");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        // The id parameter name should match the DataKeyNames value set on the control
        public void ListViewAnswers_DeleteItem(int id)
        {
            try
            {
                var questionId = int.Parse(Request.Params["questionId"]);
                var context = new ForumEntities();
                var question = context.Questions.FirstOrDefault(q => q.Id == questionId);
                if (question != null)
                {

                    var answer = context.Answers.FirstOrDefault(a => a.Id == id);
                    if (answer != null)
                    {
                        question.Answers.Remove(answer);
                        context.Answers.Remove(answer);
                        context.SaveChanges();
                        ErrorSuccessNotifier.AddInfoMessage("Answer successfuly deleted");
                    }
                    else
                    {
                        throw new ArgumentException("Answer not found");
                    }
                }
                else
                {
                    throw new ArgumentException("Question not found");
                }
            }
            catch (Exception)
            {   
                throw;
            }
        }   
    }
}