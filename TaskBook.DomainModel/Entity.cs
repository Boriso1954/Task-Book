using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskBook.DomainModel
{
    /// <summary>
    /// Base class for domain models
    /// </summary>
    public abstract class Entity
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
