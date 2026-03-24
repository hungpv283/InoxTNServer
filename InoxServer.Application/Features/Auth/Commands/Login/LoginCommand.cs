using InoxServer.Application.Features.Auth.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoxServer.Application.Features.Auth.Commands.Login
{
    public class LoginCommand : IRequest<AuthResponseDto>
    {
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
