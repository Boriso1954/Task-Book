using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using TaskBook.DomainModel;

namespace TaskBook.DataAccessLayer.AuthManagers
{
    /// <summary>
    /// Provides role related api
    /// </summary>
    public class TbRoleManager: RoleManager<TbRole>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="roleStore">Role store</param>
        public TbRoleManager(IRoleStore<TbRole, string> roleStore)
            : base(roleStore)
        {
        }

        /// <summary>
        /// Creates the application role manager
        /// </summary>
        /// <param name="options">Configuration options</param>
        /// <param name="context">OWIN context</param>
        /// <returns></returns>
        public static TbRoleManager Create(IdentityFactoryOptions<TbRoleManager> options, IOwinContext context)
        {
            var roleManager = new TbRoleManager(new RoleStore<TbRole>(context.Get<TaskBookDbContext>()));
            return roleManager;
        }
    }
}
