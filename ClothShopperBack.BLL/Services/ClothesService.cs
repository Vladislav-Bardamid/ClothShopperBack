using System.Linq;
using System.Text.RegularExpressions;
using AutoMapper;
using ClothShopperBack.BLL.Models;
using ClothShopperBack.DAL.Common;
using ClothShopperBack.DAL.Common.VkApiModels;

namespace ClothShopperBack.BLL.Services;

public interface IPhotoService
{
    Task<List<ClothDTO>?> GetPhotos(int start = 0, int length = 50);
}

public class ClothesService : IPhotoService
{
    private IMapper _mapper;
    private IVkAPI _api;

    public ClothesService(IVkAPI api, IMapper mapper)
    {
        _api = api;
        _mapper = mapper;
    }

    public async Task<List<ClothDTO>?> GetPhotos(int start = 0, int length = 50)
    {
        var vkPhotos = await _api.GetPhotos(-171875990, 280934757);
        var photos = vkPhotos.Where(x => !string.IsNullOrEmpty(x.Text))
                             .Select(ProceedCloth)
                             .ToList();
        return photos;
    }

    private ClothDTO ProceedCloth(VkPhoto photo)
    {
        var photoDTO = _mapper.Map<ClothDTO>(photo);

        photoDTO.Title = Regex.Match(photo.Text, @"^([а-яА-Я\w\s.,\(\)-]+?);?\n").Groups[1].Value;

        int.TryParse(Regex.Match(photo.Text, @"(?:д|дл|длинна)\s?(\d+)").Groups[1].Value, out var width);
        photoDTO.Width = width;

        int.TryParse(Regex.Match(photo.Text, @"(?:ш|шир|ширина)\s?(\d+)").Groups[1].Value, out var height);
        photoDTO.Height = height;

        int.TryParse(Regex.Match(photo.Text, @"(\d+)\sруб").Groups[1].Value, out var price);
        photoDTO.Price = price;
        
        return photoDTO;
    }
}