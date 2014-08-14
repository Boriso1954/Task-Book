using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Data.Entity.Infrastructure.Annotations;
using System.Threading.Tasks;
using TaskBook.DomainModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskBook.DataAccessLayer.Configurations
{
    public class ProjectConfiguration: EntityTypeConfiguration<Project>
    {
        public ProjectConfiguration()
        {
            //Property(p=>p.Title).HasColumnAnnotation("ProjectNameIndex", new IndexAnnotation(new IndexAttribute("IX_ProjectName"
        //    HasMany(x => x.Users)
        //       .WithMany(x => x.Projects)
        //       .Map(m =>
        //       {
        //           m.ToTable("ProjectUsers");
        //           m.MapLeftKey("ProjectId");
        //           m.MapRightKey("UserId");
        //       });
        }
    }
}
