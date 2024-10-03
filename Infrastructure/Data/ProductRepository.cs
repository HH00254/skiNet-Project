using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ProductRepository(StoreContext context) : IProductRepository
{
    public StoreContext Context { get; } = context;

    public void AddProduct(Product product)
    {
        this.Context.Products.Add(product);
    }

    public void DeleteProduct(Product product)
    {
        this.Context.Products.Remove(product);
    }

    public async Task<IReadOnlyList<string>> GetBrandsAsync()
    {
        return await this.Context.Products.Select(x => x.Brand)
            .Distinct()
            .ToListAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await this.Context.Products.FindAsync(id);
    }
    public async Task<IReadOnlyList<Product>> GetProductsAsync(string? brand, string? type, string? sort)
    {
        var query = this.Context.Products.AsQueryable();

        if (!string.IsNullOrWhiteSpace(brand))
            query = query.Where(x => x.Brand.Equals(brand));

        if (!string.IsNullOrWhiteSpace(type))
            query = query.Where(x => x.Type.Equals(type));

        query = sort switch
        {
            "priceAsc"  => query.OrderBy(x => x.Price),
            "priceDesc" => query.OrderByDescending(x => x.Price),
            _           => query.OrderBy(x => x.Name)
        };

        return await query.ToListAsync();
    }

    public async Task<IReadOnlyList<string>> GetTypesAsync()
    {
        return await this.Context.Products.Select(x => x.Type)
            .Distinct()
            .ToListAsync();
    }

    public bool ProductExists(int id)
    {
        return this.Context.Products.Any(x => x.Id.Equals(id));
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await this.Context.SaveChangesAsync() > 0;
    }

    public void UpdateProduct(Product product)
    {
        this.Context.Entry(product).State = EntityState.Modified;
    }
}
