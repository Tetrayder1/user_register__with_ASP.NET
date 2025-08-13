namespace user_register.service.Services
{
    public interface IEmailService
    {
        public Task SendResetPasswordEmail(string resetEmailLink,string ToEmail);
    }
}
