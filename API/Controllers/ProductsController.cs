using System;
using System.Runtime.CompilerServices;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IProductRepository repo) : ControllerBase
{
    public IProductRepository Repo { get; } = repo;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts(string? brand, string? type, string? sort)
    {
        return Ok(await this.Repo.GetProductsAsync(brand, type, sort));
    }

    [HttpGet("{id:int}")] // api/products/2
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await this.Repo.GetProductByIdAsync(id);
        return  product is not null ? product : NotFound();
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        this.Repo.AddProduct(product);

        if (await this.Repo.SaveChangesAsync())
        {
            return CreatedAtAction("GetProduct", new{id = product.Id}, product);
        }


        return BadRequest("Problem creating product");
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct(int id, Product product)
    {
        if (product.Id != id || !ProductExists(id))
            return BadRequest("Cannot update this product");

        this.Repo.UpdateProduct(product);

        if (await this.Repo.SaveChangesAsync())
        {
            return NoContent();
        }

        return BadRequest("Problem updating the product");
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await this.Repo.GetProductByIdAsync(id);

        if (product is null)
            return NotFound();

        this.Repo.DeleteProduct(product);

        if (await this.Repo.SaveChangesAsync())
        {
            return NotFound();
        }

        return BadRequest("Problem deleting the product");
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
        return Ok(await this.Repo.GetBrandsAsync());
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
        return Ok(await this.Repo.GetTypesAsync());
    }

    private bool ProductExists(int id)
    {
        return this.Repo.ProductExists(id);
    }
}
