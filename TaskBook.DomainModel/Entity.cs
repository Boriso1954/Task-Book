using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBook.DomainModel
{
    /// <summary>
    /// Base class for domain models
    /// </summary>
    public class Entity
    {
        /// <summary>
        /// Model identifier - primary key
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// The version-stamping table rows
        /// </summary>
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
