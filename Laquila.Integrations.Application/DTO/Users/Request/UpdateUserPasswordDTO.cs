
namespace Laquila.Integrations.Application.DTO.Users.Request
{
    public class UpdateUserPasswordDTO
    {
        public UpdateUserPasswordDTO(Guid Id, string ActualPassword, string NewPassword, string ConfirmPassword)
        {
            this.Id = Id;
            this.ActualPassword = ActualPassword;
            this.NewPassword = NewPassword;
            this.ConfirmPassword = ConfirmPassword;
        }

        public Guid Id { get; set; }
        public string ActualPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}