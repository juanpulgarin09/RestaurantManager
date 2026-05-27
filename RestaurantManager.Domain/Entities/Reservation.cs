using RestaurantManager.Domain.Enums;

namespace RestaurantManager.Domain.Entities;

public class Reservation
{
    public int Id { get; set; }
    public DateTime ReservationDate { get; set; }
    public int GuestCount { get; set; }
    public ReservationStatus Status { get; set; } = ReservationStatus.Pending;
    public string? Notes { get; set; }

    // FKs
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;

    public int TableId { get; set; }
    public Table Table { get; set; } = null!;

    // Navegación
    public Order? Order { get; set; }
}