using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClothShopperBack.BLL.Models;

public class ClothesFilterModelDTO
{
    public int UserId { get; set; }
    public int? AlbumId { get; set; }
    public string? Text { get; set; }
    public int MinPrice { get; set; } = 0;
    public int MaxPrice { get; set; } = 0;
    public int Start { get; set; } = 0;
    public int Length { get; set; } = 50;
    public SortType SortType { get; set; } = SortType.Date;
}

public enum SortType{
    Date,
    DateDesc,
    Name,
    NameDesc,
    Price,
    PriceDesc
}
