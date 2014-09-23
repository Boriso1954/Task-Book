using System.Data.Entity.ModelConfiguration;
using TaskBook.DomainModel;

namespace TaskBook.DataAccessLayer.Configurations
{
    /// <summary>
    /// Describes ProjectUsers model configuration
    /// </summary>
    public sealed class ProjectUsersConfiguration: EntityTypeConfiguration<ProjectUsers>
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
