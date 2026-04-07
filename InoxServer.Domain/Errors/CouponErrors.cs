namespace InoxServer.Domain.Errors
{
    public static class CouponErrors
    {
        public static readonly Error NotFound =
            Error.NotFound(DomainErrorCodes.Coupon.NotFound, "Mã giảm giá không tồn tại.");

        public static readonly Error Expired =
            Error.BadRequest(DomainErrorCodes.Coupon.Expired, "Mã giảm giá đã hết hạn.");

        public static readonly Error NotYetActive =
            Error.BadRequest(DomainErrorCodes.Coupon.NotYetActive, "Mã giảm giá chưa có hiệu lực.");

        public static readonly Error Inactive =
            Error.BadRequest(DomainErrorCodes.Coupon.NotFound, "Mã giảm giá không còn hoạt động.");

        public static readonly Error UsageLimitReached =
            Error.BadRequest(DomainErrorCodes.Coupon.UsageLimitReached, "Mã giảm giá đã hết lượt sử dụng.");

        public static readonly Error AlreadyUsed =
            Error.Conflict(DomainErrorCodes.Coupon.AlreadyUsed, "Bạn đã sử dụng mã giảm giá này.");

        public static readonly Error MinOrderNotMet =
            Error.BadRequest(DomainErrorCodes.Coupon.MinOrderNotMet, "Giá trị đơn hàng chưa đạt mức tối thiểu để sử dụng mã giảm giá.");

        public static readonly Error CodeAlreadyExists =
            Error.Conflict(DomainErrorCodes.Coupon.NotFound, "Mã giảm giá đã tồn tại.");

        public static readonly Error InvalidValue =
            Error.BadRequest(DomainErrorCodes.General.InvalidOperation, "Giá trị giảm giá không hợp lệ.");
    }
}
