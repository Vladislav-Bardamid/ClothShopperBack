using AutoMapper;
using ClothShopperBack.API.Models;
using ClothShopperBack.BLL.Models;
using ClothShopperBack.DAL.Common.VkApiModels;
using ClothShopperBack.DAL.Entities;

namespace ClothShopperBack.BLL;

internal class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        // Db maps
        CreateMap<User, UserDTO>()
            .ReverseMap();
        CreateMap<Cloth, ClothDTO>().ReverseMap();
        CreateMap<Cloth, OrderDTO>()
            .ForMember(d => d.Id, m => m.Ignore())
            .ForMember(d => d.ClothId, m => m.MapFrom(e => e.Id));
        CreateMap<Album, AlbumDTO>().ReverseMap();
        CreateMap<Order, OrderDTO>().IncludeMembers(x => x.Cloth).ReverseMap();
        CreateMap<OrderList, OrderListDTO>().ReverseMap();

        // Vk maps
        CreateMap<VkAlbum, AlbumDTO>()
            .ForMember(d => d.Id, m => m.Ignore());
        CreateMap<VkPhoto, Cloth>()
            .ForMember(d => d.VkAlbumId, m => m.MapFrom(e => e.AlbumId))
            .ForMember(d => d.VkOwnerId, m => m.MapFrom(e => e.OwnerId))
            .ForMember(d => d.UrlSm, m => m.MapFrom(e => FirstUrlOrDefault(e.Sizes, 'x')))
            .ForMember(d => d.UrlMd, m => m.MapFrom(e => FirstUrlOrDefault(e.Sizes, 'y')))
            .ForMember(d => d.UrlLg, m => m.MapFrom(e => FirstUrlOrDefault(e.Sizes, 'z')))
            .ForMember(d => d.Date, m => m.MapFrom(e => ConvertFromUnixTimestamp(e.Date)))
            .ForMember(d => d.Id, m => m.Ignore())
            .ForMember(d => d.AlbumId, m => m.Ignore());
    }

    private string FirstUrlOrDefault(IEnumerable<VkPhotoSize> sizes, char type)
    {
        return sizes.FirstOrDefault(x => x.Type == type)?.Url
            ?? sizes.First(x => x.Type == 'x').Url;
    }

    private DateTime ConvertFromUnixTimestamp(int timestamp)
    {
        return DateTimeOffset.FromUnixTimeSeconds(timestamp).UtcDateTime;
    }
}
