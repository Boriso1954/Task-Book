using System;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using TaskBook.DataAccessLayer;
using TaskBook.DataAccessLayer.AuthManagers;
using TaskBook.WebApi.Providers;

namespace TaskBook.WebApi
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db auth context, user manager and role manager to use a single instance per request
            app.CreatePerOwinContext(TaskBookDbContext.Create);
            app.CreatePerOwinContext<TbUserManager>(TbUserManager.Create);
            app.CreatePerOwinContext<TbRoleManager>(TbRoleManager.Create);

            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/Token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(30),
                Provider = new SimpleAuthorizationServerProvider()
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}