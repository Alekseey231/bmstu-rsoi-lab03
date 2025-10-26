using LibraryService.Core;
using Microsoft.EntityFrameworkCore;
using LibraryService.Core.Exceptions;
using LibraryService.Core.Interfaces;
using LibraryService.Core.Models;
using LibraryService.Core.Models.Enums;
using LibraryService.Database.Context;
using LibraryService.Database.Repositories.Converters;
using LibraryService.Database.Repositories.Converters.Enums;

namespace LibraryService.Database.Repositories;

public class LibraryRepository : ILibraryRepository
{
    private readonly LibraryServiceContext _context;

    public LibraryRepository(LibraryServiceContext context)
    {
        _context = context;
    }

    public async Task<List<Library>> GetAllLibrariesAsync(string city, int? limit = null, int? offset = null)
    {
        offset ??= 0;
        limit ??= int.MaxValue;
        
        var libraries = await _context.Libraries
            .Where(l => l.City == city)
            .OrderBy(l => l.Name)
            .Skip(offset.Value)
            .Take(limit.Value)
            .ToListAsync();
        
        return libraries.ConvertAll(LibraryConverter.Convert);
    }

    public Task<int> GetCountOfLibrariesAsync(string city)
    {
        return _context.Libraries.CountAsync(l => l.City == city);
    }

    public async Task<List<InventoryItem>> GetLibraryBooksAsync(Guid libraryUid, bool? showAll, int? offset = null, int? limit = null)
    {
        var library = await _context.Libraries.FirstOrDefaultAsync(l => l.LibraryId == libraryUid);
        if (library is null)
            throw new LibraryNotFoundException();
        
        offset ??= 0;
        limit ??= int.MaxValue;

        var query = _context.LibraryBooks
            .Include(lb => lb.Book)
            .Where(lb => lb.LibraryId == library.Id);
        
        if (showAll is not true)
        {
            query = query.Where(lb => lb.AvailableCount > 0);
        }

        var books = await query.Skip(offset.Value)
            .Take(limit.Value)
            .ToListAsync();
        
        return books.Select(lb => new InventoryItem(
            BookConverter.Convert(lb.Book!),
            lb.AvailableCount
        )).ToList();
    }
    public async Task<int> GetCountOfLibraryBooksAsync(Guid libraryUid, bool? showAll)
    {
        var library = await _context.Libraries
            .FirstOrDefaultAsync(l => l.LibraryId == libraryUid);
        
        if (library is null)
            throw new LibraryNotFoundException();

        var query = _context.LibraryBooks
            .Where(lb => lb.LibraryId == library.Id);

        if (showAll is not true)
        {
            query = query.Where(lb => lb.AvailableCount > 0);
        }

        return await query.CountAsync();
    }

    public async Task<InventoryItem> CheckOutBookAsync(Guid libraryUid, Guid bookUid)
    {
        //TODO: fix concurrently
        var library = await _context.Libraries
            .FirstOrDefaultAsync(l => l.LibraryId == libraryUid);
        
        if (library is null)
            throw new LibraryNotFoundException();

        var book = await _context.Books
            .FirstOrDefaultAsync(b => b.BookId == bookUid);
        
        if (book is null)
            throw new BookNotFoundException();

        var libraryBook = await _context.LibraryBooks
            .FirstOrDefaultAsync(lb => 
                lb.LibraryId == library.Id && 
                lb.BookId == book.Id);
        
        if (libraryBook is null)
            throw new BookNotFoundInLibraryException();

        if (libraryBook.AvailableCount <= 0)
            throw new BookNotAvailableException();

        libraryBook.AvailableCount--;
        
        await _context.SaveChangesAsync();

        return new InventoryItem(BookConverter.Convert(book), libraryBook.AvailableCount);
    }

    public async Task<InventoryItem> CheckInBookAsync(Guid libraryUid, Guid bookUid, BookCondition bookCondition)
    {
        var newCondition = BookConditionConverter.Convert(bookCondition);
        
        var library = await _context.Libraries
            .FirstOrDefaultAsync(l => l.LibraryId == libraryUid);
        
        if (library is null)
            throw new LibraryNotFoundException();

        var book = await _context.Books
            .FirstOrDefaultAsync(b => b.BookId == bookUid);
        
        if (book is null)
            throw new BookNotFoundException();
        
        book.BookCondition = newCondition;

        var libraryBook = await _context.LibraryBooks
            .FirstOrDefaultAsync(lb => 
                lb.LibraryId == library.Id && 
                lb.BookId == book.Id);
        
        if (libraryBook is null)
            throw new BookNotFoundInLibraryException();

        libraryBook.AvailableCount++;
        
        await _context.SaveChangesAsync();
        
        return new InventoryItem(BookConverter.Convert(book), libraryBook.AvailableCount);
    }

    public async Task<InventoryItem> GetBookByIdAsync(Guid libraryId, Guid bookUid)
    {
        var library = await _context.Libraries
            .FirstOrDefaultAsync(l => l.LibraryId == libraryId);
        
        if (library is null)
            throw new LibraryNotFoundException();

        var book = await _context.Books
            .FirstOrDefaultAsync(b => b.BookId == bookUid);
        
        if (book is null)
            throw new BookNotFoundException();
        
        var libraryBook = await _context.LibraryBooks
            .FirstOrDefaultAsync(lb => 
                lb.LibraryId == library.Id && 
                lb.BookId == book.Id);
        
        if (libraryBook is null)
            throw new BookNotFoundInLibraryException();
        
        return new InventoryItem(BookConverter.Convert(book), libraryBook.AvailableCount);
    }
    
    public async Task<Library> GetLibraryByIdAsync(Guid libraryId)
    {
        var library = await _context.Libraries
            .FirstOrDefaultAsync(l => l.LibraryId == libraryId);
        
        if (library is null)
            throw new LibraryNotFoundException();
        
        return LibraryConverter.Convert(library);
    }
    
    public async Task<List<BookWithLibrary>> GetBooksWithLibrariesAsync(List<Guid> bookIds)
    {
        var booksWithLibraries = await _context.LibraryBooks
            .Include(lb => lb.Book)
            .Include(lb => lb.Library)
            .Where(lb => bookIds.Contains(lb.Book!.BookId))
            .Select(lb => new BookWithLibrary(new InventoryItem(BookConverter.Convert(lb.Book!), lb.AvailableCount),
                LibraryConverter.Convert(lb.Library!)))
            .ToListAsync();

        return booksWithLibraries;
    }
}