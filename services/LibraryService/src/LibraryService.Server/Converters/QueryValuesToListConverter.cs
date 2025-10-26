namespace LibraryService.Server.Converters;

public static class QueryValuesToListConverter
{
    public static List<Guid>? Convert(string? queryParam)
    {
        return queryParam?.Trim()
            .Split(',')
            .Select(s => s.Trim())
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .Select(s => Guid.Parse(s))
            .Distinct()
            .ToList();
    }
}