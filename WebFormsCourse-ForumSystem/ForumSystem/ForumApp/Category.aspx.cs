using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ForumApp.Models;
using System.Text.RegularExpressions;

namespace ForumApp
{
    public partial class Category : System.Web.UI.Page
    {
        protected void Page_PreRender(object sender, EventArgs e)
        {
            try
            {
                var categoryId = int.Parse(Request.Params["categoryId"]);
                var context = new ForumEntities();
                var category = context.Categories.FirstOrDefault(c => c.Id == categoryId);
                if (category != null)
                {
                    this.LiteralCategoryTitle.Text = "Recent questions in "+category.Title;
                    
                }
                else
                {
                    throw new ArgumentException("Category not found");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IQueryable<ForumApp.Models.Question> ListViewQuestions_GetData()
        {
            try
            {
                var categoryId = int.Parse(Request.Params["categoryId"]);
                var context = new ForumEntities();
                var category = context.Categories.FirstOrDefault(c => c.Id == categoryId);
                if (category != null)
                {
                    var questions = category.Questions.OrderByDescending(x => x.PostedDate);

                    return questions.AsQueryable();

                }
                else
                {
                    throw new ArgumentException("Category not found");
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

       
    }
}