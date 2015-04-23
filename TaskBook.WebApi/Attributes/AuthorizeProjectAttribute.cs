using Microsoft.AspNet.Identity.Owin;
using System;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

using TaskBook.DataAccessLayer;
using TaskBook.DataAccessLayer.AuthManagers;
using TaskBook.Services;

namespace TaskBook.WebApi.Attributes
{
    public class AuthorizeProjectAttribute: AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            bool authenticated = false;
            var userName = HttpContext.Current.User.Identity.Name;
            var id = actionContext.RequestContext.RouteData.Values["projectId"];
            long projectId = Convert.ToInt64(id);

            var db = TaskBookDbContext.Create();
            var userManager = HttpContext.Current.GetOwinContext().GetUserManager<TbUserManager>();
            using (var uow = new UnitOfWork(db))
            using (var userService = new UserService(uow, userManager))
            {
                var users = userService.GetUsersWithRolesByProjectId(projectId).ToList();
                authenticated = users.Any(x => x.UserName == userName);
            }
            
            return authenticated;
        }
    }
}