
namespace TaskBook.DomainModel
{
    public static class Extensions
    {
        public static string ToTbString(this TbTaskStatus status)
        {
            string result = string.Empty;
            switch(status)
            {
                case TbTaskStatus.New:
                    result = "New";
                    break;
                case TbTaskStatus.InProgress:
                    result = "In Progress";
                    break;
                case TbTaskStatus.Completed:
                    result = "Completed";
                    break;
            }
            return result;
        }
    }
}
