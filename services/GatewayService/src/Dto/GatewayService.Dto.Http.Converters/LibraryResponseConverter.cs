using DtoResponse = GatewayService.Dto.Http.LibraryPaginationResponse;
using LibraryServiceResponse = LibraryService.Dto.Http.LibraryPaginationResponse;

namespace GatewayService.Dto.Http.Converters;

public static class LibraryResponseConverter
{
    public static DtoResponse Convert(LibraryServiceResponse model)
    {
        return new DtoResponse(model.Page,
            model.PageSize,
            model.TotalElements,
            model.Items.ConvertAll(LibraryConverter.Convert));
    }
}