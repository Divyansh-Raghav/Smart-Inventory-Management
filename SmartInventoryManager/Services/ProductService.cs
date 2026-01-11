using Microsoft.EntityFrameworkCore;
using SmartInventoryManager.Api.Data;
using SmartInventoryManager.Api.DTOs;
using SmartInventoryManager.Api.Models;

namespace SmartInventoryManager.Api.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductResponseDto>> GetAllAsync()
        {
            return await _context.Products
                .Select(p => new ProductResponseDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Quantity = p.Quantity
                }).ToListAsync();
        }

        public async Task<Product> GetProductWithStockAsync(int productId)
        {
            return await _context.Products.FindAsync(productId)
                ?? throw new KeyNotFoundException($"Product {productId} not found");
        }

        public async Task<ProductResponseDto?> GetByIdAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return null;

            return new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Quantity = product.Quantity
            };
        }

        public async Task<ProductResponseDto> CreateAsync(CreateProductDto dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Price = dto.Price,
                Quantity = dto.Quantity
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Quantity = product.Quantity
            };
        }

        public async Task UpdateAsync(int id, CreateProductDto dto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                throw new KeyNotFoundException($"Product with id {id} not found");

            product.Name = dto.Name;
            product.Price = dto.Price;
            product.Quantity = dto.Quantity;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                throw new KeyNotFoundException($"Product with id {id} not found");

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}
