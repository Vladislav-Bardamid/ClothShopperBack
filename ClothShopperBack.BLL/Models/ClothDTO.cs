namespace ClothShopperBack.BLL.Models;

public class ClothDTO
{
    public int Id { get; set; }
    public int AlbumId { get; set; }
    public string? Text { get; set; }
    public string? Title { get; set; }
    public string? Scrap { get; set; }
    public DateTime Date { get; set; }
    public string? Width { get; set; }
    public string? Height { get; set; }
    public int Price { get; set; }
    public string? UrlSm { get; set; }
    public string? UrlMd { get; set; }
    public string? UrlLg { get; set; }
}