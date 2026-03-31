using Asp.Versioning;
using InoxServer.Application.Features.Reviews.Commands.CreateReview;
using InoxServer.Application.Features.Reviews.Commands.DeleteReview;
using InoxServer.Application.Features.Reviews.Commands.UpdateReview;
using InoxServer.Application.Features.Reviews.Queries.GetAllReviews;
using InoxServer.Application.Features.Reviews.Queries.GetProductReviewSummary;
using InoxServer.Application.Features.Reviews.Queries.GetReviewById;
using InoxServer.Application.Features.Reviews.Queries.GetReviewsByProduct;
using InoxServer.Application.Features.Reviews.Queries.GetReviewsByUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InoxServer.Presentation.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/reviews")]
public class ReviewsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReviewsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    #region === Public: Lấy reviews theo sản phẩm ===

    /// <summary>
    /// Lấy danh sách review của một sản phẩm
    /// </summary>
    [HttpGet("product/{productId:guid}")]
    public async Task<IActionResult> GetByProduct(Guid productId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _mediator.Send(new GetReviewsByProductQuery(productId, page, pageSize));
        return Ok(result);
    }

    /// <summary>
    /// Lấy thống kê đánh giá của một sản phẩm (số sao trung bình, phân bố...)
    /// </summary>
    [HttpGet("product/{productId:guid}/summary")]
    public async Task<IActionResult> GetProductSummary(Guid productId)
    {
        var result = await _mediator.Send(new GetProductReviewSummaryQuery(productId));
        return Ok(result);
    }

    #endregion

    #region === Authenticated User: Tạo / Sửa / Xóa review ===

    /// <summary>
    /// Tạo đánh giá mới cho sản phẩm
    /// Chỉ cho phép khi đơn hàng có status = Completed
    /// </summary>
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateReviewCommand command)
    {
        var userId = GetCurrentUserId();
        command.UserId = userId;

        var reviewId = await _mediator.Send(command);
        return Ok(new { Id = reviewId });
    }

    /// <summary>
    /// Cập nhật đánh giá của chính mình
    /// </summary>
    [Authorize]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateReviewCommand command)
    {
        var userId = GetCurrentUserId();
        command.UserId = userId;

        var result = await _mediator.Send(command);
        return result ? Ok() : NotFound();
    }

    /// <summary>
    /// Xóa đánh giá của chính mình
    /// </summary>
    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = GetCurrentUserId();
        var result = await _mediator.Send(new DeleteReviewCommand { Id = id, UserId = userId });
        return result ? Ok() : NotFound();
    }

    /// <summary>
    /// Lấy danh sách review của chính mình
    /// </summary>
    [Authorize]
    [HttpGet("my-reviews")]
    public async Task<IActionResult> GetMyReviews([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var userId = GetCurrentUserId();
        var result = await _mediator.Send(new GetReviewsByUserQuery(userId, page, pageSize));
        return Ok(result);
    }

    #endregion

    #region === Admin: Quản lý tất cả reviews ===

    /// <summary>
    /// Lấy danh sách tất cả reviews (Admin)
    /// </summary>
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] Guid? productId = null,
        [FromQuery] Guid? userId = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _mediator.Send(new GetAllReviewsQuery(productId, userId, page, pageSize));
        return Ok(result);
    }

    /// <summary>
    /// Lấy chi tiết một review (Admin)
    /// </summary>
    [Authorize(Roles = "Admin")]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetReviewByIdQuery(id));
        return result == null ? NotFound() : Ok(result);
    }

    #endregion

    #region === Private Helpers ===

    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdClaim))
            throw new UnauthorizedAccessException("Không tìm thấy thông tin người dùng.");

        return Guid.Parse(userIdClaim);
    }

    #endregion
}
