using InoxServer.Domain.Entities;
using InoxServer.SharedKernel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoxServer.Domain.Interfaces.Repositories
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<Product?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);

        Task AddAsync(Product product, CancellationToken cancellationToken = default);

        void Update(Product product);

        void Delete(Product product);

        Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);

        Task<int> CountAsync(CancellationToken cancellationToken = default);

        Task<PagedResult<Product>> GetPagedAsync(
             int page,
             int pageSize,
             string? keyword,
             int? categoryId,
             decimal? minPrice,
             decimal? maxPrice,
             bool? isActive,
             CancellationToken cancellationToken = default);
    }
}
