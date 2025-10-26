namespace LibraryService.Core.Exceptions;

public class LibraryNotFoundException : Exception
{
    public LibraryNotFoundException()
    {

    }

    public LibraryNotFoundException(string? message)
        : base(message)
    {

    }

    public LibraryNotFoundException(string? message,
        Exception innerException)
        : base(message, innerException)
    {

    }
}
