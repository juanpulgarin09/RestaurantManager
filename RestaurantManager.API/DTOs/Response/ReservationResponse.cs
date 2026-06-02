using RestaurantManager.Domain.Enums;

namespace RestaurantManager.API.DTOs.Response;

public class ReservationResponse
{
    public int Id { get; set; }
    public DateTime ReservationDate { get; set; }
    public int GuestCount { get; set; }
    public ReservationStatus Status { get; set; }
    public string? Notes { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public int TableId { get; set; }
    public int TableNumber { get; set; }
    public string RestaurantName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}