using DtoBookCondition = GatewayService.Dto.Http.Enums.BookCondition;
using LibraryServiceBookCondition = LibraryService.Dto.Http.Models.Enums.BookCondition;
using CoreBookCondition = GatewayService.Core.Models.Enums.BookCondition;

namespace GatewayService.Dto.Http.Converters.Enums;

public static class BookConditionConverter
{
    public static DtoBookCondition Convert(LibraryServiceBookCondition model)
    {
        return model switch
        {
            LibraryServiceBookCondition.Bad => DtoBookCondition.Bad,
            LibraryServiceBookCondition.Good => DtoBookCondition.Good,
            LibraryServiceBookCondition.Excellent => DtoBookCondition.Excellent,

            _ => throw new ArgumentOutOfRangeException(nameof(model), model, null)
        };
    }
    
    public static LibraryServiceBookCondition Convert(CoreBookCondition model)
    {
        return model switch
        {
            CoreBookCondition.Bad => LibraryServiceBookCondition.Bad,
            CoreBookCondition.Good => LibraryServiceBookCondition.Good,
            CoreBookCondition.Excellent => LibraryServiceBookCondition.Excellent,

            _ => throw new ArgumentOutOfRangeException(nameof(model), model, null)
        };
    }
    
    public static LibraryServiceBookCondition Convert(DtoBookCondition model)
    {
        return model switch
        {
            DtoBookCondition.Bad => LibraryServiceBookCondition.Bad,
            DtoBookCondition.Good => LibraryServiceBookCondition.Good,
            DtoBookCondition.Excellent => LibraryServiceBookCondition.Excellent,

            _ => throw new ArgumentOutOfRangeException(nameof(model), model, null)
        };
    }
    
    public static CoreBookCondition ConvertToCore(DtoBookCondition model)
    {
        return model switch
        {
            DtoBookCondition.Bad => CoreBookCondition.Bad,
            DtoBookCondition.Good => CoreBookCondition.Good,
            DtoBookCondition.Excellent => CoreBookCondition.Excellent,

            _ => throw new ArgumentOutOfRangeException(nameof(model), model, null)
        };
    }
}