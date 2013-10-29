using Error_Handler_Control;
using ForumApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ForumApp
{
    public partial class AskQuestion : System.Web.UI.Page
    {
        protected void Page_PreRender(object sender, EventArgs e)
        {
            ForumEntities context = new ForumEntities();

            this.DropDownListCategory.DataSource = context.Categories.ToList();
            this.DropDownListCategory.SelectedIndex = 0;
            this.DropDownListCategory.DataBind();
        }

        protected void ButtonAddQuestion_Click(object sender, EventArgs e)
        {
            string title = this.TextBoxQuestionTitle.Text;
            int categoryId = Convert.ToInt32(this.DropDownListCategory.SelectedValue);
            string content = this.TextBoxContent.Text;

            if (title.Length < 6)
            {
                ErrorSuccessNotifier.AddErrorMessage("Title length must be at least 6 chars!");
            }
            else if (content.Length < 10)
            {
                ErrorSuccessNotifier.AddErrorMessage("Content length must be at least 10 chars!");
            }
            else
            {
                try
                {
                    ForumEntities context = new ForumEntities();
                    AspNetUser author = context.AspNetUsers.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();

                    ForumApp.Models.Question addedQuestion = new ForumApp.Models.Question()
                    {
                        Title = title,
                        Content = content,
                        CategoryId = categoryId,
                        AuthorId = author.Id,
                        PostedDate = DateTime.Now
                    };

                    context.Questions.Add(addedQuestion);
                    context.SaveChanges();
                    ErrorSuccessNotifier.AddSuccessMessage("Question added successfuly!");
                    ErrorSuccessNotifier.ShowAfterRedirect = true;
                    Response.Redirect("~/Question?questionId=" + addedQuestion.Id);
                }
                catch (Exception ex)
                {
                    ErrorSuccessNotifier.AddErrorMessage("Server Error: " + ex.Message);

                }
            }
        }
    }
}