using ForumApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ForumApp
{
    public partial class Search : System.Web.UI.Page
    {
        protected void Page_PreRender(object sender, EventArgs e)
        {
            this.LiteralParam.Text = Request.Params["param"];


        }

        // The return type can be changed to IEnumerable, however to support
        // paging and sorting, the following parameters must be added:
        //     int maximumRows
        //     int startRowIndex
        //     out int totalRowCount
        //     string sortByExpression
        public IQueryable<ForumApp.Models.Question> ListViewQuestionsByParam_GetData()
        {
            string param = Request.Params["param"];
            var context = new ForumEntities();
            if (String.IsNullOrEmpty(param))
            {
                return null;
            }
            else 
            {
                
                var filteredQuestions = context.Questions.Where(x => x.Title.Contains(param));

                return filteredQuestions.OrderBy(x => x.PostedDate);
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
    }
}