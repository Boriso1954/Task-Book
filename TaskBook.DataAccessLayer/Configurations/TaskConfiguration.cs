using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBook.DomainModel;

namespace TaskBook.DataAccessLayer.Configurations
{
    public class TaskConfiguration: EntityTypeConfiguration<TbTask>
    {
        public TaskConfiguration()
        {
            //HasOptional(t => t.AssignedTo)
            //    .WithMany()
            //    .HasForeignKey(t => t.AssignedToId)
            //    .WillCascadeOnDelete(false);

            //HasOptional(t => t.CreatedBy)
            //    .WithMany()
            //    .HasForeignKey(t => t.CreatedById)
            //    .WillCascadeOnDelete(false);
        }
    }
}
