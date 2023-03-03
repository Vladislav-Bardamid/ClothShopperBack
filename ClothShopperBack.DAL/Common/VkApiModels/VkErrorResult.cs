using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ClothShopperBack.DAL.Common.VkApiModels
{
    internal class VkErrorResult
    {
        [JsonPropertyName("error_code")]
        public int ErrorCode { get; set; }
        [JsonPropertyName("error_msg")]
        public string ErrorMessage { get; set; }
    }
}
