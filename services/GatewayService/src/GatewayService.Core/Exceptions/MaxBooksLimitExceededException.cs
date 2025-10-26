namespace GatewayService.Core.Exceptions;

public class MaxBooksLimitExceededException : Exception
{
    public MaxBooksLimitExceededException()
    {

    }

    public MaxBooksLimitExceededException(string? message)
        : base(message)
    {

    }

    public MaxBooksLimitExceededException(string? message,
        Exception innerException)
        : base(message, innerException)
    {

    }
}
