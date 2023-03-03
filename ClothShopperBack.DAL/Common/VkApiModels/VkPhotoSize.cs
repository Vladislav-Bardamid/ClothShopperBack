using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothShopperBack.DAL.Common.VkApiModels
{
    public class VkPhotoSize
    {
        public string Url { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public char Type { get; set; }
    }
}
