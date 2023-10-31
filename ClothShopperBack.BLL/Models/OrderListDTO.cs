using ClothShopperBack.DAL.Entities;

namespace ClothShopperBack.BLL.Models;

public class OrderListDTO
{
    public int Id { get; set; }
    public DateTime? CommitDate { get; set; }
    public List<OrderDTO> Orders { get; set; }
    public int PriceSum { get; set; }
}
