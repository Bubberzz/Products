using Products.Domain.Entities;
using Products.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Products.Infrastructure.Context;

public static class AppDbContextSeed
{
    public static async Task SeedAsync(AppDbContext context, ILogger logger)
    {
        if (!context.Products.Any())
        {
            context.Products.AddRange(
                new Product(ProductId.New(), "Pioneer DJ CDJ-3000 Professional Multi Player", new Price(2169), new Stock(50)),
                new Product(ProductId.New(), "Pioneer DJ DJM-A9 Professional DJ Mixer", new Price(2469), new Stock(60)),
                new Product(ProductId.New(), "Allen & Heath XONE:96 Club Mixer", new Price(1789), new Stock(15)),
                new Product(ProductId.New(), "Denon DJ PRIME 4+ Standalone DJ System", new Price(1899), new Stock(35)),
                new Product(ProductId.New(), "Sennheiser HD25 Headphones", new Price(117), new Stock(200))
            );

            await context.SaveChangesAsync();
            logger.LogInformation("Database seeded successfully");
        }
    }
}