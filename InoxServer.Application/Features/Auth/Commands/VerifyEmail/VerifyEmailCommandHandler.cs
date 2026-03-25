using InoxServer.Domain.Errors;
using InoxServer.Domain.Interfaces.Services;
using InoxServer.Domain.Interfaces.Repositories;
using MediatR;

namespace InoxServer.Application.Features.Auth.Commands.VerifyEmail;

public class VerifyEmailCommandHandler : IRequestHandler<VerifyEmailCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public VerifyEmailCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByVerificationTokenAsync(request.Token, cancellationToken);

        if (user is null)
            throw new DomainException(AuthErrors.InvalidVerificationToken);

        if (user.EmailVerifiedAt.HasValue)
            throw new DomainException(AuthErrors.EmailAlreadyVerified);

        if (user.EmailVerificationTokenExpiresAt < DateTime.UtcNow)
            throw new DomainException(AuthErrors.VerificationTokenExpired);

        user.EmailVerifiedAt = DateTime.UtcNow;
        user.EmailVerificationToken = null;
        user.EmailVerificationTokenExpiresAt = null;
        user.UpdatedAt = DateTime.UtcNow;

        _userRepository.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
