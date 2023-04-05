namespace PongServer.Application.Dtos.V1.Users
{
    public class ResetEmailDto
    {
        public string ChangeEmailToken { get; set; }
        public string NewEmail { get; set; }
    }
}
