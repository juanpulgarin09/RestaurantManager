using RestaurantManager.Domain.Common;

namespace RestaurantManager.Domain.Entities;

public class OrderItem : AuditBase
{
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }

    // FKS
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;

    public int MenuItemId { get; set; }
    public MenuItem MenuItem { get; set; } = null!;
}