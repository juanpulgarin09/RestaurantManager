using Microsoft.EntityFrameworkCore;
using RestaurantManager.DataAccess.Context;
using RestaurantManager.Domain.Entities;
using RestaurantManager.Domain.Interfaces.Respositories;

namespace RestaurantManager.DataAccess.Repositories;

public class ReservationRepository : GenericRepository<Reservation>, IReservationRepository
{
    public ReservationRepository(RestaurantDbContext context) : base(context) { }

    public async Task<IEnumerable<Reservation>> GetReservationsWithDetailsAsync() =>
        await _context.Reservations
            .Include(r => r.Customer)
            .Include(r => r.Table)
            .ToListAsync();

    public async Task<Reservation?> GetReservationWithDetailsAsync(int id) =>
        await _context.Reservations
            .Include(r => r.Customer)
            .Include(r => r.Table)
            .Include(r => r.Order)
                .ThenInclude(o => o!.OrderItems)
                    .ThenInclude(oi => oi.MenuItem)
            .FirstOrDefaultAsync(r => r.Id == id);
}