using InoxServer.Domain.Entities;
using InoxServer.Domain.Interfaces.Repositories;
using InoxServer.Infrastructure.Contexts;
using System;
using System.Threading.Tasks;

namespace InoxServer.Infrastructure.Repositories;

public class CouponUsageRepository : ICouponUsageRepository
{
    private readonly AppDbContext _context;

    public CouponUsageRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(CouponUsage couponUsage, CancellationToken cancellationToken = default)
    {
        await _context.CouponUsages.AddAsync(couponUsage, cancellationToken);
    }
}
