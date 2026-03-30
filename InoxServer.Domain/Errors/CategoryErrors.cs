namespace InoxServer.Domain.Errors
{
    public static class CategoryErrors
    {
        public static readonly Error NotFound =
            Error.NotFound(DomainErrorCodes.Category.NotFound, "Không tìm thấy danh mục.");

        public static readonly Error NameAlreadyExists =
            Error.Conflict(DomainErrorCodes.Category.NameAlreadyExists, "Tên danh mục đã tồn tại.");

        public static readonly Error SlugAlreadyExists =
            Error.Conflict(DomainErrorCodes.Category.SlugAlreadyExists, "Slug danh mục đã tồn tại.");

        public static readonly Error ParentNotFound =
            Error.NotFound(DomainErrorCodes.Category.ParentNotFound, "Danh mục cha không tồn tại.");

        public static readonly Error CannotBeOwnParent =
            Error.BadRequest(DomainErrorCodes.Category.CannotBeOwnParent, "Danh mục không thể là cha của chính nó.");
    }
}
