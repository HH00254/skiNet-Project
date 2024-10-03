using System;
using System.Text.Json;
using Core.Entities;

namespace Infrastructure.Data;

public class StoreContextSeed
{
    public static async Task SeedAysnc(StoreContext context)
    {
        if(!context.Products.Any())
        {
            string productsData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/products.json");

            List<Product>? products = JsonSerializer.Deserialize<List<Product>>(productsData);

            if (products!.Equals(null))
                return;

            context.Products.AddRange(products);
            await context.SaveChangesAsync();
        }
    }
}
