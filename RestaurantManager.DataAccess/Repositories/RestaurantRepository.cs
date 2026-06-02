using Microsoft.EntityFrameworkCore;
using RestaurantManager.DataAccess.Context;
using RestaurantManager.Domain.Entities;
using RestaurantManager.Domain.Interfaces.Repositories;

namespace RestaurantManager.DataAccess.Repositories;

public class RestaurantRepository : GenericRepository<Restaurant>, IRestaurantRepository
{
    public RestaurantRepository(RestaurantDbContext context) : base(context) { }

    public async Task<Restaurant?> GetByNameAsync(string name) =>
        await _dbSet
            .FirstOrDefaultAsync(r => r.Name.ToLower() == name.ToLower());

    public async Task<Restaurant?> GetByIdWithTablesAsync(int id) =>
        await _dbSet
            .Include(r => r.Tables)
            .FirstOrDefaultAsync(r => r.Id == id);
}