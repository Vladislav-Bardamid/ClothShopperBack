using System.Text.Json.Serialization;

namespace ClothShopperBack.DAL.Common.VkApiModels;

public class VkProfileInfo
{
    [JsonPropertyName("first_name")]
    public string FirstName { get; set; }
    [JsonPropertyName("last_name")]
    public string LastName { get; set; }
}
