using ClothShopperBack.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothShopperBack.DAL.Common.VkApiModels
{
    internal class GetVkListResult<T>
    {
        public int Count { get; set; }
        public List<T>? Items { get; set; }
    }
}
