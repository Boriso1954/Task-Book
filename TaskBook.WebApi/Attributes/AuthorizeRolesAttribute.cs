using System.Web.Http;

namespace TaskBook.WebApi.Attributes
{
    /// <summary>
    /// Allows using RoleKey class in authorize attribute
    /// </summary>
    public sealed class AuthorizeRolesAttribute: AuthorizeAttribute
    {
        public AuthorizeRolesAttribute(params string[] roles)
        {
            Roles = string.Join(",", roles);
        }
    }
}