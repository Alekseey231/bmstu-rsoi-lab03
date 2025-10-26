namespace LibraryService.Server.Helpers;

public class PageSizeConverter
{
    public static (int limit, int offset) ToLimitOffset(int page, int size)
    {
        var limit = size;
        var offset = (page - 1) * size;
    
        return (limit, offset);
    }
}