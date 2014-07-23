using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBook.DomainModel;

namespace TaskBook.DataAccessLayer.Configurations
{
    public class UserConfiguration: EntityTypeConfiguration<TbUser>
    {
        //public UserConfiguration()
        //{
        //    HasOptional(u => u.Project)
        //           .WithMany()
        //           .HasForeignKey(u => u.ProjectId)
        //           .WillCascadeOnDelete(false);
        //}
    }
}
