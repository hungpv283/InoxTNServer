using InoxServer.Domain.Entities;

namespace InoxServer.Domain.Interfaces.Repositories;

public interface IProductImageRepository
{
    Task<ProductImage?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(ProductImage image, CancellationToken cancellationToken = default);
    void Delete(ProductImage image);
}
