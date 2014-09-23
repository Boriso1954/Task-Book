
namespace TaskBook.DomainModel.ViewModels
{
    public sealed class ResetPasswordVm
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Code { get; set; }
    }
}
