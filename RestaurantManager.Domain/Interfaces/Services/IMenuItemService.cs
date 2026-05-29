using RestaurantManager.Domain.Entities;

namespace RestaurantManager.Domain.Interfaces.Services;

public interface IMenuItemService
{
    Task<IEnumerable<MenuItem>> GetAllAsync();
    Task<MenuItem?> GetByIdAsync(int id);
    Task<MenuItem> CreateAsync(MenuItem menuItem);
    Task UpdateAsync(int id, MenuItem menuItem);
    Task DeleteAsync(int id);
}