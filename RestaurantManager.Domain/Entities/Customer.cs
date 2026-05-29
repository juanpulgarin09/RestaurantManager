namespace RestaurantManager.Domain.Entities;

public class Customer : AuditBase
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;

    // Navegation Properties
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}