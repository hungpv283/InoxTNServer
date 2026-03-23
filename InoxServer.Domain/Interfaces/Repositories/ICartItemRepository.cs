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
        Task<CartItem?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<CartItem?> GetByCartAndProductAsync(int cartId, int productId, CancellationToken cancellationToken = default);

        Task<List<CartItem>> GetByCartIdAsync(int cartId, CancellationToken cancellationToken = default);

        Task AddAsync(CartItem cartItem, CancellationToken cancellationToken = default);

        void Update(CartItem cartItem);

        void Delete(CartItem cartItem);
    }
}
