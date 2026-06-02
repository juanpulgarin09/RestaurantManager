using Microsoft.EntityFrameworkCore;
using RestaurantManager.DataAccess.Context;
using RestaurantManager.Domain.Entities;
using RestaurantManager.Domain.Enums;
using RestaurantManager.Domain.Interfaces.Repositories;

namespace RestaurantManager.DataAccess.Repositories;

public class MenuItemRepository : GenericRepository<MenuItem>, IMenuItemRepository
{
    public MenuItemRepository(RestaurantDbContext context) : base(context) { }

    public async Task<IEnumerable<MenuItem>> GetByCategoryAsync(MenuCategory category) =>
        await _dbSet
            .Where(m => m.Category == category)
            .OrderBy(m => m.Name)
            .ToListAsync();

    public async Task<IEnumerable<MenuItem>> GetAvailableAsync() =>
        await _dbSet
            .Where(m => m.IsAvailable)
            .OrderBy(m => m.Category)
            .ThenBy(m => m.Name)
            .ToListAsync();
}