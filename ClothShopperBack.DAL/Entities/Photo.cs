namespace ClothShopperBack.DAL.Entities;

public class Photo
{
    public int Id { get; set; }
    public string? Url { get; set; }
    public string? Src { get; set; }
    public int VkAlbumId { get; set; }
    public Album VkAlbum { get; set; }
}