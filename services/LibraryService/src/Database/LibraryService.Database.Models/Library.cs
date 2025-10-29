namespace LibraryService.Database.Models;

public class Library
{
    #region Properties
    
    public int Id { get; set; }
    
    public Guid LibraryId { get; set; }
    
    public string Name { get; set; }
    
    public string City { get; set; }
    
    public string Address { get; set; }
    
    #endregion
    
    #region Navigation Properties
    
    public ICollection<LibraryBook>? LibrariesBooks { get; set; }
    
    #endregion

    public Library(Guid libraryId, string name, string city, string address)
    {
        LibraryId = libraryId;
        Name = name;
        City = city;
        Address = address;
    }
}