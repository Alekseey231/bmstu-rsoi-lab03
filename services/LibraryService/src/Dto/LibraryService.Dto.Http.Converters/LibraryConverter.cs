using CoreLibrary = LibraryService.Core.Models.Library;
using DtoLibrary = LibraryService.Dto.Http.Models.Library;

namespace LibraryService.Dto.Http.Converters;

public static class LibraryConverter
{
    public static DtoLibrary Convert(CoreLibrary model)
    {
        return new DtoLibrary(model.LibraryId,
            model.Name,
            model.Address,
            model.City);
    }
}