namespace RatingService.Core.Exceptions;

public class RatingNotFoundException : Exception
{
    public RatingNotFoundException()
    {

    }

    public RatingNotFoundException(string? message)
        : base(message)
    {

    }

    public RatingNotFoundException(string? message,
        Exception innerException)
        : base(message, innerException)
    {

    }
}
