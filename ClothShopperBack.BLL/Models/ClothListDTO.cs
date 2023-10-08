using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClothShopperBack.BLL.Models;

public class ClothListDTO
{
    public List<ClothDTO> Items { get; set; }
    public int Price { get; set; }
}