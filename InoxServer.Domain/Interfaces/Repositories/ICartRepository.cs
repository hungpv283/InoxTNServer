using InoxServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoxServer.Domain.Interfaces.Repositories
{
    public interface ICartRepository
    {
        Task<Cart?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<Cart?> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default);

        Task<bool> ExistsByUserIdAsync(int userId, CancellationToken cancellationToken = default);

        Task AddAsync(Cart cart, CancellationToken cancellationToken = default);

        void Update(Cart cart);

        void Delete(Cart cart);
    }
}
