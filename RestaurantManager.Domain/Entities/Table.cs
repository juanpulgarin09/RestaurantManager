using RestaurantManager.Domain.Enums;

namespace RestaurantManager.Domain.Entities;

public class Table : AuditBase
{
    public int Number { get; set; }
    public int Capacity { get; set; }
    public TableStatus Status { get; set; } = TableStatus.Available;

    // FK
    public int RestaurantId { get; set; }
    public Restaurant Restaurant { get; set; } = null!;

    // Navegation Properties
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}