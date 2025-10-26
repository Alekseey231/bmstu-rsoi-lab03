using DtoResponse = GatewayService.Dto.Http.LibraryBookPaginationResponse;
using LibraryServiceResponse = LibraryService.Dto.Http.LibraryBookPaginationResponse;

namespace GatewayService.Dto.Http.Converters;

public static class LibraryBookPaginationResponseConverter
{
    public static DtoResponse Convert(LibraryServiceResponse model)
    {
        return new DtoResponse(model.Page,
            model.PageSize,
            model.TotalElements,
            model.Items.ConvertAll(BookConverter.Convert));
    }
}