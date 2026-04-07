namespace InoxServer.Domain.Errors
{
    public static class WishlistErrors
    {
        public static readonly Error NotFound =
            Error.NotFound(DomainErrorCodes.Product.NotFound, "Không tìm thấy sản phẩm trong danh sách yêu thích.");

        public static readonly Error ProductNotFound =
            Error.NotFound(DomainErrorCodes.Product.NotFound, "Sản phẩm không tồn tại.");

        public static readonly Error ProductInactive =
            Error.BadRequest(DomainErrorCodes.Product.NotFound, "Sản phẩm không còn được bán.");

        public static readonly Error AlreadyInWishlist =
            Error.Conflict(DomainErrorCodes.General.ObjectAlreadyExists, "Sản phẩm đã có trong danh sách yêu thích.");

        public static readonly Error Unauthorized =
            Error.Unauthorized(DomainErrorCodes.General.InsufficientPermissions, "Bạn không có quyền thực hiện thao tác này.");

        public static readonly Error EmptyWishlist =
            Error.BadRequest(DomainErrorCodes.General.InvalidOperation, "Danh sách yêu thích trống.");
    }
}
