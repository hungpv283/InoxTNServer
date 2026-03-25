namespace InoxServer.Domain.Errors
{
    public static class AuthErrors
    {
        public static readonly Error EmailAlreadyExists =
            Error.Conflict(DomainErrorCodes.Auth.EmailAlreadyExists, "Email này đã được sử dụng.");

        public static readonly Error InvalidCredentials =
            Error.Unauthorized(DomainErrorCodes.Auth.InvalidCredentials, "Email hoặc mật khẩu không đúng.");

        public static readonly Error AccountInactive =
            Error.Forbidden(DomainErrorCodes.Auth.AccountInactive, "Tài khoản đã bị vô hiệu hóa.");

        public static readonly Error EmailNotVerified =
            Error.Forbidden(DomainErrorCodes.Auth.EmailNotVerified, "Vui lòng xác thực email trước khi đăng nhập.");

        public static readonly Error InvalidVerificationToken =
            Error.BadRequest(DomainErrorCodes.Auth.InvalidVerificationToken, "Token xác thực không hợp lệ hoặc không tồn tại.");

        public static readonly Error VerificationTokenExpired =
            Error.BadRequest(DomainErrorCodes.Auth.VerificationTokenExpired, "Token xác thực đã hết hạn.");

        public static readonly Error EmailAlreadyVerified =
            Error.Conflict(DomainErrorCodes.Auth.EmailAlreadyVerified, "Email này đã được xác thực trước đó.");

        public static readonly Error ResendVerificationTooSoon =
            Error.BadRequest(DomainErrorCodes.Auth.ResendVerificationTooSoon, "Vui lòng chờ ít nhất 2 phút trước khi yêu cầu gửi lại email xác thực.");
    }
}
