namespace InoxServer.Domain.Errors
{
    public static class DomainErrorCodes
    {
        public static class Auth
        {
            public const string EmailAlreadyExists = "AUTH_EMAIL_ALREADY_EXISTS";
            public const string InvalidCredentials = "AUTH_INVALID_CREDENTIALS";
            public const string AccountInactive = "AUTH_ACCOUNT_INACTIVE";
            public const string EmailNotVerified = "AUTH_EMAIL_NOT_VERIFIED";
            public const string InvalidVerificationToken = "AUTH_INVALID_VERIFICATION_TOKEN";
            public const string VerificationTokenExpired = "AUTH_VERIFICATION_TOKEN_EXPIRED";
            public const string EmailAlreadyVerified = "AUTH_EMAIL_ALREADY_VERIFIED";
            public const string ResendVerificationTooSoon = "AUTH_RESEND_VERIFICATION_TOO_SOON";
        }

        public static class User
        {
            public const string NotFound = "USER_NOT_FOUND";
            public const string NotFoundById = "USER_NOT_FOUND_BY_ID";
            public const string NotFoundByEmail = "USER_NOT_FOUND_BY_EMAIL";
            public const string EmptyName = "USER_EMPTY_NAME";
            public const string EmptyEmail = "USER_EMPTY_EMAIL";
            public const string EmptyPassword = "USER_EMPTY_PASSWORD";
            public const string InvalidEmail = "USER_INVALID_EMAIL";
            public const string ShortPassword = "USER_PASSWORD_TOO_SHORT";
            public const string LongPassword = "USER_PASSWORD_TOO_LONG";
            public const string PasswordMissingUppercase = "USER_PASSWORD_MISSING_UPPERCASE";
            public const string PasswordMissingLowercase = "USER_PASSWORD_MISSING_LOWERCASE";
            public const string PasswordMissingNumber = "USER_PASSWORD_MISSING_NUMBER";
            public const string PasswordMissingSpecialChar = "USER_PASSWORD_MISSING_SPECIAL_CHAR";
        }

        public static class Category
        {
            public const string NotFound = "CATEGORY_NOT_FOUND";
            public const string NameAlreadyExists = "CATEGORY_NAME_ALREADY_EXISTS";
            public const string SlugAlreadyExists = "CATEGORY_SLUG_ALREADY_EXISTS";
            public const string ParentNotFound = "CATEGORY_PARENT_NOT_FOUND";
            public const string CannotBeOwnParent = "CATEGORY_CANNOT_BE_OWN_PARENT";
        }

        public static class Product
        {
            public const string NotFound = "PRODUCT_NOT_FOUND";
            public const string NameAlreadyExists = "PRODUCT_NAME_ALREADY_EXISTS";
            public const string SlugAlreadyExists = "PRODUCT_SLUG_ALREADY_EXISTS";
            public const string InsufficientStock = "PRODUCT_INSUFFICIENT_STOCK";
            public const string CategoryNotFound = "PRODUCT_CATEGORY_NOT_FOUND";
        }

        public static class Order
        {
            public const string NotFound = "ORDER_NOT_FOUND";
            public const string AlreadyCancelled = "ORDER_ALREADY_CANCELLED";
            public const string CannotCancel = "ORDER_CANNOT_CANCEL";
            public const string InvalidStatus = "ORDER_INVALID_STATUS";
        }

        public static class Coupon
        {
            public const string NotFound = "COUPON_NOT_FOUND";
            public const string Expired = "COUPON_EXPIRED";
            public const string UsageLimitReached = "COUPON_USAGE_LIMIT_REACHED";
            public const string NotYetActive = "COUPON_NOT_YET_ACTIVE";
            public const string AlreadyUsed = "COUPON_ALREADY_USED_BY_USER";
            public const string MinOrderNotMet = "COUPON_MIN_ORDER_NOT_MET";
        }

        public static class Payment
        {
            public const string NotFound = "PAYMENT_NOT_FOUND";
            public const string AlreadyPaid = "PAYMENT_ALREADY_PAID";
            public const string Failed = "PAYMENT_FAILED";
        }

        public static class Review
        {
            public const string NotFound = "REVIEW_NOT_FOUND";
            public const string AlreadyReviewed = "REVIEW_ALREADY_REVIEWED";
            public const string NotPurchased = "REVIEW_NOT_PURCHASED";
        }

        public static class Banner
        {
            public const string NotFound = "BANNER_NOT_FOUND";
        }

        public static class Cart
        {
            public const string NotFound = "CART_NOT_FOUND";
            public const string ItemNotFound = "CART_ITEM_NOT_FOUND";
        }

        public static class General
        {
            public const string CommitFailed = "COMMIT_FAILED";
            public const string ObjectNotFound = "OBJECT_NOT_FOUND";
            public const string InsufficientPermissions = "UNAUTHORIZED";
            public const string UploadFailed = "UPLOAD_FAILED";
            public const string ExpiredToken = "EXPIRED_TOKEN";
            public const string ObjectAlreadyExists = "OBJECT_ALREADY_EXISTS";
            public const string InvalidOperation = "INVALID_OPERATION";
        }
    }
}
