using InoxServer.Domain.Entities;
using InoxServer.Domain.Enums;
using InoxServer.Domain.Interfaces.Repositories;
using InoxServer.Infrastructure.Contexts;
using InoxServer.SharedKernel.Common;
using Microsoft.EntityFrameworkCore;

namespace InoxServer.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Orders
                .AsNoTracking()
                .Include(x => x.OrderItems)
                .Include(x => x.Payment)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<PagedResult<Order>> GetPagedAsync(
            int page,
            int pageSize,
            OrderStatus? status,
            string? orderNumber,
            Guid? userId,
            CancellationToken cancellationToken = default)
        {
            var safePage = page <= 0 ? 1 : page;
            var safePageSize = pageSize <= 0 ? 20 : Math.Min(pageSize, 100);

            var query = _context.Orders
                .AsNoTracking()
                .Include(x => x.OrderItems)
                .Include(x => x.Payment)
                .AsQueryable();

            if (status.HasValue)
                query = query.Where(x => x.Status == status.Value);

            if (userId.HasValue)
                query = query.Where(x => x.UserId == userId.Value);

            if (!string.IsNullOrWhiteSpace(orderNumber))
            {
                var term = orderNumber.Trim();
                query = query.Where(x => x.OrderNumber.Contains(term));
            }

            var total = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((safePage - 1) * safePageSize)
                .Take(safePageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<Order>
            {
                Items = items,
                Page = safePage,
                PageSize = safePageSize,
                TotalCount = total
            };
        }

        public async Task<List<Order>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _context.Orders
                .AsNoTracking()
                .Include(x => x.OrderItems)
                .Include(x => x.Payment)
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Orders
                .Include(x => x.OrderItems)
                .Include(x => x.Payment)
                .Include(x => x.OrderStatusLogs)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<Order?> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default)
        {
            return await _context.Orders
                .Include(x => x.OrderItems)
                .Include(x => x.Payment)
                .Include(x => x.OrderStatusLogs)
                .FirstOrDefaultAsync(x => x.OrderNumber == orderNumber, cancellationToken);
        }

        public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Orders.AnyAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<bool> ExistsByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default)
        {
            return await _context.Orders.AnyAsync(x => x.OrderNumber == orderNumber, cancellationToken);
        }

        public async Task AddAsync(Order order, CancellationToken cancellationToken = default)
        {
            await _context.Orders.AddAsync(order, cancellationToken);
        }

        public void Update(Order order)
        {
            _context.Orders.Update(order);
        }

        public void Delete(Order order)
        {
            _context.Orders.Remove(order);
        }
    }
}
