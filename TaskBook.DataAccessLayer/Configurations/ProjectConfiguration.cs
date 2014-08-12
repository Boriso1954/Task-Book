using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBook.DomainModel;

namespace TaskBook.DataAccessLayer.Configurations
{
    public class ProjectConfiguration: EntityTypeConfiguration<Project>
    {
        //public ProjectConfiguration()
        //{
        //    HasMany(x => x.Users)
        //       .WithMany(x => x.Projects)
        //       .Map(m =>
        //       {
        //           m.ToTable("ProjectUsers");
        //           m.MapLeftKey("ProjectId");
        //           m.MapRightKey("UserId");
        //       });
        //}
    }
}
