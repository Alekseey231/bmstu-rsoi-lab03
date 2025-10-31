using Microsoft.EntityFrameworkCore;
using ReservationService.Core.Exceptions;
using ReservationService.Core.Interfaces;
using ReservationService.Core.Models;
using ReservationService.Core.Models.Enums;
using ReservationService.Database.Context;
using ReservationService.Database.Repositories.Converters;
using ReservationService.Database.Repositories.Converters.Enums;
using DbReservation = ReservationService.Database.Models.Reservation;

namespace ReservationService.Database.Repositories;

public class ReservationRepository : IReservationRepository
{
    private readonly ReservationServiceContext _context;

    public ReservationRepository(ReservationServiceContext context)
    {
        _context = context;
    }

    public async Task<Reservation> CreateReservationAsync(Guid reservationId,
        string userName,
        Guid bookId,
        Guid libraryId, 
        ReservationStatus status,
        DateTime startDate, 
        DateTime tillDate)
    {
        var isExists = await _context.Reservation.AnyAsync(r => r.ReservationId == reservationId);
        if (isExists)
            throw new ReservationAlreadyExistsException();
        
        var dbReservation = new DbReservation(reservationId,
            userName,
            bookId,
            libraryId,
            ReservationStatusConverter.Convert(status),
            startDate,
            tillDate);
        
        await _context.Reservation.AddAsync(dbReservation);
        await _context.SaveChangesAsync();
        
        return ReservationConverter.Convert(dbReservation);
    }

    public async Task<Reservation> UpdateReservationAsync(Guid reservationId, ReservationStatus status)
    {
        var reservation = await _context.Reservation.FirstOrDefaultAsync(r => r.ReservationId == reservationId);
        if (reservation is null)
            throw new ReservationNotFoundException();
        
        reservation.Status = ReservationStatusConverter.Convert(status);
        
        await _context.SaveChangesAsync();
        
        return ReservationConverter.Convert(reservation);
    }

    public async Task<Reservation> GetReservationByIdAsync(Guid reservationId)
    {
        var reservation = await _context.Reservation.FirstOrDefaultAsync(r => r.ReservationId == reservationId);
        if (reservation is null)
            throw new ReservationNotFoundException();
        
        return ReservationConverter.Convert(reservation);
    }

    public async Task<List<Reservation>> GetReservationByUserNameAsync(string userName, ReservationStatus? status)
    {
        var query = _context.Reservation.Where(r => r.UserName == userName);
        
        if (status is not null)
        {
            var dbStatus = ReservationStatusConverter.Convert(status.Value);
            query = query.Where(r => r.Status == dbStatus);
        }
        
        var reservations = await query.ToListAsync();
        
        return reservations.ConvertAll(ReservationConverter.Convert);
    }

    public Task DeleteReservationAsync(Guid reservationId)
    {
        var reservation = _context.Reservation.FirstOrDefault(r => r.ReservationId == reservationId);
        if (reservation is null)
            throw new ReservationNotFoundException();
        
        _context.Reservation.Remove(reservation);
        
        return _context.SaveChangesAsync();
    }
}