using InoxServer.Domain.Entities;
using InoxServer.Domain.Interfaces.Repositories;
using InoxServer.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoxServer.Infrastructure.Repositories
{
    public class BannerRepository : IBannerRepository
    {
        private readonly AppDbContext _context;

        public BannerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Banner>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Banners
                .AsNoTracking()
                .OrderBy(x => x.SortOrder)
                .ThenByDescending(x => x.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Banner>> GetActiveAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Banners
                .AsNoTracking()
                .Where(x => x.IsActive)
                .OrderBy(x => x.SortOrder)
                .ThenByDescending(x => x.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<Banner?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Banners.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task AddAsync(Banner banner, CancellationToken cancellationToken = default)
        {
            await _context.Banners.AddAsync(banner, cancellationToken);
        }

        public void Update(Banner banner)
        {
            _context.Banners.Update(banner);
        }

        public void Delete(Banner banner)
        {
            _context.Banners.Remove(banner);
        }
    }
}
