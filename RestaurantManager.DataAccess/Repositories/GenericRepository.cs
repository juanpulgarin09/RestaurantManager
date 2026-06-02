using Microsoft.EntityFrameworkCore;
using RestaurantManager.DataAccess.Context;
using RestaurantManager.Domain.Entities;
using RestaurantManager.Domain.Interfaces.Repositories;

namespace RestaurantManager.DataAccess.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : AuditBase
{
    protected readonly RestaurantDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(RestaurantDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync() =>
        await _dbSet.ToListAsync();

    public async Task<T?> GetByIdAsync(int id) =>
        await _dbSet.FindAsync(id);

    public async Task<T> CreateAsync(T entity)
    {
        entity.CreatedAt = DateTime.UtcNow;
        entity.UpdatedAt = null;
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(T entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id) =>
        await _dbSet.AnyAsync(e => e.Id == id);
}