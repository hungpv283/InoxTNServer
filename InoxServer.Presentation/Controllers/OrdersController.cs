using Asp.Versioning;
using InoxServer.Application.Features.Orders.Commands.CancelOrder;
using InoxServer.Application.Features.Orders.Commands.UpdateOrderStatus;
using InoxServer.Application.Features.Orders.Queries.GetAllOrders;
using InoxServer.Application.Features.Orders.Queries.GetMyOrders;
using InoxServer.Application.Features.Orders.Queries.GetOrderById;
using InoxServer.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InoxServer.Presentation.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/orders")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Danh sách đơn của user đang đăng nhập.</summary>
    [HttpGet("me")]
    public async Task<IActionResult> GetMyOrders()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await _mediator.Send(new GetMyOrdersQuery { UserId = userId });
        return Ok(result);
    }

    /// <summary>Chi tiết một đơn (chủ đơn hoặc Admin).</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var isAdmin = User.IsInRole(nameof(UserRole.Admin));

        var result = await _mediator.Send(new GetOrderByIdQuery
        {
            OrderId = id,
            RequestingUserId = userId,
            IsAdmin = isAdmin
        });

        return Ok(result);
    }

    /// <summary>Hủy đơn (khách — chỉ khi đơn còn Pending).</summary>
    [HttpPost("{id:guid}/cancel")]
    public async Task<IActionResult> Cancel(Guid id, [FromBody] CancelOrderRequest? body)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        await _mediator.Send(new CancelOrderCommand
        {
            UserId = userId,
            OrderId = id,
            Reason = body?.Reason
        });
        return Ok();
    }

    /// <summary>Tất cả đơn (Admin) — phân trang, lọc theo status / orderNumber / userId.</summary>
    [Authorize(Roles = nameof(UserRole.Admin))]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAllOrdersQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>Cập nhật trạng thái đơn (Admin).</summary>
    [Authorize(Roles = nameof(UserRole.Admin))]
    [HttpPut("{id:guid}/status")]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateOrderStatusRequest body)
    {
        var adminId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        await _mediator.Send(new UpdateOrderStatusCommand
        {
            OrderId = id,
            NewStatus = body.Status,
            AdminUserId = adminId,
            Note = body.Note
        });
        return Ok();
    }
}

public class CancelOrderRequest
{
    public string? Reason { get; set; }
}

public class UpdateOrderStatusRequest
{
    public OrderStatus Status { get; set; }
    public string? Note { get; set; }
}
