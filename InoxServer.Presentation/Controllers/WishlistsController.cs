using InoxServer.Application.Features.Wishlists.Commands.AddToWishlist;
using InoxServer.Application.Features.Wishlists.Commands.RemoveFromWishlist;
using InoxServer.Application.Features.Wishlists.Queries.GetMyWishlist;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InoxServer.Presentation.Controllers;

[ApiController]
[Route("api/v1/wishlists")]
[Authorize]
public class WishlistsController : ControllerBase
{
    private readonly IMediator _mediator;

    public WishlistsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Lấy danh sách yêu thích của người dùng hiện tại
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetMyWishlist()
    {
        var userId = GetCurrentUserId();
        var result = await _mediator.Send(new GetMyWishlistQuery { UserId = userId });
        return Ok(result);
    }

    /// <summary>
    /// Thêm sản phẩm vào danh sách yêu thích
    /// </summary>
    [HttpPost("{productId:guid}")]
    public async Task<IActionResult> AddToWishlist(Guid productId)
    {
        var userId = GetCurrentUserId();
        var wishlistItemId = await _mediator.Send(new AddToWishlistCommand
        {
            UserId = userId,
            ProductId = productId
        });

        return Ok(new { Id = wishlistItemId, Message = "Đã thêm vào danh sách yêu thích." });
    }

    /// <summary>
    /// Xóa sản phẩm khỏi danh sách yêu thích
    /// </summary>
    [HttpDelete("{productId:guid}")]
    public async Task<IActionResult> RemoveFromWishlist(Guid productId)
    {
        var userId = GetCurrentUserId();
        await _mediator.Send(new RemoveFromWishlistCommand
        {
            UserId = userId,
            ProductId = productId
        });

        return Ok(new { Message = "Đã xóa khỏi danh sách yêu thích." });
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            throw new UnauthorizedAccessException("Không thể xác định người dùng.");

        return userId;
    }
}
