using System;
using System.Linq;
using Core.Entities;
using Core.Util;

namespace Core.Specifications;

public class ProductSpecification : BaseSpecification<Product>
{
    public ProductSpecification(string? brand, string? type, string? sort) : base(x =>
        (string.IsNullOrWhiteSpace(brand) || x.Brand == brand) &&
        (string.IsNullOrWhiteSpace(type)  || x.Type == type)
    )
    {
        DefaultDictionary<string, Action> productActions = new(() => () => AddOrderBy(x => x.Name));
        productActions["priceAsc"]  = () =>{this.AddOrderBy(x => x.Price);};
        productActions["priceDesc"] = () =>{this.AddOrderByDescending(x => x.Price);};

        productActions[sort is null? "": sort]();
    }
}
