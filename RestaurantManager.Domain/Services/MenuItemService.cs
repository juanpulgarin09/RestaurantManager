using Microsoft.Extensions.Logging;
using RestaurantManager.Domain.Entities;
using RestaurantManager.Domain.Interfaces.Repositories;
using RestaurantManager.Domain.Interfaces.Services;

namespace RestaurantManager.Domain.Services;

public class MenuItemService : IMenuItemService
{
    private readonly IMenuItemRepository _menuItemRepository;
    private readonly ILogger<MenuItemService> _logger;

    public MenuItemService(
        IMenuItemRepository menuItemRepository,
        ILogger<MenuItemService> logger)
    {
        _menuItemRepository = menuItemRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<MenuItem>> GetAllAsync()
    {
        _logger.LogInformation("Retrieving all menu items");
        return await _menuItemRepository.GetAllAsync();
    }

    public async Task<MenuItem?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Retrieving menu item with ID: {Id}", id);
        return await _menuItemRepository.GetByIdAsync(id);
    }

    public async Task<MenuItem> CreateAsync(MenuItem menuItem)
    {
        if (string.IsNullOrWhiteSpace(menuItem.Name))
            throw new InvalidOperationException("El nombre del ítem es obligatorio.");

        if (menuItem.Price <= 0)
            throw new InvalidOperationException("El precio debe ser mayor a 0.");

        _logger.LogInformation("Creating menu item: {Name}", menuItem.Name);
        return await _menuItemRepository.CreateAsync(menuItem);
    }

    public async Task UpdateAsync(int id, MenuItem menuItem)
    {
        var existing = await _menuItemRepository.GetByIdAsync(id);
        if (existing == null)
            throw new KeyNotFoundException($"No se encontró el ítem con ID {id}");

        if (menuItem.Price <= 0)
            throw new InvalidOperationException("El precio debe ser mayor a 0.");

        existing.Name = menuItem.Name;
        existing.Description = menuItem.Description;
        existing.Price = menuItem.Price;
        existing.Category = menuItem.Category;
        existing.IsAvailable = menuItem.IsAvailable;

        _logger.LogInformation("Updating menu item with ID: {Id}", id);
        await _menuItemRepository.UpdateAsync(existing);
    }

    public async Task DeleteAsync(int id)
    {
        var exists = await _menuItemRepository.ExistsAsync(id);
        if (!exists)
            throw new KeyNotFoundException($"No se encontró el ítem con ID {id}");

        _logger.LogInformation("Deleting menu item with ID: {Id}", id);
        await _menuItemRepository.DeleteAsync(id);
    }
}