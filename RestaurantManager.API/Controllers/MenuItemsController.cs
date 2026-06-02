using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RestaurantManager.API.DTOs.Request;
using RestaurantManager.API.DTOs.Response;
using RestaurantManager.Domain.Entities;
using RestaurantManager.Domain.Interfaces.Services;

namespace RestaurantManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MenuItemsController : ControllerBase
{
    private readonly IMenuItemService _menuItemService;
    private readonly IMapper _mapper;

    public MenuItemsController(IMenuItemService menuItemService, IMapper mapper)
    {
        _menuItemService = menuItemService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MenuItemResponse>>> GetAll()
    {
        var items = await _menuItemService.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<MenuItemResponse>>(items));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MenuItemResponse>> GetById(int id)
    {
        var item = await _menuItemService.GetByIdAsync(id);
        if (item == null)
            return NotFound(new { message = $"Ítem con ID {id} no encontrado" });

        return Ok(_mapper.Map<MenuItemResponse>(item));
    }

    [HttpPost]
    public async Task<ActionResult<MenuItemResponse>> Create(CreateMenuItemRequest dto)
    {
        try
        {
            var entity = _mapper.Map<MenuItem>(dto);
            var created = await _menuItemService.CreateAsync(entity);
            var response = _mapper.Map<MenuItemResponse>(created);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }
        catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, UpdateMenuItemRequest dto)
    {
        try
        {
            var entity = _mapper.Map<MenuItem>(dto);
            await _menuItemService.UpdateAsync(id, entity);
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
            await _menuItemService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }
}