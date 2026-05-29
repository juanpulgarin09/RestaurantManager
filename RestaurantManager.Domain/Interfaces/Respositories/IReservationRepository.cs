using RestaurantManager.Domain.Entities;

namespace RestaurantManager.Domain.Interfaces.Repositories;

public interface IReservationRepository : IGenericRepository<Reservation>
{
    Task<IEnumerable<Reservation>> GetAllWithDetailsAsync();
    Task<Reservation?> GetByIdWithDetailsAsync(int id);
}