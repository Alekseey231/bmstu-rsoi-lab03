using CoreBookWithLibrary = LibraryService.Core.BookWithLibrary;
using DtoBookWithLibrary = LibraryService.Dto.Http.Models.BookWithLibrary;

namespace LibraryService.Dto.Http.Converters;

public static class LibraryWithBookConverter
{
    public static DtoBookWithLibrary Convert(CoreBookWithLibrary model)
    {
        return new DtoBookWithLibrary(LibraryBookConverter.Convert(model.Book),
            LibraryConverter.Convert(model.Library));
    }
}