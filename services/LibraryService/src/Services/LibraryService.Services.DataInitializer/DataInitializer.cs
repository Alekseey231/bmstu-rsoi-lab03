using LibraryService.Core.Interfaces;
using LibraryService.Database.Context;
using LibraryService.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryService.Services.DataInitializer;

public class DataInitializer : IDataInitializer
{
    private readonly LibraryServiceContext _context;
    
    public DataInitializer(LibraryServiceContext context)
    {
        _context = context;
    }
    
    public async Task InitializeAsync()
    {
        var library = new Library(Guid.Parse("83575e12-7ce0-48ee-9931-51919ff3c9ee"),
            "Библиотека имени 7 Непьющих",
            "Москва",
            "2-я Бауманская ул., д.5, стр.1");

        var book = new Book(Guid.Parse("f7cdc58f-2caf-4b15-9727-f89dcc629b27"),
            "Краткий курс C++ в 7 томах",
            "Бьерн Страуструп",
            "Научная фантастика",
            0);

        var libraryBook = new LibraryBook(1, 1, 1);
        
        await _context.Libraries.AddAsync(library);
        await _context.Books.AddAsync(book);
        await _context.LibraryBooks.AddAsync(libraryBook);
        await _context.SaveChangesAsync();
    }
 }