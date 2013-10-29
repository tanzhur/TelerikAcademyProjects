using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.ModelBinding;

namespace ForumApp
{
    public partial class Open : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        // The return type can be changed to IEnumerable, however to support
        // paging and sorting, the following parameters must be added:
        //     int maximumRows
        //     int startRowIndex
        //     out int totalRowCount
        //     string sortByExpression
        public IQueryable<ForumApp.Models.Question> ListViewOpenedQuestions_GetData([Control] string byUser)
        {
            ForumApp.Models.ForumEntities context = new Models.ForumEntities();
            var questions = context.Questions.Where(x => x.Answers.Count == 0).OrderBy(x => x.PostedDate);
            if (byUser != null)
            {
                questions = questions.Where(x => x.AspNetUser.UserName == byUser).OrderBy(x => x.PostedDate);
            }

            return questions;
        }

        public string GetTimeDifference(DateTime posted)
        {
            string difference = (DateTime.Now - posted).Days >= 1 ? (DateTime.Now - posted).Days + " days"
                                : (DateTime.Now - posted).Hours < 1 ? (DateTime.Now - posted).Minutes + " minutes"
                                  : (DateTime.Now - posted).Hours + " hours";

            return difference;
        }

        public string GetAnswersCount(int answersCount)
        {
            if (answersCount == 1)
            {
                return "1 Answer";
            }

            return String.Format("{0} Answers", answersCount);
        }
    }
}