using RestaurantManager.Domain.Common;

namespace RestaurantManager.Domain.Entities;

public class Order : AuditBase
{
    public decimal TotalAmount { get; set; }

    // FK 1:1 con Reservation
    public int ReservationId { get; set; }
    public Reservation Reservation { get; set; } = null!;

    // Navegación N:M
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}