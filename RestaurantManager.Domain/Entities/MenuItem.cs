using RestaurantManager.Domain.Enums;

namespace RestaurantManager.Domain.Entities;

public class MenuItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public MenuCategory Category { get; set; }
    public bool IsAvailable { get; set; } = true;

    // Navegación N:M
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}