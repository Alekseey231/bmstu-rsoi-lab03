using DbLibrary = LibraryService.Database.Models.Library;
using CoreLibrary = LibraryService.Core.Models.Library;

namespace LibraryService.Database.Repositories.Converters;

public static class LibraryConverter
{
    public static CoreLibrary Convert(DbLibrary model)
    {
        return new CoreLibrary(model.LibraryId,
            model.Name,
            model.City,
            model.Address);
    }
}