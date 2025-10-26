using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LibraryService.Database.Models;
using LibraryService.Database.Models.Enums;

namespace LibraryService.Database.Context.Configurations;

public class LibraryConfiguration : IEntityTypeConfiguration<Library>
{
    public void Configure(EntityTypeBuilder<Library> builder)
    {
        builder.HasIndex(library => library.Id).IsUnique();
        builder.HasKey(library => library.Id);

        builder.Property(library => library.Id)
            .IsRequired()
            .HasColumnName("id");
        
        builder.Property(library => library.LibraryId)
            .IsRequired()
            .HasColumnName("library_uid");
        
        builder.Property(library => library.Name)
            .IsRequired()
            .HasColumnName("name");
        
        builder.Property(library => library.City)
            .IsRequired()
            .HasColumnName("city");
        
        builder.Property(library => library.Address)
            .IsRequired()
            .HasColumnName("address");
    }
}