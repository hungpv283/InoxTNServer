using Asp.Versioning;
using InoxServer.Application.Features.Cart.Commands.AddToCart;
using InoxServer.Application.Features.Cart.Commands.AdjustCartItemQuantity;
using InoxServer.Application.Features.Cart.Commands.CheckoutCart;
using InoxServer.Application.Features.Cart.Commands.ClearCart;
using InoxServer.Application.Features.Cart.Commands.RemoveCartItem;
using InoxServer.Application.Features.Cart.Commands.UpdateCartItemQuantity;
using InoxServer.Application.Features.Cart.Queries.GetCart;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InoxServer.Presentation.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/cart")]
[Authorize]
public class CartController : ControllerBase
{
    private readonly IMediator _mediator;

    public CartController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetCart()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await _mediator.Send(new GetCartQuery { UserId = userId });
        return Ok(result);
    }

    [HttpPost("items")]
    public async Task<IActionResult> AddItem([FromBody] AddToCartCommand command)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        command.UserId = userId;

        var cartItemId = await _mediator.Send(command);
        return Ok(new { CartItemId = cartItemId });
    }

    [HttpPut("items/{productId:guid}")]
    public async Task<IActionResult> UpdateItem(
        [FromRoute] Guid productId,
        [FromBody] UpdateCartItemQuantityCommand command)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        command.UserId = userId;
        command.ProductId = productId;

        await _mediator.Send(command);
        return Ok();
    }

    [HttpPost("items/{productId:guid}/inc")]
    public async Task<IActionResult> Increment([FromRoute] Guid productId)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        await _mediator.Send(new AdjustCartItemQuantityCommand
        {
            UserId = userId,
            ProductId = productId,
            Delta = 1
        });
        return Ok();
    }

    [HttpPost("items/{productId:guid}/dec")]
    public async Task<IActionResult> Decrement([FromRoute] Guid productId)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        await _mediator.Send(new AdjustCartItemQuantityCommand
        {
            UserId = userId,
            ProductId = productId,
            Delta = -1
        });
        return Ok();
    }

    [HttpDelete("items/{productId:guid}")]
    public async Task<IActionResult> RemoveItem([FromRoute] Guid productId)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var result = await _mediator.Send(new RemoveCartItemCommand
        {
            UserId = userId,
            ProductId = productId
        });

        return result ? Ok() : NotFound();
    }

    [HttpDelete]
    public async Task<IActionResult> ClearCart()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        await _mediator.Send(new ClearCartCommand { UserId = userId });
        return Ok();
    }

    [HttpPost("checkout")]
    public async Task<IActionResult> Checkout([FromBody] CheckoutCartCommand command)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        command.UserId = userId;

        var result = await _mediator.Send(command);
        return Ok(result);
    }
}

