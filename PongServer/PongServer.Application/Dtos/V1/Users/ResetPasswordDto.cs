namespace PongServer.Application.Dtos.V1.Users
{
    public class ResetPasswordDto
    {
        public string ResetPasswordToken { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
}
