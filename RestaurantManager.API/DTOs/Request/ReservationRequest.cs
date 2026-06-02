using RestaurantManager.Domain.Enums;

namespace RestaurantManager.API.DTOs.Request;

public class CreateReservationRequest
{
    public DateTime ReservationDate { get; set; }
    public int GuestCount { get; set; }
    public string? Notes { get; set; }
    public int CustomerId { get; set; }
    public int TableId { get; set; }
}

public class UpdateReservationRequest : CreateReservationRequest
{
    public ReservationStatus Status { get; set; }
}