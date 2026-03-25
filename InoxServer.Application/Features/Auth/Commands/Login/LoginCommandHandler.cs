using InoxServer.Application.Features.Auth.DTOs;
using InoxServer.Domain.Errors;
using InoxServer.Domain.Interfaces.Services;
using InoxServer.Domain.Interfaces.Repositories;
using MediatR;

namespace InoxServer.Application.Features.Auth.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenService _jwtTokenService;

        public LoginCommandHandler(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IJwtTokenService jwtTokenService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
            if (user == null)
                throw new DomainException(AuthErrors.InvalidCredentials);

            if (!user.IsActive)
                throw new DomainException(AuthErrors.AccountInactive);

            var isValidPassword = _passwordHasher.VerifyPassword(request.Password, user.PasswordHash);
            if (!isValidPassword)
                throw new DomainException(AuthErrors.InvalidCredentials);

            if (!user.EmailVerifiedAt.HasValue)
                throw new DomainException(AuthErrors.EmailNotVerified);

            var token = _jwtTokenService.GenerateToken(user);

            return new AuthResponseDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role.ToString(),
                IsEmailVerified = user.EmailVerifiedAt.HasValue,
                AccessToken = token
            };
        }
    }
}
