using Microsoft.EntityFrameworkCore;
using RestaurantManager.DataAccess.Context;
using RestaurantManager.Domain.Entities;
using RestaurantManager.Domain.Interfaces.Repositories;

namespace RestaurantManager.DataAccess.Repositories;

public class OrderRepository : GenericRepository<Order>, IOrderRepository
{
    public OrderRepository(RestaurantDbContext context) : base(context) { }

    public async Task<Order?> GetByIdWithItemsAsync(int id) =>
        await _dbSet
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuItem)
            .Include(o => o.Reservation)
            .FirstOrDefaultAsync(o => o.Id == id);

    public async Task<Order?> GetByReservationAsync(int reservationId) =>
        await _dbSet
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuItem)
            .FirstOrDefaultAsync(o => o.ReservationId == reservationId);
}