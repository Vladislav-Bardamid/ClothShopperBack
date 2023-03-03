using System.Text.Json.Serialization;

namespace ClothShopperBack.DAL.Common.VkApiModels;

public class VkPhoto
{
    public int Id { get; set; }
    [JsonPropertyName("album_id")]
    public int AlbumId { get; set; }
    public string Text { get; set; }
    public int Date { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public List<VkPhotoSize> Sizes { get; set; }
}