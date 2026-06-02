using RestaurantManager.Domain.Enums;

namespace RestaurantManager.API.DTOs.Request;

public class CreateTableRequest
{
    public int Number { get; set; }
    public int Capacity { get; set; }
    public int RestaurantId { get; set; }
}

public class UpdateTableRequest : CreateTableRequest
{
    public TableStatus Status { get; set; }
}