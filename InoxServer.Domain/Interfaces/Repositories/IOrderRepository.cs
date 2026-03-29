using InoxServer.Domain.Entities;
using InoxServer.Domain.Enums;
using InoxServer.SharedKernel.Common;

namespace InoxServer.Domain.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<PagedResult<Order>> GetPagedAsync(
            int page,
            int pageSize,
            OrderStatus? status,
            string? orderNumber,
            Guid? userId,
            CancellationToken cancellationToken = default);

        Task<List<Order>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

        Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<Order?> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);

        Task<bool> ExistsByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default);

        Task AddAsync(Order order, CancellationToken cancellationToken = default);

        void Update(Order order);

        void Delete(Order order);
    }
}
