namespace RestaurantManager.Domain.Entities;

public class Order
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public decimal TotalAmount { get; set; }

    // FK 1:1 con Reservation
    public int ReservationId { get; set; }
    public Reservation Reservation { get; set; } = null!;

    // Navegación N:M
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}