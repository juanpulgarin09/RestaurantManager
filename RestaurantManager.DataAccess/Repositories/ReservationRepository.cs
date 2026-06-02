using Microsoft.EntityFrameworkCore;
using RestaurantManager.DataAccess.Context;
using RestaurantManager.Domain.Entities;
using RestaurantManager.Domain.Interfaces.Repositories;

namespace RestaurantManager.DataAccess.Repositories;

public class ReservationRepository : GenericRepository<Reservation>, IReservationRepository
{
    public ReservationRepository(RestaurantDbContext context) : base(context) { }

    public async Task<IEnumerable<Reservation>> GetAllWithDetailsAsync() =>
        await _dbSet
            .Include(r => r.Customer)
            .Include(r => r.Table)
                .ThenInclude(t => t.Restaurant)
            .OrderByDescending(r => r.ReservationDate)
            .ToListAsync();

    public async Task<Reservation?> GetByIdWithDetailsAsync(int id) =>
        await _dbSet
            .Include(r => r.Customer)
            .Include(r => r.Table)
                .ThenInclude(t => t.Restaurant)
            .Include(r => r.Order)
                .ThenInclude(o => o!.OrderItems)
                    .ThenInclude(oi => oi.MenuItem)
            .FirstOrDefaultAsync(r => r.Id == id);
}