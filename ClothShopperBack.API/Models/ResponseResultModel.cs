using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClothShopperBack.API.Models
{
    public class ResponseResultModel<T>
    {
        public T Data { get; set; }
    }
}