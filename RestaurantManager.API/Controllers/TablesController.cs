using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RestaurantManager.API.DTOs.Request;
using RestaurantManager.API.DTOs.Response;
using RestaurantManager.Domain.Entities;
using RestaurantManager.Domain.Interfaces.Services;

namespace RestaurantManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TablesController : ControllerBase
{
    private readonly ITableService _tableService;
    private readonly IMapper _mapper;

    public TablesController(ITableService tableService, IMapper mapper)
    {
        _tableService = tableService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TableResponse>>> GetAll()
    {
        var tables = await _tableService.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<TableResponse>>(tables));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TableResponse>> GetById(int id)
    {
        var table = await _tableService.GetByIdAsync(id);
        if (table == null)
            return NotFound(new { message = $"Mesa con ID {id} no encontrada" });

        return Ok(_mapper.Map<TableResponse>(table));
    }

    [HttpPost]
    public async Task<ActionResult<TableResponse>> Create(CreateTableRequest dto)
    {
        try
        {
            var entity = _mapper.Map<Table>(dto);
            var created = await _tableService.CreateAsync(entity);
            var response = _mapper.Map<TableResponse>(created);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, UpdateTableRequest dto)
    {
        try
        {
            var entity = _mapper.Map<Table>(dto);
            await _tableService.UpdateAsync(id, entity);
            return NoContent();
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            await _tableService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }
}