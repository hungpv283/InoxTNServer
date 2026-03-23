using InoxServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoxServer.Domain.Interfaces.Repositories
{
    public interface IReviewRepository
    {
        Task<List<Review>> GetByProductIdAsync(int productId, CancellationToken cancellationToken = default);

        Task<Review?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<Review?> GetByUserAndProductAsync(int userId, int productId, CancellationToken cancellationToken = default);

        Task AddAsync(Review review, CancellationToken cancellationToken = default);

        void Update(Review review);

        void Delete(Review review);
    }
}
