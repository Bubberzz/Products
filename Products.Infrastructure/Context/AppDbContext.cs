using Microsoft.EntityFrameworkCore;
using Products.Domain.Entities;

namespace Products.Infrastructure.Context;

public class AppDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}
