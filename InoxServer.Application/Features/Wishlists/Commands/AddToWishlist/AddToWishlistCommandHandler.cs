using InoxServer.Domain.Entities;
using InoxServer.Domain.Errors;
using InoxServer.Domain.Interfaces.Repositories;
using InoxServer.Domain.Interfaces.Services;
using MediatR;
using System;

namespace InoxServer.Application.Features.Wishlists.Commands.AddToWishlist;

public class AddToWishlistCommandHandler : IRequestHandler<AddToWishlistCommand, Guid>
{
    private readonly IWishlistRepository _wishlistRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddToWishlistCommandHandler(
        IWishlistRepository wishlistRepository,
        IProductRepository productRepository,
        IUnitOfWork unitOfWork)
    {
        _wishlistRepository = wishlistRepository;
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(AddToWishlistCommand request, CancellationToken cancellationToken)
    {
        // 1. Kiểm tra sản phẩm tồn tại
        var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product == null)
            throw new DomainException(WishlistErrors.ProductNotFound);

        // 2. Kiểm tra sản phẩm đang active
        if (!product.IsActive)
            throw new DomainException(WishlistErrors.ProductInactive);

        // 3. Kiểm tra đã có trong wishlist chưa
        var existing = await _wishlistRepository.GetByUserAndProductAsync(
            request.UserId, request.ProductId, cancellationToken);
        if (existing != null)
            throw new DomainException(WishlistErrors.AlreadyInWishlist);

        // 4. Tạo wishlist item mới
        var wishlistItem = new Wishlist
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            ProductId = request.ProductId,
            AddedAt = DateTime.UtcNow
        };

        // 5. Lưu vào database
        await _wishlistRepository.AddAsync(wishlistItem, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return wishlistItem.Id;
    }
}
