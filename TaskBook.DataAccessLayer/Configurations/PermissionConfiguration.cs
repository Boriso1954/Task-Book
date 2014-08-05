using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBook.DomainModel;

namespace TaskBook.DataAccessLayer.Configurations
{
    public class PermissionConfiguration: EntityTypeConfiguration<Permission>
    {
        public PermissionConfiguration()
        {
            HasMany(x => x.Roles)
               .WithMany(x => x.Permissions)
               .Map(m =>
               {
                   m.ToTable("PermissionRoles");
                   m.MapLeftKey("PermissionId");
                   m.MapRightKey("RoleID");
               });
        }
    }
}
