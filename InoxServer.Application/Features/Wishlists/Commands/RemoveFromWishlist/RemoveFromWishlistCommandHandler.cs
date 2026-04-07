using InoxServer.Domain.Errors;
using InoxServer.Domain.Interfaces.Repositories;
using InoxServer.Domain.Interfaces.Services;
using MediatR;
using System;

namespace InoxServer.Application.Features.Wishlists.Commands.RemoveFromWishlist;

public class RemoveFromWishlistCommandHandler : IRequestHandler<RemoveFromWishlistCommand, bool>
{
    private readonly IWishlistRepository _wishlistRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveFromWishlistCommandHandler(
        IWishlistRepository wishlistRepository,
        IUnitOfWork unitOfWork)
    {
        _wishlistRepository = wishlistRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(RemoveFromWishlistCommand request, CancellationToken cancellationToken)
    {
        var wishlistItem = await _wishlistRepository.GetByUserAndProductAsync(
            request.UserId, request.ProductId, cancellationToken);

        if (wishlistItem == null)
            throw new DomainException(WishlistErrors.NotFound);

        _wishlistRepository.Delete(wishlistItem);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
