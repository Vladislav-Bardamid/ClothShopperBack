using AutoMapper;
using ClothShopperBack.API.Models;
using ClothShopperBack.BLL.Models;

namespace ClothShopperBack.API;

internal class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<UserDTO, UserViewModel>().ReverseMap();
        CreateMap<ClothDTO, ClothViewModel>().ReverseMap();
        CreateMap<AlbumDTO, AlbumViewModel>().ReverseMap();
    }
}