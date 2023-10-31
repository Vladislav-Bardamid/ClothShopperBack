using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClothShopperBack.BLL.Models
{
    public class OrderListCommand
    {
        public List<int> Add { get; set; }
        public List<int> Delete { get; set; }
    }
}
