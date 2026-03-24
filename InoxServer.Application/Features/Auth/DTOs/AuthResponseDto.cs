using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoxServer.Application.Features.Auth.DTOs
{
    public class AuthResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;

        public string Role { get; set; } = default!;

        public string AccessToken { get; set; } = default!;

    }
}
