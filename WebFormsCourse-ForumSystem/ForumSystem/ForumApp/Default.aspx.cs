using ForumApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Entity;

namespace ForumApp
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //using (var context = new ForumApp.Models.ForumEntities())
            //{
            //    var question = context.Questions.FirstOrDefault(q=> q.Id == 2);
            //    this.testLiteral.Text = question.Title;
            //}
        }

        // The return type can be changed to IEnumerable, however to support
        // paging and sorting, the following parameters must be added:
        //     int maximumRows
        //     int startRowIndex
        //     out int totalRowCount
        //     string sortByExpression
        public IQueryable<ForumApp.Models.Question> ListViewQuestions_GetData()
        {
            ForumEntities context = new ForumEntities();
            var questions = context.Questions.OrderByDescending(x => x.PostedDate);

            return questions;
        }

        public string GetTimeDifference(DateTime posted)
        {
            string difference = (DateTime.Now - posted).Days >= 1 ? (DateTime.Now - posted).Days + " days"
                           : (DateTime.Now - posted).Hours < 1 ? (DateTime.Now - posted).Minutes + " minutes"
                           : (DateTime.Now - posted).Hours + " hours";

            return difference;
        }
    }
}