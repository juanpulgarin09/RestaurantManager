using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RestaurantManager.API.DTOs.Request;
using RestaurantManager.API.DTOs.Response;
using RestaurantManager.Domain.Entities;
using RestaurantManager.Domain.Interfaces.Services;

namespace RestaurantManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RestaurantsController : ControllerBase
{
    private readonly IRestaurantService _restaurantService;
    private readonly IMapper _mapper;

    public RestaurantsController(IRestaurantService restaurantService, IMapper mapper)
    {
        _restaurantService = restaurantService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RestaurantResponse>>> GetAll()
    {
        var restaurants = await _restaurantService.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<RestaurantResponse>>(restaurants));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RestaurantResponse>> GetById(int id)
    {
        var restaurant = await _restaurantService.GetByIdAsync(id);
        if (restaurant == null)
            return NotFound(new { message = $"Restaurante con ID {id} no encontrado" });

        return Ok(_mapper.Map<RestaurantResponse>(restaurant));
    }

    [HttpPost]
    public async Task<ActionResult<RestaurantResponse>> Create(CreateRestaurantRequest dto)
    {
        try
        {
            var entity = _mapper.Map<Restaurant>(dto);
            var created = await _restaurantService.CreateAsync(entity);
            var response = _mapper.Map<RestaurantResponse>(created);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, UpdateRestaurantRequest dto)
    {
        try
        {
            var entity = _mapper.Map<Restaurant>(dto);
            await _restaurantService.UpdateAsync(id, entity);
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
            await _restaurantService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }
}