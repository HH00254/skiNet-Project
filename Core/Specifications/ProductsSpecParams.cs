using System;

namespace Core.Specifications;

public class ProductsSpecParams
{
    private const int MaxPageSize = 50;
    private int _pageSize = 6;
    private string? _search;
    private List<string> _brands = [];
    private List<string> _types = [];

    public List<string> Brands
    {
        get => this._brands; // types=boards, gloves
        set
        {
            _brands = value.SelectMany(
                x => x.Split(',', StringSplitOptions.RemoveEmptyEntries)
            ).ToList();
        }
    }

    public List<string> Types
    {
        get => this._types;
        set
        {
            _types = value.SelectMany(
                x => x.Split(',', StringSplitOptions.RemoveEmptyEntries)
            ).ToList();
        }
    }

    public string? Sort { get; set; }
    public int PageIndex { get; set; } = 1;
    public int PageSize
    {
        get => this._pageSize;
        set => this._pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }
    public string Search
    {
        get => this._search ?? "";
        set => this._search = value.ToLower();
     }



}
