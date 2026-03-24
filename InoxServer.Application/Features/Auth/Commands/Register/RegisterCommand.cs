using InoxServer.Application.Features.Auth.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoxServer.Application.Features.Auth.Commands.Register
{
    public class RegisterCommand : IRequest<AuthResponseDto>
    {
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string? Phone { get; set; }
        public string? Address { get; set; }
    }
}
