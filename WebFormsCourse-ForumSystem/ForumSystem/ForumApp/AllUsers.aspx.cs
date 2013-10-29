using ForumApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ForumApp
{
    public partial class AllUsers : System.Web.UI.Page
    {
        protected void Page_PreRender(object sender, EventArgs e)
        {
            
        }

        // The return type can be changed to IEnumerable, however to support
        // paging and sorting, the following parameters must be added:
        //     int maximumRows
        //     int startRowIndex
        //     out int totalRowCount
        //     string sortByExpression
        public IQueryable<ForumApp.Models.ApplicationUser> ListViewUsers_GetData()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            return context.Users.OrderBy(x => x.UserName);
            
        }
    }
}