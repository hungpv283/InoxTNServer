using InoxServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoxServer.Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

        Task<User?> GetByVerificationTokenAsync(string token, CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);

        Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);

        Task AddAsync(User user, CancellationToken cancellationToken = default);

        void Update(User user);

        void Delete(User user);
    }
}
