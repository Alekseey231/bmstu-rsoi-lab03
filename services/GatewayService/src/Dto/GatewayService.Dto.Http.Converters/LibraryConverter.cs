using LibraryService.Dto.Http.Models;

namespace GatewayService.Dto.Http.Converters;

public static class LibraryConverter
{
    public static LibraryResponse Convert(Library model)
    {
        return new LibraryResponse(model.LibraryUid,
            model.Name,
            model.Address,
            model.City);
    }
}