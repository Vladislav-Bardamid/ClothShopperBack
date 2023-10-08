using Microsoft.AspNetCore.Identity;

namespace ClothShopperBack.DAL.Entities;

public class User : IdentityUser<int>
{
    public int VkUserId { get; set; }
    public string VkAccessToken { get; set; }
    public ICollection<Order> Orders { get; set; }
}