using CoreBookCondition = LibraryService.Core.Models.Enums.BookCondition;
using DtoBookCondition = LibraryService.Dto.Http.Models.Enums.BookCondition;

namespace LibraryService.Dto.Http.Converters.Enums;

public static class BookConditionConvertor
{
    public static DtoBookCondition Convert(CoreBookCondition model)
    {
        return model switch
        {
            CoreBookCondition.Bad => DtoBookCondition.Bad,
            CoreBookCondition.Good => DtoBookCondition.Good,
            CoreBookCondition.Excellent => DtoBookCondition.Excellent,

            _ => throw new ArgumentOutOfRangeException(nameof(model), model, null)
        };
    }
    
    public static CoreBookCondition Convert(DtoBookCondition model)
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