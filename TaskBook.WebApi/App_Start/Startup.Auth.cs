using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using TaskBook.DataAccessLayer;
using TaskBook.DataAccessLayer.AuthManagers;
using TaskBook.DataAccessLayer.Repositories;
using TaskBook.DomainModel;
using TaskBook.WebApi.Providers;

namespace TaskBook.WebApi
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db auth context, user manager and role manager to use a single instance per request
            app.CreatePerOwinContext(AuthDbContext.Create);
            app.CreatePerOwinContext<TbUserManager>(TbUserManager.Create);
            app.CreatePerOwinContext<TbRoleManager>(TbRoleManager.Create);

            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/Token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new SimpleAuthorizationServerProvider()
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}