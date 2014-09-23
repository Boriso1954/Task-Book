
namespace TaskBook.DomainModel
{
    /// <summary>
    /// Provides TaskBook task status options
    /// </summary>
    public enum TbTaskStatus: int
    {
        /// <summary>
        /// New task
        /// </summary>
        New = 0,

        /// <summary>
        /// Task in progress
        /// </summary>
        InProgress,

        /// <summary>
        /// Completed task
        /// </summary>
        Completed
    }
}
