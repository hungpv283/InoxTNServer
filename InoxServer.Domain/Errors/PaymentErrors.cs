namespace InoxServer.Domain.Errors;

public static class PaymentErrors
{
    public static readonly Error PayOsCreateFailed =
        Error.BadRequest(DomainErrorCodes.Payment.Failed, "Không tạo được link thanh toán PayOS.");

    public static readonly Error WebhookInvalidSignature =
        Error.BadRequest(DomainErrorCodes.Payment.Failed, "Chữ ký webhook PayOS không hợp lệ.");

    public static readonly Error NotFound =
        Error.NotFound(DomainErrorCodes.Payment.NotFound, "Không tìm thấy thanh toán.");
}
