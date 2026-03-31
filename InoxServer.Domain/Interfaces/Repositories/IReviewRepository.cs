using InoxServer.Domain.Entities;
using InoxServer.SharedKernel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoxServer.Domain.Interfaces.Repositories
{
    public interface IReviewRepository
    {
        Task<List<Review>> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);

        Task<PagedResult<Review>> GetPagedByProductAsync(
            Guid productId,
            int page,
            int pageSize,
            CancellationToken cancellationToken = default);

        Task<PagedResult<Review>> GetPagedByUserAsync(
            Guid userId,
            int page,
            int pageSize,
            CancellationToken cancellationToken = default);

        Task<PagedResult<Review>> GetAllPagedAsync(
            Guid? productId,
            Guid? userId,
            int page,
            int pageSize,
            CancellationToken cancellationToken = default);

        Task<ReviewSummary> GetSummaryByProductAsync(Guid productId, CancellationToken cancellationToken = default);

        Task<Review?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<Review?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);

        Task<Review?> GetByUserAndProductAsync(Guid userId, Guid productId, CancellationToken cancellationToken = default);

        Task AddAsync(Review review, CancellationToken cancellationToken = default);

        void Update(Review review);

        void Delete(Review review);
    }

    public class ReviewSummary
    {
        public Guid ProductId { get; set; }
        public decimal AvgRating { get; set; }
        public int TotalReviews { get; set; }
        public int FiveStar { get; set; }
        public int FourStar { get; set; }
        public int ThreeStar { get; set; }
        public int TwoStar { get; set; }
        public int OneStar { get; set; }
    }
}
