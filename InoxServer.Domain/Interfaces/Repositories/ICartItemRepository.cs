using InoxServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoxServer.Domain.Interfaces.Repositories
{
    public interface ICartItemRepository
    {
        Task<CartItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<CartItem?> GetByCartAndProductAsync(Guid cartId, Guid productId, CancellationToken cancellationToken = default);

        Task<List<CartItem>> GetByCartIdAsync(Guid cartId, CancellationToken cancellationToken = default);

        Task AddAsync(CartItem cartItem, CancellationToken cancellationToken = default);

        void Update(CartItem cartItem);

        void Delete(CartItem cartItem);
    }
}
