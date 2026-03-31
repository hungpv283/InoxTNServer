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
    public class ReviewRepository : IReviewRepository
    {
        private readonly AppDbContext _context;

        public ReviewRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Review>> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default)
        {
            return await _context.Reviews
                .AsNoTracking()
                .Where(x => x.ProductId == productId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<PagedResult<Review>> GetPagedByProductAsync(
            Guid productId,
            int page,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            var safePage = page <= 0 ? 1 : page;
            var safePageSize = pageSize <= 0 ? 10 : pageSize;

            var query = _context.Reviews
                .AsNoTracking()
                .Include(x => x.User)
                .Include(x => x.Product)
                .Where(x => x.ProductId == productId)
                .AsQueryable();

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((safePage - 1) * safePageSize)
                .Take(safePageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<Review>
            {
                Items = items,
                Page = safePage,
                PageSize = safePageSize,
                TotalCount = totalCount
            };
        }

        public async Task<PagedResult<Review>> GetPagedByUserAsync(
            Guid userId,
            int page,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            var safePage = page <= 0 ? 1 : page;
            var safePageSize = pageSize <= 0 ? 10 : pageSize;

            var query = _context.Reviews
                .AsNoTracking()
                .Include(x => x.User)
                .Include(x => x.Product)
                .Where(x => x.UserId == userId)
                .AsQueryable();

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((safePage - 1) * safePageSize)
                .Take(safePageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<Review>
            {
                Items = items,
                Page = safePage,
                PageSize = safePageSize,
                TotalCount = totalCount
            };
        }

        public async Task<PagedResult<Review>> GetAllPagedAsync(
            Guid? productId,
            Guid? userId,
            int page,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            var safePage = page <= 0 ? 1 : page;
            var safePageSize = pageSize <= 0 ? 10 : pageSize;

            var query = _context.Reviews
                .AsNoTracking()
                .Include(x => x.User)
                .Include(x => x.Product)
                .AsQueryable();

            if (productId.HasValue)
                query = query.Where(x => x.ProductId == productId.Value);

            if (userId.HasValue)
                query = query.Where(x => x.UserId == userId.Value);

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((safePage - 1) * safePageSize)
                .Take(safePageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<Review>
            {
                Items = items,
                Page = safePage,
                PageSize = safePageSize,
                TotalCount = totalCount
            };
        }

        public async Task<ReviewSummary> GetSummaryByProductAsync(Guid productId, CancellationToken cancellationToken = default)
        {
            var reviews = await _context.Reviews
                .AsNoTracking()
                .Where(x => x.ProductId == productId)
                .ToListAsync(cancellationToken);

            var total = reviews.Count;

            return new ReviewSummary
            {
                ProductId = productId,
                TotalReviews = total,
                AvgRating = total > 0 ? Math.Round((decimal)reviews.Average(x => x.Rating), 1) : 0,
                FiveStar = reviews.Count(r => r.Rating == 5),
                FourStar = reviews.Count(r => r.Rating == 4),
                ThreeStar = reviews.Count(r => r.Rating == 3),
                TwoStar = reviews.Count(r => r.Rating == 2),
                OneStar = reviews.Count(r => r.Rating == 1)
            };
        }

        public async Task<Review?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Reviews
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<Review?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Reviews
                .AsNoTracking()
                .Include(x => x.User)
                .Include(x => x.Product)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<Review?> GetByUserAndProductAsync(Guid userId, Guid productId, CancellationToken cancellationToken = default)
        {
            return await _context.Reviews
                .FirstOrDefaultAsync(x => x.UserId == userId && x.ProductId == productId, cancellationToken);
        }

        public async Task AddAsync(Review review, CancellationToken cancellationToken = default)
        {
            await _context.Reviews.AddAsync(review, cancellationToken);
        }

        public void Update(Review review)
        {
            _context.Reviews.Update(review);
        }

        public void Delete(Review review)
        {
            _context.Reviews.Remove(review);
        }
    }
}
