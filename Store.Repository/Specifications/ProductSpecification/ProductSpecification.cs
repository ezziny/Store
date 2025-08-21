using System;

namespace Store.Repository.Specifications.ProductSpecification;

public class ProductSpecification
{
    public int? TypeId { get; set; }
    public int? BrandId { get; set; }
    public string? Sort { get; set; }
    public int PageIndex { get; set; } = 1;
    private int _pageSize = 10;
    private const int MAXPAGESIZE = 50;
    public int PageSize
    {
        get { return _pageSize; }
        set { _pageSize = (value>MAXPAGESIZE)? MAXPAGESIZE: value; }
    }
    
}
