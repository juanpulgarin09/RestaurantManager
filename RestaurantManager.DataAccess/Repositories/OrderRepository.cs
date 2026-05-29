using Microsoft.EntityFrameworkCore;
using RestaurantManager.DataAccess.Context;
using RestaurantManager.Domain.Entities;
using RestaurantManager.Domain.Interfaces.Respositories;

namespace RestaurantManager.DataAccess.Repositories;

public class OrderRepository : GenericRepository<Order>, IOrderRepository
{
    public OrderRepository(RestaurantDbContext context) : base(context) { }

    public async Task<Order?> GetOrderWithItemsAsync(int id) =>
        await _context.Orders
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuItem)
            .Include(o => o.Reservation)
            .FirstOrDefaultAsync(o => o.Id == id);
}