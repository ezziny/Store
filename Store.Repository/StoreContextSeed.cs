using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Store.Data.Contexts;
using Store.Data.Entities;

namespace Store.Repository;

public class StoreContextSeed
{
    public static async Task SeedAsync(StoreDbContext context, ILoggerFactory loggerFactory)
    {
        try
        {
            async Task SeedEntityAsync<TEntity>(DbSet<TEntity>? dbSet, string filePath) where TEntity : class
            {
                if (dbSet != null && !await dbSet.AnyAsync())
                {
                    using var data = File.OpenRead(filePath);
                    var entities = await JsonSerializer.DeserializeAsync<List<TEntity>>(data);
                    if (entities is not null)
                        await dbSet.AddRangeAsync(entities);
                }
            }

            await SeedEntityAsync(context.ProductBrands, "../Store.Repository/SeedData/brands.json");
            await SeedEntityAsync(context.ProductTypes, "../Store.Repository/SeedData/types.json");
            await SeedEntityAsync(context.Products, "../Store.Repository/SeedData/products.json");
            await SeedEntityAsync(context.DeliveryMethods, "../Store.Repository/SeedData/delivery.json");

            await context.SaveChangesAsync();
        }
        catch(Exception e)
        {
            var logger = loggerFactory.CreateLogger<StoreContextSeed>();
            logger.LogError(e.Message);
        }
    }
}
