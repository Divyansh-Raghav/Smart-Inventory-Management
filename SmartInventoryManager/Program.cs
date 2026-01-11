using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using SmartInventoryManager.Api.Data;
using SmartInventoryManager.Api.Middleware;
using SmartInventoryManager.Api.Services;
using SmartInventoryManager.Api.Validators;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

// Database (PostgreSQL ready for Render)
var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection") ??
    Environment.GetEnvironmentVariable("DATABASE_URL");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));



// FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateProductDtoValidator>();

// Services
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IOrderService, OrderService>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDev", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Global Exception Middleware
app.UseMiddleware<GlobalExceptionMiddleware>();

// Middleware pipeline
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowAngularDev");
app.UseAuthorization();

// OpenAPI + Scalar
app.MapOpenApi();

app.MapControllers();

app.MapScalarApiReference(options =>
{
    options.Title = "Smart Inventory Management API";
    options.Theme = ScalarTheme.Mars;
});

app.Run();
