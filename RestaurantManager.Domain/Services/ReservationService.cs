using Microsoft.Extensions.Logging;
using RestaurantManager.Domain.Entities;
using RestaurantManager.Domain.Enums;
using RestaurantManager.Domain.Interfaces.Repositories;
using RestaurantManager.Domain.Interfaces.Services;

namespace RestaurantManager.Domain.Services;

public class ReservationService : IReservationService
{
    private readonly IReservationRepository _reservationRepository;
    private readonly ITableRepository _tableRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly ILogger<ReservationService> _logger;

    public ReservationService(
        IReservationRepository reservationRepository,
        ITableRepository tableRepository,
        ICustomerRepository customerRepository,
        ILogger<ReservationService> logger)
    {
        _reservationRepository = reservationRepository;
        _tableRepository = tableRepository;
        _customerRepository = customerRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<Reservation>> GetAllAsync()
    {
        _logger.LogInformation("Retrieving all reservations");
        return await _reservationRepository.GetAllWithDetailsAsync();
    }

    public async Task<Reservation?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Retrieving reservation with ID: {Id}", id);
        return await _reservationRepository.GetByIdWithDetailsAsync(id);
    }

    public async Task<Reservation> CreateAsync(Reservation reservation)
    {
        // REGLA 1: Fecha futura
        if (reservation.ReservationDate <= DateTime.UtcNow)
            throw new InvalidOperationException(
                "La fecha de reserva debe ser futura.");

        // REGLA 2: Comensales válidos
        if (reservation.GuestCount <= 0)
            throw new InvalidOperationException(
                "El número de comensales debe ser mayor a 0.");

        // REGLA 3: Cliente existe
        var customerExists = await _customerRepository.ExistsAsync(reservation.CustomerId);
        if (!customerExists)
            throw new KeyNotFoundException(
                $"No se encontró el cliente con ID {reservation.CustomerId}");

        // REGLA 4: Mesa existe
        var table = await _tableRepository.GetByIdAsync(reservation.TableId);
        if (table == null)
            throw new KeyNotFoundException(
                $"No se encontró la mesa con ID {reservation.TableId}");

        // REGLA 5: Mesa disponible
        if (table.Status != TableStatus.Available)
            throw new InvalidOperationException(
                "La mesa no está disponible.");

        // REGLA 6: Capacidad suficiente
        if (reservation.GuestCount > table.Capacity)
            throw new InvalidOperationException(
                $"La mesa solo tiene capacidad para {table.Capacity} personas.");

        // Cambiar estado de la mesa
        table.Status = TableStatus.Reserved;
        await _tableRepository.UpdateAsync(table);

        reservation.Status = ReservationStatus.Confirmed;

        _logger.LogInformation(
            "Creating reservation for customer {CustomerId} on table {TableId}",
            reservation.CustomerId, reservation.TableId);

        return await _reservationRepository.CreateAsync(reservation);
    }

    public async Task UpdateAsync(int id, Reservation reservation)
    {
        var existing = await _reservationRepository.GetByIdAsync(id);
        if (existing == null)
            throw new KeyNotFoundException($"No se encontró la reserva con ID {id}");

        // Si se cancela o completa, liberar la mesa
        if (reservation.Status == ReservationStatus.Cancelled ||
            reservation.Status == ReservationStatus.Completed)
        {
            var table = await _tableRepository.GetByIdAsync(existing.TableId);
            if (table != null)
            {
                table.Status = TableStatus.Available;
                await _tableRepository.UpdateAsync(table);
            }
        }

        existing.ReservationDate = reservation.ReservationDate;
        existing.GuestCount = reservation.GuestCount;
        existing.Notes = reservation.Notes;
        existing.Status = reservation.Status;

        _logger.LogInformation("Updating reservation with ID: {Id}", id);
        await _reservationRepository.UpdateAsync(existing);
    }

    public async Task DeleteAsync(int id)
    {
        var exists = await _reservationRepository.ExistsAsync(id);
        if (!exists)
            throw new KeyNotFoundException($"No se encontró la reserva con ID {id}");

        _logger.LogInformation("Deleting reservation with ID: {Id}", id);
        await _reservationRepository.DeleteAsync(id);
    }
}