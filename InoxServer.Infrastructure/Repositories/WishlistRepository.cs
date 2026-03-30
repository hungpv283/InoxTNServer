using InoxServer.Domain.Entities;
using InoxServer.Domain.Interfaces.Repositories;
using InoxServer.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoxServer.Infrastructure.Repositories
{
    public class WishlistRepository : IWishlistRepository
    {
        private readonly AppDbContext _context;

        public WishlistRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Wishlist>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _context.Wishlists
                .AsNoTracking()
                .Include(x => x.Product)
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.AddedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<Wishlist?> GetByUserAndProductAsync(Guid userId, Guid productId, CancellationToken cancellationToken = default)
        {
            return await _context.Wishlists
                .FirstOrDefaultAsync(x => x.UserId == userId && x.ProductId == productId, cancellationToken);
        }

        public async Task AddAsync(Wishlist wishlist, CancellationToken cancellationToken = default)
        {
            await _context.Wishlists.AddAsync(wishlist, cancellationToken);
        }

        public void Delete(Wishlist wishlist)
        {
            _context.Wishlists.Remove(wishlist);
        }
    }
}
