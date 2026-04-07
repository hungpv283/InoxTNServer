using InoxServer.Application.Features.Coupons.Commands.ApplyCoupon;
using InoxServer.Application.Features.Coupons.Commands.ValidateCoupon;
using InoxServer.Application.Features.Coupons.Queries.GetAvailableCoupons;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InoxServer.Presentation.Controllers;

[ApiController]
[Route("api/v1/coupons")]
public class CouponsController : ControllerBase
{
    private readonly IMediator _mediator;

    public CouponsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Lấy danh sách coupon khả dụng (Public - ai cũng xem được)
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAvailableCoupons()
    {
        var result = await _mediator.Send(new GetAvailableCouponsQuery());
        return Ok(result);
    }

    /// <summary>
    /// Validate coupon - Kiểm tra coupon có hợp lệ không và tính số tiền giảm giá
    /// </summary>
    [HttpPost("validate")]
    [Authorize]
    public async Task<IActionResult> ValidateCoupon([FromBody] ValidateCouponDto dto)
    {
        var userId = GetCurrentUserId();
        var result = await _mediator.Send(new ValidateCouponCommand
        {
            Code = dto.Code,
            UserId = userId,
            OrderAmount = dto.OrderAmount
        });
        return Ok(result);
    }

    /// <summary>
    /// Apply coupon - Áp dụng coupon vào đơn hàng (gọi khi checkout)
    /// </summary>
    [HttpPost("apply")]
    [Authorize]
    public async Task<IActionResult> ApplyCoupon([FromBody] ApplyCouponDto dto)
    {
        var userId = GetCurrentUserId();
        var discountAmount = await _mediator.Send(new ApplyCouponCommand
        {
            Code = dto.Code,
            UserId = userId,
            OrderId = dto.OrderId,
            OrderAmount = dto.OrderAmount
        });
        return Ok(new { DiscountAmount = discountAmount, Message = "Áp dụng mã giảm giá thành công." });
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            throw new UnauthorizedAccessException("Không thể xác định người dùng.");
        return userId;
    }
}

public class ValidateCouponDto
{
    public string Code { get; set; } = default!;
    public decimal OrderAmount { get; set; }
}

public class ApplyCouponDto
{
    public string Code { get; set; } = default!;
    public Guid OrderId { get; set; }
    public decimal OrderAmount { get; set; }
}
