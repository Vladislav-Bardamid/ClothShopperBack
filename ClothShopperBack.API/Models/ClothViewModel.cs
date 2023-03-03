namespace ClothShopperBack.API.Models;

public class ClothViewModel
{
    public int Id { get; set; }
    public string? Text { get; set; }
    public string? Title { get; set; }
    public DateTime Date { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public int Price { get; set; }
    public string? UrlSm { get; set; }
    public string? UrlMd { get; set; }
    public string? UrlLg { get; set; }
}