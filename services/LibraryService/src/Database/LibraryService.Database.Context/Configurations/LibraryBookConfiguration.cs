using LibraryService.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryService.Database.Context.Configurations;

public class LibraryBookConfiguration : IEntityTypeConfiguration<LibraryBook>
{
    public void Configure(EntityTypeBuilder<LibraryBook> builder)
    {
        builder.ToTable("library_books");
        
        builder.HasIndex(link => new
        {
            link.LibraryId,
            link.BookId,
        }).IsUnique();
        builder.HasKey(link => new
        {
            link.LibraryId,
            link.BookId,
        });
        
        builder.Property(link => link.BookId)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(link => link.LibraryId)
            .IsRequired()
            .ValueGeneratedNever();
        
        builder.Property(link => link.AvailableCount)
            .IsRequired()
            .HasColumnName("available_count");
        
        builder.HasOne(link => link.Book)
            .WithMany(book => book.LibrariesBooks)
            .HasForeignKey(link => link.BookId);

        builder.HasOne(link => link.Library)
            .WithMany(book => book.LibrariesBooks)
            .HasForeignKey(link => link.LibraryId);

        builder.Property(link => link.BookId).HasColumnName("book_id");
        builder.Property(link => link.LibraryId).HasColumnName("library_id");
    }
}