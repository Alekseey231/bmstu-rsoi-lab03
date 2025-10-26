namespace LibraryService.Core.Exceptions;

public class BookNotFoundInLibraryException : Exception
{
    public BookNotFoundInLibraryException()
    {

    }

    public BookNotFoundInLibraryException(string? message)
        : base(message)
    {

    }

    public BookNotFoundInLibraryException(string? message,
        Exception innerException)
        : base(message, innerException)
    {

    }
}
