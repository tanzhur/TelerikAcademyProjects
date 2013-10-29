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
    public partial class Categories : System.Web.UI.Page
    {
        public bool IsAdminInRoleProp
        {
            get
            {
                return this.IsAdminInRole();
            }
        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            ForumEntities context = new ForumEntities();
            var categories = context.Categories.ToList();
            this.RepeaterCategories.DataSource = categories;
            this.RepeaterCategories.DataBind();

            this.AddCategoryPanel.Visible = this.IsAdminInRole();
        }

        public bool IsAdminInRole()
        {
            if (User.Identity.IsAuthenticated)
            {
                bool isAdmin = this.CheckAdminPermissions(User.Identity.Name);
                return isAdmin;
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
                    if (userAdmin != null)
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

        protected void LinkButtonAdd_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.NewCatTextBox.Text))
            {
                ErrorSuccessNotifier.AddErrorMessage("Please enter valid category name!");
            }
            else
            {
                string catName = this.NewCatTextBox.Text;
                var context = new ForumEntities();
                try
                {
                    context.Categories.Add(new Models.Category() { Title = catName });
                    context.SaveChanges();
                    this.NewCatTextBox.Text = null;
                    ErrorSuccessNotifier.AddSuccessMessage("Category added successfuly!");
                }
                catch (Exception ex)
                {
                    ErrorSuccessNotifier.AddErrorMessage("Server error: " + ex.Message);
                }
            }
        }

        // The return type can be changed to IEnumerable, however to support
        // paging and sorting, the following parameters must be added:
        //     int maximumRows
        //     int startRowIndex
        //     out int totalRowCount
        //     string sortByExpression
        public IQueryable<ForumApp.Models.Category> ListViewCategories_GetData()
        {
            ForumEntities context = new ForumEntities();
            var categories = context.Categories;
            return categories;
        }
    }
}