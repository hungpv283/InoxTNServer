using InoxServer.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace InoxServer.Domain.Interfaces.Repositories;

public interface ICouponUsageRepository
{
    Task AddAsync(CouponUsage couponUsage, CancellationToken cancellationToken = default);
}
