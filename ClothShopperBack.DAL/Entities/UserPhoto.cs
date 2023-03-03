using ClothShopperBack.DAL.Common.VkApiModels;

namespace ClothShopperBack.DAL.Entities;

public class UserPhoto
{
    public int Id { get; set; }
    public int VkUserId { get; set; }
    public User? VkUser { get; set; }
    public int VkPhotoId { get; set; }
    public VkPhoto? VkPhoto { get; set; }
}