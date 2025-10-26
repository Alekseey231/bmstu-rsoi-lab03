namespace ReservationService.Core.Exceptions;

public class ReservationAlreadyExistsException : Exception
{
    public ReservationAlreadyExistsException()
    {

    }

    public ReservationAlreadyExistsException(string? message)
        : base(message)
    {

    }

    public ReservationAlreadyExistsException(string? message,
        Exception innerException)
        : base(message, innerException)
    {

    }
}
