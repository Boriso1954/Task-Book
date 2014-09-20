using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security.OAuth;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using TaskBook.DataAccessLayer.AuthManagers;

namespace TaskBook.WebApi.Providers
{
    /// <summary>
    /// Creates and provides "access_token"
    /// </summary>
    public class SimpleAuthorizationServerProvider: OAuthAuthorizationServerProvider
    {
        /// <summary>
        /// Validates the input context
        /// </summary>
        /// <param name="context">The context information</param>
        /// <returns>Task to enable asynchronous execution</returns>
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            await Task.FromResult<object>(null);
        }

        /// <summary>
        /// Provides an "access_token" on user's login request
        /// </summary>
        /// <param name="context">The context information</param>
        /// <returns>Task to enable asynchronous execution of the "access_token" request</returns>
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            var userManager = context.OwinContext.GetUserManager<TbUserManager>();

            var user = await userManager.FindAsync(context.UserName, context.Password);
            if(user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);

            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));

            // Add roles as climes to the identity
            foreach(var role in userManager.GetRoles(user.Id))
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, role));
            }

            context.Validated(identity);
        }
    }
}