using InoxServer.Domain.Entities;
using InoxServer.Domain.Interfaces.Repositories;
using InoxServer.Infrastructure.Contexts;
using InoxServer.SharedKernel.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoxServer.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Categories
                .AsNoTracking()
                .Include(x => x.Children)
                .OrderBy(x => x.SortOrder)
                .ThenBy(x => x.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Category>> GetRootCategoriesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Categories
                .AsNoTracking()
                .Include(x => x.Children)
                .Where(x => x.ParentId == null)
                .OrderBy(x => x.SortOrder)
                .ThenBy(x => x.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<Category?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Categories
                .Include(x => x.Children)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<Category?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Categories
                .Include(x => x.Children)
                .FirstOrDefaultAsync(x => x.Slug == slug, cancellationToken);
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Categories.AnyAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<bool> ExistsBySlugAsync(string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Categories.AnyAsync(x => x.Slug == slug, cancellationToken);
        }

        public async Task AddAsync(Category category, CancellationToken cancellationToken = default)
        {
            await _context.Categories.AddAsync(category, cancellationToken);
        }

        public void Update(Category category)
        {
            _context.Categories.Update(category);
        }

        public void Delete(Category category)
        {
            _context.Categories.Remove(category);
        }

        public async Task<PagedResult<Category>> GetPagedAsync(
            int page,
            int pageSize,
            string? keyword,
            int? parentId,
            bool? isActive,
            CancellationToken cancellationToken = default)
        {
            var query = _context.Categories
                .AsNoTracking()
                .Include(x => x.Children)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(x =>
                    x.Name.Contains(keyword) ||
                    x.Slug.Contains(keyword) ||
                    (x.Description != null && x.Description.Contains(keyword)));
            }

            if (parentId.HasValue)
            {
                query = query.Where(x => x.ParentId == parentId.Value);
            }

            if (isActive.HasValue)
            {
                query = query.Where(x => x.IsActive == isActive.Value);
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderBy(x => x.SortOrder)
                .ThenBy(x => x.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<Category>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }
    }
}
