namespace InoxServer.Domain.Errors
{
    public static class BannerErrors
    {
        public static readonly Error NotFound =
            Error.NotFound(DomainErrorCodes.General.ObjectNotFound, "Không tìm thấy banner.");
    }
}
