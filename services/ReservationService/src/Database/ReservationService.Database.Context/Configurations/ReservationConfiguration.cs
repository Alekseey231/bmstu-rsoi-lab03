using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReservationService.Database.Models;

namespace ReservationService.Database.Context.Configurations;

public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.ToTable("reservation");
        
        builder.HasIndex(reservation => reservation.Id).IsUnique();
        builder.HasKey(reservation => reservation.Id);
        
        builder.HasIndex(reservation => reservation.ReservationId).IsUnique();

        builder.Property(reservation => reservation.Id)
            .IsRequired()
            .HasColumnName("id");
        
        builder.Property(reservation => reservation.ReservationId)
            .IsRequired()
            .HasColumnName("reservation_uid");
        
        builder.Property(reservation => reservation.UserName)
            .IsRequired()
            .HasColumnName("username");
        
        builder.Property(reservation => reservation.Status)
            .IsRequired()
            .HasColumnName("status");
        
        builder.Property(reservation => reservation.StartDate)
            .IsRequired()
            .HasColumnName("start_date");
        
        builder.Property(reservation => reservation.TillDate)
            .IsRequired()
            .HasColumnName("till_date");
        
        builder.Property(reservation => reservation.BookId)
            .IsRequired()
            .HasColumnName("book_uid");
        
        builder.Property(reservation => reservation.LibraryId)
            .IsRequired()
            .HasColumnName("library_uid");
    }
}