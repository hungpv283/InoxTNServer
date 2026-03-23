using InoxServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoxServer.Domain.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<List<Order>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default);

        Task<Order?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<Order?> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);

        Task<bool> ExistsByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default);

        Task AddAsync(Order order, CancellationToken cancellationToken = default);

        void Update(Order order);

        void Delete(Order order);
    }
}
