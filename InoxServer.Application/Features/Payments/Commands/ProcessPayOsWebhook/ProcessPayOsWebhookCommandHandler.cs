using System.Text.Json;
using InoxServer.Domain.Enums;
using InoxServer.Domain.Errors;
using InoxServer.Domain.Interfaces.Repositories;
using InoxServer.Domain.Interfaces.Services;
using MediatR;

namespace InoxServer.Application.Features.Payments.Commands.ProcessPayOsWebhook;

public class ProcessPayOsWebhookCommandHandler : IRequestHandler<ProcessPayOsWebhookCommand, Unit>
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPayOsWebhookSignatureVerifier _signatureVerifier;

    public ProcessPayOsWebhookCommandHandler(
        IPaymentRepository paymentRepository,
        IUnitOfWork unitOfWork,
        IPayOsWebhookSignatureVerifier signatureVerifier)
    {
        _paymentRepository = paymentRepository;
        _unitOfWork = unitOfWork;
        _signatureVerifier = signatureVerifier;
    }

    public async Task<Unit> Handle(ProcessPayOsWebhookCommand request, CancellationToken cancellationToken)
    {
        var root = request.Payload;
        if (root.ValueKind != JsonValueKind.Object)
            throw new DomainException(PaymentErrors.WebhookInvalidSignature);

        if (!root.TryGetProperty("data", out var data) || data.ValueKind != JsonValueKind.Object)
            throw new DomainException(PaymentErrors.WebhookInvalidSignature);

        if (!root.TryGetProperty("signature", out var sigEl) || sigEl.ValueKind != JsonValueKind.String)
            throw new DomainException(PaymentErrors.WebhookInvalidSignature);

        var signature = sigEl.GetString()!;
        if (!_signatureVerifier.IsValid(data, signature))
            throw new DomainException(PaymentErrors.WebhookInvalidSignature);

        if (!data.TryGetProperty("orderCode", out var orderCodeEl) || orderCodeEl.ValueKind != JsonValueKind.Number)
            return Unit.Value;

        var orderCode = orderCodeEl.GetInt64();

        var rootCode = root.TryGetProperty("code", out var codeEl) && codeEl.ValueKind == JsonValueKind.String
            ? codeEl.GetString()
            : null;

        var webhookJson = JsonSerializer.Serialize(request.Payload);

        await _unitOfWork.ExecuteInTransactionAsync(
            async () =>
            {
                var tracked = await _paymentRepository.GetByPayosOrderCodeAsync(orderCode, cancellationToken);
                if (tracked is null)
                    return;

                tracked.PayosWebhookData = webhookJson;

                if (tracked.Method != PaymentMethod.PayOS)
                    return;

                if (rootCode == "00")
                {
                    if (tracked.Status != PaymentStatus.Paid)
                    {
                        tracked.Status = PaymentStatus.Paid;
                        tracked.PaidAt = DateTime.UtcNow;
                        tracked.Order.Status = Domain.Enums.OrderStatus.Confirmed;
                    }
                }
                else if (tracked.Status == PaymentStatus.Pending)
                {
                    tracked.Status = PaymentStatus.Failed;
                }

                _paymentRepository.Update(tracked);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            },
            cancellationToken);

        return Unit.Value;
    }
}
