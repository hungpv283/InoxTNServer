using InoxServer.Domain.Entities;
using InoxServer.Domain.Interfaces.Repositories;
using InoxServer.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace InoxServer.Infrastructure.Repositories;

public class ProductImageRepository : IProductImageRepository
{
    private readonly AppDbContext _context;

    public ProductImageRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ProductImage?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.ProductImages.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task AddAsync(ProductImage image, CancellationToken cancellationToken = default)
        => await _context.ProductImages.AddAsync(image, cancellationToken);

    public void Delete(ProductImage image)
        => _context.ProductImages.Remove(image);
}
