namespace InoxServer.Domain.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendEmailVerificationAsync(string toEmail, string toName, string verificationLink, CancellationToken cancellationToken = default);

        Task ResendVerificationEmailAsync(string toEmail, string toName, string verificationLink, CancellationToken cancellationToken = default);

        Task SendPasswordResetEmailAsync(string toEmail, string toName, string resetLink, CancellationToken cancellationToken = default);
    }
}
