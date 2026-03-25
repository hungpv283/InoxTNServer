using InoxServer.Domain.Errors;
using InoxServer.Domain.Interfaces.Repositories;
using InoxServer.Domain.Interfaces.Services;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace InoxServer.Application.Features.Auth.Commands.ResendVerifyEmail;

public class ResendVerifyEmailCommandHandler : IRequestHandler<ResendVerifyEmailCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;

    public ResendVerifyEmailCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IEmailService emailService,
        IConfiguration configuration)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _emailService = emailService;
        _configuration = configuration;
    }

    public async Task<bool> Handle(ResendVerifyEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (user is null)
            throw new DomainException(UserErrors.NotFoundByEmail);

        if (user.EmailVerifiedAt.HasValue)
            throw new DomainException(AuthErrors.EmailAlreadyVerified);

        // Cooldown 2 phút: tránh spam gửi lại
        if (user.EmailVerificationTokenExpiresAt.HasValue)
        {
            var tokenCreatedAt = user.EmailVerificationTokenExpiresAt.Value.AddHours(-24);
            var cooldownEnd = tokenCreatedAt.AddMinutes(2);
            if (DateTime.UtcNow < cooldownEnd)
                throw new DomainException(AuthErrors.ResendVerificationTooSoon);
        }

        var newToken = Guid.NewGuid().ToString("N");
        user.EmailVerificationToken = newToken;
        user.EmailVerificationTokenExpiresAt = DateTime.UtcNow.AddHours(24);
        user.UpdatedAt = DateTime.UtcNow;

        _userRepository.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var frontendUrl = _configuration["App:FrontendUrl"]?.TrimEnd('/') ?? "http://localhost:3000";
        var verificationLink = $"{frontendUrl}/verify-email?token={newToken}";

        await _emailService.ResendVerificationEmailAsync(user.Email, user.Name, verificationLink, cancellationToken);

        return true;
    }
}
