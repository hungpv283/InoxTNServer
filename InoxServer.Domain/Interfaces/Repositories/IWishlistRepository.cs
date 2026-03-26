using InoxServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoxServer.Domain.Interfaces.Repositories
{
    public interface IWishlistRepository
    {
        Task<List<Wishlist>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

        Task<Wishlist?> GetByUserAndProductAsync(Guid userId, Guid productId, CancellationToken cancellationToken = default);

        Task AddAsync(Wishlist wishlist, CancellationToken cancellationToken = default);

        void Delete(Wishlist wishlist);
    }
}
