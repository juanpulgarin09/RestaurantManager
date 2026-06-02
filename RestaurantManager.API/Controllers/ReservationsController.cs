using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RestaurantManager.API.DTOs.Request;
using RestaurantManager.API.DTOs.Response;
using RestaurantManager.Domain.Entities;
using RestaurantManager.Domain.Interfaces.Services;

namespace RestaurantManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    private readonly IReservationService _reservationService;
    private readonly IMapper _mapper;

    public ReservationsController(IReservationService reservationService, IMapper mapper)
    {
        _reservationService = reservationService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReservationResponse>>> GetAll()
    {
        var reservations = await _reservationService.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<ReservationResponse>>(reservations));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ReservationResponse>> GetById(int id)
    {
        var reservation = await _reservationService.GetByIdAsync(id);
        if (reservation == null)
            return NotFound(new { message = $"Reserva con ID {id} no encontrada" });

        return Ok(_mapper.Map<ReservationResponse>(reservation));
    }

    [HttpPost]
    public async Task<ActionResult<ReservationResponse>> Create(CreateReservationRequest dto)
    {
        try
        {
            var entity = _mapper.Map<Reservation>(dto);
            var created = await _reservationService.CreateAsync(entity);

            // Recargar con detalles para el response
            var withDetails = await _reservationService.GetByIdAsync(created.Id);
            var response = _mapper.Map<ReservationResponse>(withDetails);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, UpdateReservationRequest dto)
    {
        try
        {
            var entity = _mapper.Map<Reservation>(dto);
            await _reservationService.UpdateAsync(id, entity);
            return NoContent();
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            await _reservationService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }
}