using InoxServer.Domain.Entities;
using InoxServer.SharedKernel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoxServer.Domain.Interfaces.Repositories
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<List<Category>> GetRootCategoriesAsync(CancellationToken cancellationToken = default);

        Task<Category?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<Category?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);

        Task<bool> ExistsBySlugAsync(string slug, CancellationToken cancellationToken = default);

        Task AddAsync(Category category, CancellationToken cancellationToken = default);

        void Update(Category category);

        void Delete(Category category);

        Task<PagedResult<Category>> GetPagedAsync(
            int page,
            int pageSize,
            string? keyword,
            int? parentId,
            bool? isActive,
            CancellationToken cancellationToken = default);
    }
}
