using Microsoft.EntityFrameworkCore;
using SmartInventoryManager.Api.Models;

namespace SmartInventoryManager.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Product Configuration
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Name)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(p => p.Price)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();
                entity.Property(p => p.Quantity)
                    .IsRequired();
            });

            // Customer Configuration
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Name)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(c => c.Email)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.HasIndex(c => c.Email)
                    .IsUnique();
                entity.Property(c => c.Phone)
                    .HasMaxLength(20);
            });

            // Order Configuration
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(o => o.Id);
                entity.Property(o => o.OrderDate)
                    .IsRequired()
                    .HasDefaultValueSql("GETUTCDATE()");
                entity.Property(o => o.TotalAmount)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();
                
                // Relationship: Order -> Customer (Many-to-One)
                entity.HasOne(o => o.Customer)
                    .WithMany()
                    .HasForeignKey(o => o.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Relationship: Order -> OrderItems (One-to-Many)
                entity.HasMany(o => o.OrderItems)
                    .WithOne(oi => oi.Order)
                    .HasForeignKey(oi => oi.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // OrderItem Configuration
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(oi => oi.Id);
                entity.Property(oi => oi.Quantity)
                    .IsRequired();
                entity.Property(oi => oi.UnitPrice)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                // Relationship: OrderItem -> Product (Many-to-One)
                entity.HasOne(oi => oi.Product)
                    .WithMany()
                    .HasForeignKey(oi => oi.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
