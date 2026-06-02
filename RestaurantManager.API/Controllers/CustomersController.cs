using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RestaurantManager.API.DTOs.Request;
using RestaurantManager.API.DTOs.Response;
using RestaurantManager.Domain.Entities;
using RestaurantManager.Domain.Interfaces.Services;

namespace RestaurantManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;
    private readonly IMapper _mapper;

    public CustomersController(ICustomerService customerService, IMapper mapper)
    {
        _customerService = customerService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerResponse>>> GetAll()
    {
        var customers = await _customerService.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<CustomerResponse>>(customers));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerResponse>> GetById(int id)
    {
        var customer = await _customerService.GetByIdAsync(id);
        if (customer == null)
            return NotFound(new { message = $"Cliente con ID {id} no encontrado" });

        return Ok(_mapper.Map<CustomerResponse>(customer));
    }

    [HttpPost]
    public async Task<ActionResult<CustomerResponse>> Create(CreateCustomerRequest dto)
    {
        try
        {
            var entity = _mapper.Map<Customer>(dto);
            var created = await _customerService.CreateAsync(entity);
            var response = _mapper.Map<CustomerResponse>(created);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }
        catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, UpdateCustomerRequest dto)
    {
        try
        {
            var entity = _mapper.Map<Customer>(dto);
            await _customerService.UpdateAsync(id, entity);
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
            await _customerService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }
}