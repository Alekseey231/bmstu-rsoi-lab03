using LibraryService.Database.Context.Configurations;
using Microsoft.EntityFrameworkCore;
using LibraryService.Database.Models;

namespace LibraryService.Database.Context;

public class LibraryServiceContext : DbContext
{
    public DbSet<Library> Libraries { get; set; }
    
    public DbSet<Book> Books { get; set; }
    
    public DbSet<LibraryBook> LibraryBooks { get; set; }
    
    public LibraryServiceContext(DbContextOptions<LibraryServiceContext> options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new BookConfiguration());
        modelBuilder.ApplyConfiguration(new LibraryBookConfiguration());
        modelBuilder.ApplyConfiguration(new LibraryConfiguration());
    }
}