using InoxServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoxServer.Domain.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateToken(User user);
    }
}
