using RestaurantManager.Domain.Enums;

namespace RestaurantManager.API.DTOs.Request;

public class CreateMenuItemRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public MenuCategory Category { get; set; }
}

public class UpdateMenuItemRequest : CreateMenuItemRequest
{
    public bool IsAvailable { get; set; }
}