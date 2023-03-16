using System.Text.RegularExpressions;
using AutoMapper;
using ClothShopperBack.BLL.Models;
using ClothShopperBack.DAL.Common;
using ClothShopperBack.DAL.Common.VkApiModels;

namespace ClothShopperBack.BLL.Services;

public interface IPhotoService
{
    Task<List<ClothDTO>?> GetPhotos(ClothesFilterModel? filter = null);
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

    public async Task<List<ClothDTO>?> GetPhotos(ClothesFilterModel? filter = null)
    {
        var vkPhotos = await _api.GetPhotos(174012088, 263431528);
        var photos = vkPhotos.Where(x => !string.IsNullOrEmpty(x.Text))
                             .Select(ProceedCloth);

        if (filter == null)
            return photos.ToList();

        if (filter.Text != null)
        {
            photos = photos.Where(x => x.Text.ToLower().Contains(filter.Text));
        }
        if (filter.MinPrice != 0)
        {
            photos = photos.Where(x => x.Price > filter?.MinPrice);
        }
        if (filter.MaxPrice != 0)
        {
            photos = photos.Where(x => x.Price < filter?.MaxPrice);
        }
        if (filter.SortType != SortType.Date)
        {
            photos = filter.SortType switch
            {
                SortType.Name => photos.OrderBy(x => x.Title),
                SortType.LowPrice => photos.OrderBy(x => x.Price),
                SortType.HightPrice => photos.OrderByDescending(x => x.Price)
            };
        }

        return photos.ToList();
    }

    private ClothDTO ProceedCloth(VkPhoto photo)
    {
        var photoDTO = _mapper.Map<ClothDTO>(photo);

        var wh = @"([\d-*]+)";
        var match = Regex.Match(photo.Text, @$"^\d+\s[A-zА-я]\s(.+)\s(?:длинна|дл|д)\s?{wh}\s(?:ширина|шир|ш)\s?{wh}\s(.+)?\s(?:цена)\s?(\d+)", RegexOptions.IgnoreCase);

        photoDTO.Title = match.Groups[1].Value;
        photoDTO.Width = match.Groups[2].Value;
        photoDTO.Height = match.Groups[3].Value;
        photoDTO.Scrap = match.Groups[4].Value;

        int.TryParse(match.Groups[5].Value, out var price);
        photoDTO.Price = price;

        return photoDTO;
    }
}