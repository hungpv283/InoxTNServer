namespace InoxServer.Domain.Errors
{
    public sealed class Error
    {
        public string Code { get; }
        public string Message { get; }
        public int StatusCode { get; }

        private Error(string code, string message, int statusCode)
        {
            Code = code;
            Message = message;
            StatusCode = statusCode;
        }

        public static Error NotFound(string code, string message) => new(code, message, 404);
        public static Error Conflict(string code, string message) => new(code, message, 409);
        public static Error Unauthorized(string code, string message) => new(code, message, 401);
        public static Error Forbidden(string code, string message) => new(code, message, 403);
        public static Error BadRequest(string code, string message) => new(code, message, 400);
        public static Error Internal(string code, string message) => new(code, message, 500);

        public override string ToString() => $"[{Code}] {Message}";
    }
}
