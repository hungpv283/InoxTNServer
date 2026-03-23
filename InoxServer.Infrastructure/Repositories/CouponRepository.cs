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
    public class CouponRepository : ICouponRepository
    {
        private readonly AppDbContext _context;

        public CouponRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Coupon>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Coupons
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<Coupon?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Coupons.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<Coupon?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
        {
            return await _context.Coupons.FirstOrDefaultAsync(x => x.Code == code, cancellationToken);
        }

        public async Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default)
        {
            return await _context.Coupons.AnyAsync(x => x.Code == code, cancellationToken);
        }

        public async Task AddAsync(Coupon coupon, CancellationToken cancellationToken = default)
        {
            await _context.Coupons.AddAsync(coupon, cancellationToken);
        }

        public void Update(Coupon coupon)
        {
            _context.Coupons.Update(coupon);
        }

        public void Delete(Coupon coupon)
        {
            _context.Coupons.Remove(coupon);
        }
    }
}
