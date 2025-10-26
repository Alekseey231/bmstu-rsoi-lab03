namespace LibraryService.Core.Models;

public class Library
{
    public Guid LibraryId { get; set; }
    
    public string Name { get; set; }
    
    public string City { get; set; }
    
    public string Address { get; set; }
    
    public Library(Guid libraryId, string name, string city, string address)
    {
        LibraryId = libraryId;
        Name = name;
        City = city;
        Address = address;
    }
}