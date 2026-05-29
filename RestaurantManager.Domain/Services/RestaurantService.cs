using Microsoft.Extensions.Logging;
using RestaurantManager.Domain.Entities;
using RestaurantManager.Domain.Interfaces.Repositories;
using RestaurantManager.Domain.Interfaces.Services;

namespace RestaurantManager.Domain.Services;

public class RestaurantService : IRestaurantService
{
    private readonly IRestaurantRepository _restaurantRepository;
    private readonly ILogger<RestaurantService> _logger;

    public RestaurantService(
        IRestaurantRepository restaurantRepository,
        ILogger<RestaurantService> logger)
    {
        _restaurantRepository = restaurantRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<Restaurant>> GetAllAsync()
    {
        _logger.LogInformation("Retrieving all restaurants");
        return await _restaurantRepository.GetAllAsync();
    }

    public async Task<Restaurant?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Retrieving restaurant with ID: {Id}", id);
        return await _restaurantRepository.GetByIdAsync(id);
    }

    public async Task<Restaurant> CreateAsync(Restaurant restaurant)
    {
        var existing = await _restaurantRepository.GetByNameAsync(restaurant.Name);
        if (existing != null)
            throw new InvalidOperationException(
                $"Ya existe un restaurante con el nombre '{restaurant.Name}'");

        _logger.LogInformation("Creating restaurant: {Name}", restaurant.Name);
        return await _restaurantRepository.CreateAsync(restaurant);
    }

    public async Task UpdateAsync(int id, Restaurant restaurant)
    {
        var existing = await _restaurantRepository.GetByIdAsync(id);
        if (existing == null)
            throw new KeyNotFoundException($"No se encontró el restaurante con ID {id}");

        if (existing.Name != restaurant.Name)
        {
            var nameConflict = await _restaurantRepository.GetByNameAsync(restaurant.Name);
            if (nameConflict != null)
                throw new InvalidOperationException(
                    $"Ya existe un restaurante con el nombre '{restaurant.Name}'");
        }

        existing.Name = restaurant.Name;
        existing.Address = restaurant.Address;
        existing.Phone = restaurant.Phone;
        existing.Email = restaurant.Email;

        _logger.LogInformation("Updating restaurant with ID: {Id}", id);
        await _restaurantRepository.UpdateAsync(existing);
    }

    public async Task DeleteAsync(int id)
    {
        var exists = await _restaurantRepository.ExistsAsync(id);
        if (!exists)
            throw new KeyNotFoundException($"No se encontró el restaurante con ID {id}");

        _logger.LogInformation("Deleting restaurant with ID: {Id}", id);
        await _restaurantRepository.DeleteAsync(id);
    }
}