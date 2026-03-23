using InoxServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoxServer.Domain.Interfaces.Repositories
{
    public interface IPaymentRepository
    {
        Task<Payment?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<Payment?> GetByOrderIdAsync(int orderId, CancellationToken cancellationToken = default);

        Task<Payment?> GetByPayosOrderCodeAsync(long payosOrderCode, CancellationToken cancellationToken = default);

        Task AddAsync(Payment payment, CancellationToken cancellationToken = default);

        void Update(Payment payment);

        void Delete(Payment payment);
    }
}
