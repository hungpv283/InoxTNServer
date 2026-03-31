namespace InoxServer.Domain.Errors
{
    public static class ReviewErrors
    {
        public static readonly Error NotFound =
            Error.NotFound(DomainErrorCodes.Review.NotFound, "Không tìm thấy đánh giá.");

        public static readonly Error AlreadyReviewed =
            Error.Conflict(DomainErrorCodes.Review.AlreadyReviewed, "Bạn đã đánh giá sản phẩm này rồi.");

        public static readonly Error NotPurchased =
            Error.BadRequest(DomainErrorCodes.Review.NotPurchased, "Bạn chưa mua sản phẩm này hoặc đơn hàng chưa hoàn thành.");

        public static readonly Error OrderNotCompleted =
            Error.BadRequest(DomainErrorCodes.Review.NotPurchased, "Chỉ có thể đánh giá khi đơn hàng đã hoàn thành.");

        public static readonly Error Unauthorized =
            Error.Forbidden(DomainErrorCodes.General.InsufficientPermissions, "Bạn không có quyền thực hiện thao tác này.");

        public static readonly Error InvalidRating =
            Error.BadRequest("REVIEW_INVALID_RATING", "Điểm đánh giá phải từ 1 đến 5.");

        public static readonly Error ProductNotFound =
            Error.NotFound(DomainErrorCodes.Product.NotFound, "Sản phẩm không tồn tại.");
    }
}
