using ClothShopperBack.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothShopperBack.DAL.Common.VkApiModels
{
    internal class GetVkPhotosResult
    {
        public int Count { get; set; }
        public List<VkPhoto>? Items { get; set; }
    }
}
