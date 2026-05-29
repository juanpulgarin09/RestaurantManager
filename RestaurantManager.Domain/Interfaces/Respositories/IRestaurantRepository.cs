using RestaurantManager.Domain.Entities;

namespace RestaurantManager.Domain.Interfaces.Repositories;

public interface IRestaurantRepository : IGenericRepository<Restaurant>
{
    Task<Restaurant?> GetByNameAsync(string name);
    Task<Restaurant?> GetByIdWithTablesAsync(int id);
}