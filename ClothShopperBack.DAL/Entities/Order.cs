using ClothShopperBack.DAL.Common.VkApiModels;

namespace ClothShopperBack.DAL.Entities;

public class Order
{
    public int Id { get; set; }
    public int ClothId { get; set; }
    public Cloth? Cloth { get; set; }
    public int OrderListId { get; set; }
    public OrderList OrderList { get; set; }
    public bool? IsSuccess { get; set; }
}
