using InoxServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoxServer.Domain.Interfaces.Repositories
{
    public interface ICouponRepository
    {
        Task<List<Coupon>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<Coupon?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<Coupon?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);

        Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default);

        Task AddAsync(Coupon coupon, CancellationToken cancellationToken = default);

        void Update(Coupon coupon);

        void Delete(Coupon coupon);
    }
}
