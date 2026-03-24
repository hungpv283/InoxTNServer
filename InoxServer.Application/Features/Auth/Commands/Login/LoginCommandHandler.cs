using InoxServer.Application.Features.Auth.DTOs;
using InoxServer.Domain.Interfaces;
using InoxServer.Domain.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                throw new Exception("Invalid email or password.");

            if (!user.IsActive)
                throw new Exception("Account is inactive.");

            var isValidPassword = _passwordHasher.VerifyPassword(request.Password, user.PasswordHash);
            if (!isValidPassword)
                throw new Exception("Invalid email or password.");

            var token = _jwtTokenService.GenerateToken(user);

            return new AuthResponseDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role.ToString(),
                AccessToken = token
            };
        }
    }
}
