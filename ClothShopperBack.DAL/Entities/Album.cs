namespace ClothShopperBack.DAL.Entities;

public class Album
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int VkOwnerId { get; set; }
    public int VkAlbumId { get; set; }
}