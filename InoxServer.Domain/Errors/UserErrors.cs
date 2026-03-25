namespace InoxServer.Domain.Errors
{
    public static class UserErrors
    {
        public static readonly Error NotFound =
            Error.NotFound(DomainErrorCodes.User.NotFound, "Không tìm thấy người dùng.");

        public static readonly Error NotFoundById =
            Error.NotFound(DomainErrorCodes.User.NotFoundById, "Không tìm thấy người dùng với ID này.");

        public static readonly Error NotFoundByEmail =
            Error.NotFound(DomainErrorCodes.User.NotFoundByEmail, "Không tìm thấy người dùng với email này.");
    }
}
