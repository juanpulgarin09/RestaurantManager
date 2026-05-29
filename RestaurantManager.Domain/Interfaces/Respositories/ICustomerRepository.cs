using RestaurantManager.Domain.Entities;

namespace RestaurantManager.Domain.Interfaces.Repositories;

public interface ICustomerRepository : IGenericRepository<Customer>
{
    Task<Customer?> GetByEmailAsync(string email);
}