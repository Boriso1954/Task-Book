using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBook.DomainModel;

namespace TaskBook.DataAccessLayer.Configurations
{
    public class ProjectUsersConfiguration: EntityTypeConfiguration<ProjectUsers>
    {

        public ProjectUsersConfiguration()
        {
            HasRequired(x => x.Project)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.ProjectId);

            HasRequired(x => x.User)
                .WithMany(x => x.Projects)
                .HasForeignKey(x => x.UserId);
        }
    }
}
