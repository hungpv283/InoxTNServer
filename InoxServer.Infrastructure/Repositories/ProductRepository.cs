using InoxServer.Domain.Entities;
using InoxServer.Domain.Interfaces.Repositories;
using InoxServer.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using InoxServer.SharedKernel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoxServer.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Products
                .AsNoTracking()
                .Include(x => x.Category)
                .ToListAsync(cancellationToken);
        }

        public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Products
                .Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<Product?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Products
                .Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.Slug == slug, cancellationToken);
        }

        public async Task AddAsync(Product product, CancellationToken cancellationToken = default)
        {
            await _context.Products.AddAsync(product, cancellationToken);
        }

        public void Update(Product product)
        {
            _context.Products.Update(product);
        }

        public void Delete(Product product)
        {
            _context.Products.Remove(product);
        }

        public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Products.AnyAsync(x => x.Id == id, cancellationToken);
        }
        public async Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Products.CountAsync(cancellationToken);
        }

        public async Task<PagedResult<Product>> GetPagedAsync(
    int page,
    int pageSize,
    string? keyword,
    Guid? categoryId,
    decimal? minPrice,
    decimal? maxPrice,
    bool? isActive,
    CancellationToken cancellationToken = default)
        {
            var query = _context.Products
                .AsNoTracking()
                .Include(x => x.Category)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(x =>
                    x.Name.Contains(keyword) ||
                    (x.Sku != null && x.Sku.Contains(keyword)) ||
                    (x.Material != null && x.Material.Contains(keyword)));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(x => x.CategoryId == categoryId.Value);
            }

            if (minPrice.HasValue)
            {
                query = query.Where(x => x.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(x => x.Price <= maxPrice.Value);
            }

            if (isActive.HasValue)
            {
                query = query.Where(x => x.IsActive == isActive.Value);
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<Product>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

    }
}
