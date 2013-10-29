using ForumApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ForumApp.Admin
{
    public partial class EditCategory : System.Web.UI.Page
    {

        public int Id { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Id = Convert.ToInt32(this.Request.Params["id"]);

        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            ForumEntities context = new ForumEntities();
            int id = Id;
            this.CatNameTb.Text = context.Categories.Find(id).Title;
        }

        protected void Save_Click(object sender, EventArgs e)
        {
            try
            {
                ForumEntities context = new ForumEntities();
                var currentCat = context.Categories.Find(Id);
                currentCat.Title = this.CatNameTb.Text;
                context.SaveChanges();

                Response.Redirect("~/Categories.aspx", false);
                Error_Handler_Control.ErrorSuccessNotifier.AddSuccessMessage("Successfuly edited");
                Error_Handler_Control.ErrorSuccessNotifier.ShowAfterRedirect = true;
            }
            catch (Exception ex)
            {
                Error_Handler_Control.ErrorSuccessNotifier.AddErrorMessage("Error: " + ex.Message);
            }
        }
    }
}