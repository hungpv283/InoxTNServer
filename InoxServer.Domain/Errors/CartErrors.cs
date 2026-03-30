using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoxServer.Domain.Errors
{
    public static class CartErrors
    {
        public static readonly Error NotFound =
            Error.NotFound(DomainErrorCodes.Cart.NotFound, "Không tìm thấy giỏ hàng.");

        public static readonly Error Empty =
            Error.BadRequest(DomainErrorCodes.Cart.Empty, "Giỏ hàng đang trống.");

        public static readonly Error ItemNotFound =
            Error.NotFound(DomainErrorCodes.Cart.ItemNotFound, "Không tìm thấy sản phẩm trong giỏ hàng.");

        public static readonly Error ProductNotFound =
            Error.NotFound(DomainErrorCodes.Cart.ProductNotFound, "Sản phẩm không tồn tại.");

        public static readonly Error ProductInactive =
            Error.BadRequest(DomainErrorCodes.Cart.ProductInactive, "Sản phẩm hiện không khả dụng.");

        public static readonly Error QuantityInvalid =
            Error.BadRequest(DomainErrorCodes.Cart.QuantityInvalid, "Số lượng phải lớn hơn 0.");

        public static readonly Error ExceedStock =
            Error.BadRequest(DomainErrorCodes.Cart.ExceedStock, "Số lượng vượt quá tồn kho.");

        public static readonly Error DuplicateItem =
            Error.Conflict(DomainErrorCodes.Cart.DuplicateItem, "Sản phẩm đã tồn tại trong giỏ hàng.");

        public static readonly Error CheckoutEmpty =
            Error.BadRequest(DomainErrorCodes.Cart.CheckoutEmpty, "Không thể thanh toán khi giỏ hàng đang trống.");

        public static readonly Error UnauthorizedAccess =
            Error.Forbidden(DomainErrorCodes.Cart.UnauthorizedAccess, "Bạn không có quyền truy cập giỏ hàng này.");
    }
}
