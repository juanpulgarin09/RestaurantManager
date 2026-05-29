using Microsoft.Extensions.Logging;
using RestaurantManager.Domain.Entities;
using RestaurantManager.Domain.Interfaces.Repositories;
using RestaurantManager.Domain.Interfaces.Services;

namespace RestaurantManager.Domain.Services;

public class TableService : ITableService
{
    private readonly ITableRepository _tableRepository;
    private readonly IRestaurantRepository _restaurantRepository;
    private readonly ILogger<TableService> _logger;

    public TableService(
        ITableRepository tableRepository,
        IRestaurantRepository restaurantRepository,
        ILogger<TableService> logger)
    {
        _tableRepository = tableRepository;
        _restaurantRepository = restaurantRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<Table>> GetAllAsync()
    {
        _logger.LogInformation("Retrieving all tables");
        return await _tableRepository.GetAllAsync();
    }

    public async Task<Table?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Retrieving table with ID: {Id}", id);
        return await _tableRepository.GetByIdAsync(id);
    }

    public async Task<Table> CreateAsync(Table table)
    {
        if (table.Capacity <= 0)
            throw new InvalidOperationException("La capacidad debe ser mayor a 0.");

        var restaurantExists = await _restaurantRepository.ExistsAsync(table.RestaurantId);
        if (!restaurantExists)
            throw new KeyNotFoundException(
                $"No se encontró el restaurante con ID {table.RestaurantId}");

        _logger.LogInformation("Creating table number {Number}", table.Number);
        return await _tableRepository.CreateAsync(table);
    }

    public async Task UpdateAsync(int id, Table table)
    {
        var existing = await _tableRepository.GetByIdAsync(id);
        if (existing == null)
            throw new KeyNotFoundException($"No se encontró la mesa con ID {id}");

        existing.Number = table.Number;
        existing.Capacity = table.Capacity;
        existing.Status = table.Status;

        _logger.LogInformation("Updating table with ID: {Id}", id);
        await _tableRepository.UpdateAsync(existing);
    }

    public async Task DeleteAsync(int id)
    {
        var exists = await _tableRepository.ExistsAsync(id);
        if (!exists)
            throw new KeyNotFoundException($"No se encontró la mesa con ID {id}");

        _logger.LogInformation("Deleting table with ID: {Id}", id);
        await _tableRepository.DeleteAsync(id);
    }
}