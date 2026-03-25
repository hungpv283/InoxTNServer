namespace InoxServer.Domain.Errors
{
    public sealed class DomainException : Exception
    {
        public Error Error { get; }

        public DomainException(Error error) : base(error.Message)
        {
            Error = error;
        }
    }
}
