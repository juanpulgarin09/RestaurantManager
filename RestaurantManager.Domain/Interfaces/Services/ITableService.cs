using RestaurantManager.Domain.Entities;

namespace RestaurantManager.Domain.Interfaces.Services;

public interface ITableService
{
    Task<IEnumerable<Table>> GetAllAsync();
    Task<Table?> GetByIdAsync(int id);
    Task<Table> CreateAsync(Table table);
    Task UpdateAsync(int id, Table table);
    Task DeleteAsync(int id);
}
