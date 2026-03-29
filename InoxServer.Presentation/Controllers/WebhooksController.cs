using Asp.Versioning;
using InoxServer.Application.Features.Payments.Commands.ProcessPayOsWebhook;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace InoxServer.Presentation.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/webhooks")]
[AllowAnonymous]
public class WebhooksController : ControllerBase
{
    private readonly IMediator _mediator;

    public WebhooksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Webhook PayOS — xác thực chữ ký và cập nhật trạng thái thanh toán.</summary>
    [HttpPost("payos")]
    public async Task<IActionResult> PayOs([FromBody] JsonElement payload)
    {
        await _mediator.Send(new ProcessPayOsWebhookCommand { Payload = payload });
        return Ok();
    }
}
