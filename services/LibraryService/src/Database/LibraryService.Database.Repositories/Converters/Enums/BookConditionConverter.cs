using CoreBookCondition = LibraryService.Core.Models.Enums.BookCondition;
using DbBookCondition = LibraryService.Database.Models.Enums.BookCondition;

namespace LibraryService.Database.Repositories.Converters.Enums;

public static class BookConditionConverter
{
    public static CoreBookCondition Convert(DbBookCondition model)
    {
        return model switch
        {
            DbBookCondition.Bad => CoreBookCondition.Bad,
            DbBookCondition.Excellent => CoreBookCondition.Excellent,
            DbBookCondition.Good => CoreBookCondition.Good,

            _ => throw new ArgumentOutOfRangeException(nameof(model), model, null)
        };
    }
    
    public static DbBookCondition Convert(CoreBookCondition model)
    {
        return model switch
        {
            CoreBookCondition.Bad => DbBookCondition.Bad,
            CoreBookCondition.Excellent => DbBookCondition.Excellent,
            CoreBookCondition.Good => DbBookCondition.Good,

            _ => throw new ArgumentOutOfRangeException(nameof(model), model, null)
        };
    }
}