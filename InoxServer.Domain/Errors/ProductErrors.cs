namespace InoxServer.Domain.Errors
{
    public static class ProductErrors
    {
        public static readonly Error NotFound =
            Error.NotFound(DomainErrorCodes.Product.NotFound, "Không tìm thấy sản phẩm.");

        public static readonly Error NameAlreadyExists =
            Error.Conflict(DomainErrorCodes.Product.NameAlreadyExists, "Tên sản phẩm đã tồn tại.");

        public static readonly Error SlugAlreadyExists =
            Error.Conflict(DomainErrorCodes.Product.SlugAlreadyExists, "Slug sản phẩm đã tồn tại.");

        public static readonly Error InsufficientStock =
            Error.BadRequest(DomainErrorCodes.Product.InsufficientStock, "Số lượng tồn kho không đủ.");

        public static readonly Error CategoryNotFound =
            Error.NotFound(DomainErrorCodes.Product.CategoryNotFound, "Danh mục của sản phẩm không tồn tại.");
    }
}
