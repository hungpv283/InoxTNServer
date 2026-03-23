using InoxServer.Domain.Entities;
using InoxServer.Domain.Interfaces.Repositories;
using InoxServer.Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoxServer.Infrastructure.Repositories
{
    public class CartItemRepository : ICartItemRepository
    {
        private readonly AppDbContext _context;

        public CartItemRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CartItem?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.CartItems
                .Include(x => x.Product)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<CartItem?> GetByCartAndProductAsync(int cartId, int productId, CancellationToken cancellationToken = default)
        {
            return await _context.CartItems
                .FirstOrDefaultAsync(x => x.CartId == cartId && x.ProductId == productId, cancellationToken);
        }

        public async Task<List<CartItem>> GetByCartIdAsync(int cartId, CancellationToken cancellationToken = default)
        {
            return await _context.CartItems
                .AsNoTracking()
                .Include(x => x.Product)
                .Where(x => x.CartId == cartId)
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(CartItem cartItem, CancellationToken cancellationToken = default)
        {
            await _context.CartItems.AddAsync(cartItem, cancellationToken);
        }

        public void Update(CartItem cartItem)
        {
            _context.CartItems.Update(cartItem);
        }

        public void Delete(CartItem cartItem)
        {
            _context.CartItems.Remove(cartItem);
        }
    }
}
