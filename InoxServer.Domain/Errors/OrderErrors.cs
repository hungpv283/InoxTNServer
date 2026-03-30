namespace InoxServer.Domain.Errors;

public static class OrderErrors
{
    public static readonly Error NotFound =
        Error.NotFound(DomainErrorCodes.Order.NotFound, "Không tìm thấy đơn hàng.");

    public static readonly Error AlreadyCancelled =
        Error.BadRequest(DomainErrorCodes.Order.AlreadyCancelled, "Đơn hàng đã bị hủy trước đó.");

    public static readonly Error CannotCancel =
        Error.BadRequest(DomainErrorCodes.Order.CannotCancel, "Không thể hủy đơn hàng ở trạng thái hiện tại.");

    public static readonly Error InvalidStatus =
        Error.BadRequest(DomainErrorCodes.Order.InvalidStatus, "Trạng thái đơn hàng không hợp lệ.");

    public static readonly Error UnauthorizedAccess =
        Error.Forbidden(DomainErrorCodes.Order.UnauthorizedAccess, "Bạn không có quyền truy cập đơn hàng này.");
}
