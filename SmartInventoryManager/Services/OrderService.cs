using Microsoft.EntityFrameworkCore;
using SmartInventoryManager.Api.Data;
using SmartInventoryManager.Api.DTOs;
using SmartInventoryManager.Api.Models;
using System;

namespace SmartInventoryManager.Api.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;

        public OrderService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<OrderResponseDto> CreateOrderAsync(CreateOrderDto dto)
        {
            // 1. Validate Customer exists
            var customer = await _context.Customers.FindAsync(dto.CustomerId);
            if (customer == null)
                throw new KeyNotFoundException($"Customer with id {dto.CustomerId} not found");

            // 2. Validate Products & Stock
            var orderItems = new List<OrderItem>();
            decimal totalAmount = 0;

            foreach (var itemDto in dto.OrderItems)
            {
                var product = await _context.Products.FindAsync(itemDto.ProductId);
                if (product == null)
                    throw new KeyNotFoundException($"Product with id {itemDto.ProductId} not found");

                if (product.Quantity < itemDto.Quantity)
                    throw new InvalidOperationException($"Insufficient stock for {product.Name}. Available: {product.Quantity}");

                // Create OrderItem
                var orderItem = new OrderItem
                {
                    ProductId = itemDto.ProductId,
                    Quantity = itemDto.Quantity,
                    UnitPrice = product.Price
                };
                orderItems.Add(orderItem);

                totalAmount += product.Price * itemDto.Quantity;
            }

            // 3. Create Order
            var order = new Order
            {
                CustomerId = dto.CustomerId,
                TotalAmount = totalAmount,
                OrderItems = orderItems
            };

            // 4. Save Order + Update Stock
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // 5. Reduce Product Stock
            foreach (var item in order.OrderItems)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                product.Quantity -= item.Quantity;
            }
            await _context.SaveChangesAsync();

            // 6. Return Order Response
            return new OrderResponseDto
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                CustomerName = customer.Name,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                OrderItems = order.OrderItems.Select(oi => new OrderItemDto
                {
                    ProductId = oi.ProductId,
                    Quantity = oi.Quantity
                }).ToList()
            };
        }

        public async Task<List<OrderResponseDto>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Select(o => new OrderResponseDto
                {
                    Id = o.Id,
                    CustomerId = o.CustomerId,
                    CustomerName = o.Customer.Name,
                    OrderDate = o.OrderDate,
                    TotalAmount = o.TotalAmount,
                    OrderItems = o.OrderItems.Select(oi => new OrderItemDto
                    {
                        ProductId = oi.ProductId,
                        Quantity = oi.Quantity
                    }).ToList()
                }).ToListAsync();
        }
    }
}
