using Microsoft.EntityFrameworkCore;
using RestaurantManager.DataAccess.Context;
using RestaurantManager.Domain.Entities;
using RestaurantManager.Domain.Enums;
using RestaurantManager.Domain.Interfaces.Repositories;

namespace RestaurantManager.DataAccess.Repositories;

public class TableRepository : GenericRepository<Table>, ITableRepository
{
    public TableRepository(RestaurantDbContext context) : base(context) { }

    public async Task<IEnumerable<Table>> GetByRestaurantAsync(int restaurantId) =>
        await _dbSet
            .Where(t => t.RestaurantId == restaurantId)
            .Include(t => t.Restaurant)
            .OrderBy(t => t.Number)
            .ToListAsync();

    public async Task<IEnumerable<Table>> GetAvailableByRestaurantAsync(int restaurantId) =>
        await _dbSet
            .Where(t => t.RestaurantId == restaurantId
                     && t.Status == TableStatus.Available)
            .OrderBy(t => t.Number)
            .ToListAsync();
}