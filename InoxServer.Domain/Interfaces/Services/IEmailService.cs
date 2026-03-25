namespace InoxServer.Domain.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendEmailVerificationAsync(string toEmail, string toName, string verificationLink, CancellationToken cancellationToken = default);
    }
}
