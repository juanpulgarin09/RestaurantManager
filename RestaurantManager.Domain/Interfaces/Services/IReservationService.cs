using RestaurantManager.Domain.Entities;

namespace RestaurantManager.Domain.Interfaces.Services;

public interface IReservationService
{
    Task<IEnumerable<Reservation>> GetAllAsync();
    Task<Reservation?> GetByIdAsync(int id);
    Task<Reservation> CreateAsync(Reservation reservation);
    Task UpdateAsync(int id, Reservation reservation);
    Task DeleteAsync(int id);
}