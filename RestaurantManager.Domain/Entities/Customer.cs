using RestaurantManager.Domain.Common;

namespace RestaurantManager.Domain.Entities;

public class Customer : AuditBase
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;

    // Navegación
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}