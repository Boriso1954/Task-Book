
namespace TaskBook.DomainModel.ViewModels
{
    public sealed class TaskUserVm
    {
        public long TaskId { get; set; }
        public string Title { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}
