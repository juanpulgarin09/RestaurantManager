using RestaurantManager.Domain.Entities;
using RestaurantManager.Domain.Enums;

namespace RestaurantManager.Domain.Interfaces.Repositories;

public interface IMenuItemRepository : IGenericRepository<MenuItem>
{
    Task<IEnumerable<MenuItem>> GetByCategoryAsync(MenuCategory category);
    Task<IEnumerable<MenuItem>> GetAvailableAsync();
}