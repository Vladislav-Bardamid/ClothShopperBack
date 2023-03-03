using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothShopperBack.DAL.Common.VkApiModels
{
    internal class VkApiResult<T>
    {
        public T? Response { get; set; }
        public VkErrorResult Error { get; set; }
    }
}
