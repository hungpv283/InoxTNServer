using InoxServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoxServer.Domain.Interfaces.Repositories
{
    public interface IBannerRepository
    {
        Task<List<Banner>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<List<Banner>> GetActiveAsync(CancellationToken cancellationToken = default);

        Task<Banner?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task AddAsync(Banner banner, CancellationToken cancellationToken = default);

        void Update(Banner banner);

        void Delete(Banner banner);
    }
}
