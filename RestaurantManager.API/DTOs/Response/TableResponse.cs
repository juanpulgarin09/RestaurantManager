using RestaurantManager.Domain.Enums;

namespace RestaurantManager.API.DTOs.Response;

public class TableResponse
{
    public int Id { get; set; }
    public int Number { get; set; }
    public int Capacity { get; set; }
    public TableStatus Status { get; set; }
    public int RestaurantId { get; set; }
    public string RestaurantName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}