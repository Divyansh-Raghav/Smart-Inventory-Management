using Microsoft.EntityFrameworkCore;
using SmartInventoryManager.Api.Data;
using SmartInventoryManager.Api.DTOs;
using SmartInventoryManager.Api.Models;

namespace SmartInventoryManager.Api.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly AppDbContext _context;

        public CustomerService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CustomerResponseDto>> GetAllAsync()
        {
            return await _context.Customers
                .Select(c => new CustomerResponseDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Email = c.Email,
                    Phone = c.Phone
                }).ToListAsync();
        }

        public async Task<CustomerResponseDto?> GetByIdAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return null;

            return new CustomerResponseDto
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email,
                Phone = customer.Phone
            };
        }

        public async Task<CustomerResponseDto> CreateAsync(CreateCustomerDto dto)
        {
            var customer = new Customer
            {
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return new CustomerResponseDto
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email,
                Phone = customer.Phone
            };
        }

        public async Task UpdateAsync(int id, CreateCustomerDto dto)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
                throw new KeyNotFoundException($"Customer with id {id} not found");

            customer.Name = dto.Name;
            customer.Email = dto.Email;
            customer.Phone = dto.Phone;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
                throw new KeyNotFoundException($"Customer with id {id} not found");

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
        }
    }
}
