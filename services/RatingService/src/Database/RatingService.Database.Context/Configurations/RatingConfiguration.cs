using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RatingService.Database.Models;

namespace RatingService.Database.Context.Configurations;

public class RatingConfiguration : IEntityTypeConfiguration<Rating>
{
    public void Configure(EntityTypeBuilder<Rating> builder)
    {
        builder.ToTable("rating");
        
        builder.HasIndex(rating => rating.Id).IsUnique();
        builder.HasKey(rating => rating.Id);

        builder.Property(rating => rating.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.HasIndex(rating => rating.UserName).IsUnique();
        
        builder.Property(rating => rating.UserName)
            .HasColumnName("username")
            .IsRequired();

        builder.Property(rating => rating.Stars)
            .HasColumnName("stars")
            .IsRequired();
    }
}