using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LibraryService.Database.Models;
using LibraryService.Database.Models.Enums;

namespace LibraryService.Database.Context.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasIndex(book => book.Id).IsUnique();
        builder.HasKey(book => book.Id);

        builder.Property(book => book.Id)
            .IsRequired()
            .HasColumnName("id");
        
        builder.Property(book => book.BookId)
            .IsRequired()
            .HasColumnName("book_uid");
        
        builder.Property(book => book.Name)
            .IsRequired()
            .HasColumnName("name");
        
        builder.Property(book => book.Author)
            .HasColumnName("author");
        
        builder.Property(book => book.Genre)
            .HasColumnName("genre");
        
        builder.Property(book => book.BookCondition)
            .HasDefaultValue(BookCondition.Excellent)
            .HasColumnName("condition");
    }
}