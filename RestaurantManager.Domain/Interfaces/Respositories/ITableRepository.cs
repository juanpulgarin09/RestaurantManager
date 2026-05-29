using RestaurantManager.Domain.Entities;
using RestaurantManager.Domain.Enums;

namespace RestaurantManager.Domain.Interfaces.Repositories;

public interface ITableRepository : IGenericRepository<Table>
{
    Task<IEnumerable<Table>> GetByRestaurantAsync(int restaurantId);
    Task<IEnumerable<Table>> GetAvailableByRestaurantAsync(int restaurantId);
}