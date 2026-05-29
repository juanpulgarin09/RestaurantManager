using RestaurantManager.Domain.Entities;

namespace RestaurantManager.Domain.Interfaces.Repositories;

public interface IOrderRepository : IGenericRepository<Order>
{
    Task<Order?> GetByIdWithItemsAsync(int id);
    Task<Order?> GetByReservationAsync(int reservationId);
}