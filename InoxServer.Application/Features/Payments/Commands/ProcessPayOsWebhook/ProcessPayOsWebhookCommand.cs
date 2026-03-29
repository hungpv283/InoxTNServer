using System.Text.Json;
using MediatR;

namespace InoxServer.Application.Features.Payments.Commands.ProcessPayOsWebhook;

public class ProcessPayOsWebhookCommand : IRequest<Unit>
{
    public JsonElement Payload { get; set; }
}
