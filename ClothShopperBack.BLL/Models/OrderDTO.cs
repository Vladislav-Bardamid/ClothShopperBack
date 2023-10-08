namespace ClothShopperBack.BLL.Models;

public class OrderDTO
{
    public int ClothId { get; set; }
    public int UserId { get; set; }
    public int? OrderListId { get; set; }
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
    public bool? IsSuccess { get; set; }
}