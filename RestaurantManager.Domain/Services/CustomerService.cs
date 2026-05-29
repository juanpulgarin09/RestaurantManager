using Microsoft.Extensions.Logging;
using RestaurantManager.Domain.Entities;
using RestaurantManager.Domain.Interfaces.Repositories;
using RestaurantManager.Domain.Interfaces.Services;

namespace RestaurantManager.Domain.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ILogger<CustomerService> _logger;

    public CustomerService(
        ICustomerRepository customerRepository,
        ILogger<CustomerService> logger)
    {
        _customerRepository = customerRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        _logger.LogInformation("Retrieving all customers");
        return await _customerRepository.GetAllAsync();
    }

    public async Task<Customer?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Retrieving customer with ID: {Id}", id);
        return await _customerRepository.GetByIdAsync(id);
    }

    public async Task<Customer> CreateAsync(Customer customer)
    {
        if (string.IsNullOrWhiteSpace(customer.FullName))
            throw new InvalidOperationException("El nombre del cliente es obligatorio.");

        var existing = await _customerRepository.GetByEmailAsync(customer.Email);
        if (existing != null)
            throw new InvalidOperationException(
                $"Ya existe un cliente con el email '{customer.Email}'");

        _logger.LogInformation("Creating customer: {FullName}", customer.FullName);
        return await _customerRepository.CreateAsync(customer);
    }

    public async Task UpdateAsync(int id, Customer customer)
    {
        var existing = await _customerRepository.GetByIdAsync(id);
        if (existing == null)
            throw new KeyNotFoundException($"No se encontró el cliente con ID {id}");

        if (existing.Email != customer.Email)
        {
            var emailConflict = await _customerRepository.GetByEmailAsync(customer.Email);
            if (emailConflict != null)
                throw new InvalidOperationException(
                    $"Ya existe un cliente con el email '{customer.Email}'");
        }

        existing.FullName = customer.FullName;
        existing.Email = customer.Email;
        existing.Phone = customer.Phone;

        _logger.LogInformation("Updating customer with ID: {Id}", id);
        await _customerRepository.UpdateAsync(existing);
    }

    public async Task DeleteAsync(int id)
    {
        var exists = await _customerRepository.ExistsAsync(id);
        if (!exists)
            throw new KeyNotFoundException($"No se encontró el cliente con ID {id}");

        _logger.LogInformation("Deleting customer with ID: {Id}", id);
        await _customerRepository.DeleteAsync(id);
    }
}