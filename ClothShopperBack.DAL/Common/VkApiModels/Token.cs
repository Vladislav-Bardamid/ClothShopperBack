using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClothShopperBack.DAL.Common.VkApiModels
{
    public class VkToken
    {
        public string AccessToken { get; set; }
        public int ExpiresIn { get; set; }
        public int UserId { get; set; }
    }
}