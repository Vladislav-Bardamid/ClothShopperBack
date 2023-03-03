using AutoMapper;
using ClothShopperBack.BLL.Models;
using ClothShopperBack.DAL.Common.VkApiModels;
using ClothShopperBack.DAL.Entities;

namespace ClothShopperBack.BLL;

internal class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<User, UserDTO>().ReverseMap();
        CreateMap<VkPhoto, ClothDTO>()
            .ForMember(d => d.Title, m => m.MapFrom(e => e.Text))
            .ForMember(d => d.UrlSm, m => m.MapFrom(e => e.Sizes.First(x => x.Type == 'x').Url))
            .ForMember(d => d.UrlMd, m => m.MapFrom(e => e.Sizes.First(x => x.Type == 'y').Url))
            .ForMember(d => d.UrlLg, m => m.MapFrom(e => e.Sizes.First(x => x.Type == 'z').Url))
            .ForMember(d => d.Date, m => m.MapFrom(e => ConvertFromUnixTimestamp(e.Date)))
            .ForMember(d => d.Width, m => m.Ignore())
            .ForMember(d => d.Height, m => m.Ignore())
            .ReverseMap();
        CreateMap<Album, AlbumDTO>().ReverseMap();
    }

    private DateTime ConvertFromUnixTimestamp(int timestamp)
    {
        return DateTimeOffset.FromUnixTimeSeconds(timestamp).UtcDateTime;
    }
}