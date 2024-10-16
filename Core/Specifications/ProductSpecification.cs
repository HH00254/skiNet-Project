using System;
using System.Linq;
using Core.Entities;
using Core.Util;

namespace Core.Specifications;

public class ProductSpecification : BaseSpecification<Product>
{
    public ProductSpecification(ProductsSpecParams specParams) : base(x =>
        (string.IsNullOrEmpty(specParams.Search) || x.Name.ToLower().Contains(specParams.Search)) &&
        (specParams.Brands.Count == 0 || specParams.Brands.Contains(x.Brand)) &&
        (specParams.Types.Count  == 0 || specParams.Types.Contains(x.Type))
    )
    {
        DefaultDictionary<string, Action> productActions = new(() => () => AddOrderBy(x => x.Name));
        productActions["priceAsc"]  = () =>{this.AddOrderBy(x => x.Price);};
        productActions["priceDesc"] = () =>{this.AddOrderByDescending(x => x.Price);};

        this.ApplyPaging(specParams.PageSize * (specParams.PageIndex -1), specParams.PageSize);

        productActions[specParams.Sort is null? "": specParams.Sort]();
    }
}
