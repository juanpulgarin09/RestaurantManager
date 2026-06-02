using Microsoft.EntityFrameworkCore;
using RestaurantManager.DataAccess.Context;
using RestaurantManager.Domain.Entities;
using RestaurantManager.Domain.Interfaces.Repositories;

namespace RestaurantManager.DataAccess.Repositories;

public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
{
    public CustomerRepository(RestaurantDbContext context) : base(context) { }

    public async Task<Customer?> GetByEmailAsync(string email) =>
        await _dbSet
            .FirstOrDefaultAsync(c => c.Email.ToLower() == email.ToLower());
}