using System.Text.RegularExpressions;
using AutoMapper;
using ClothShopperBack.BLL.Models;
using ClothShopperBack.DAL;
using ClothShopperBack.DAL.Common;
using ClothShopperBack.DAL.Common.VkApiModels;
using ClothShopperBack.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ClothShopperBack.BLL.Services;

public interface IClothService
{
    Task UpdateCachedClothes();
    Task<ClothListDTO> GetCachedClothesAsync(ClothesFilterModelDTO filter);
}

public class ClothService : IClothService
{
    private AppDbContext _context;
    private IMapper _mapper;
    private IVkAPI _api;

    public ClothService(AppDbContext context, IVkAPI api, IMapper mapper, UserManager<User> userManager)
    {
        _context = context;
        _mapper = mapper;
        _api = api;
    }

    public async Task UpdateCachedClothes()
    {
        var clothes = await GetAllClothesFromAllAlbumsAsync();
        await SaveCachedClothesAsync(clothes);
    }

    public async Task<ClothListDTO> GetCachedClothesAsync(ClothesFilterModelDTO filter)
    {
        var clothes = _context.Clothes.Where(x => !x.IsDeleted);

        if (filter.AlbumId != null)
        {
            clothes = clothes.Where(x => x.AlbumId == filter.AlbumId);
        }

        if (filter.Text != null)
        {
            clothes = clothes.Where(x => x.Text == filter.Text);
        }

        if (filter.MinPrice != 0)
        {
            clothes = clothes.Where(x => x.Price > filter.MinPrice);
        }
        if (filter.MaxPrice != 0)
        {
            clothes = clothes.Where(x => x.Price < filter.MaxPrice);
        }

        if (filter.SortType != SortType.Date)
        {
            clothes = filter.SortType switch
            {
                SortType.DateDesc => clothes.OrderByDescending(x => x.Date),
                SortType.Name => clothes.OrderBy(x => x.Title),
                SortType.NameDesc => clothes.OrderByDescending(x => x.Title),
                SortType.Price => clothes.OrderBy(x => x.Price),
                SortType.PriceDesc => clothes.OrderByDescending(x => x.Price)
            };
        }

        var dtoClothes = _mapper.Map<List<ClothDTO>>(await clothes.ToListAsync());

        var selectedClothes = clothes
                .Include(x => x.Orders)
                .Where(x => x.Orders
                    .Any(y => y.UserId == filter.UserId));

        var price = 0;

        foreach (var cloth in selectedClothes)
        {
            dtoClothes.First(x => x.Id == cloth.Id).IsActive = true;
            price += cloth.Price;
        }

        var result = new ClothListDTO
        {
            Items = dtoClothes,
            Price = price
        };

        return result;
    }

    private async Task<List<Cloth>> GetAllClothesFromAllAlbumsAsync()
    {
        var albums = await _context.Albums.ToListAsync();
        var clothes = albums.Select(async x => await _api.GetPhotosAsync(x.VkOwnerId, x.VkAlbumId))
                            .SelectMany(x => x.Result!)
                            .ToList();

        return _mapper.Map<List<Cloth>>(clothes);
    }

    private async Task SaveCachedClothesAsync(IEnumerable<Cloth> clothes)
    {
        var dbClothes = _context.Clothes.Where(x => !x.IsDeleted);

        var addedClothes = clothes.Where(x => !dbClothes.Select(y => y.VkId).Contains(x.VkId));
        var currentClothes = dbClothes.Where(x => !addedClothes.Select(y => y.VkId).Contains(x.VkId));
        var removedClothes = dbClothes.Where(x => !clothes.Select(y => y.VkId).Contains(x.VkId));

        foreach (var cloth in addedClothes)
        {
            ProceedClothText(cloth);

            if (cloth.Title.IsNullOrEmpty()) continue;

            cloth.AlbumId = _context.Albums.First(x => x.VkAlbumId == cloth.VkAlbumId).Id;
            await _context.Clothes.AddAsync(cloth);
        }

        foreach (var cloth in currentClothes)
        {
            var newCloth = clothes.First(x => x.VkId == cloth.VkId);
            _mapper.Map(newCloth, cloth);
        }

        foreach (var cloth in removedClothes)
        {
            cloth.IsDeleted = true;
        }

        await _context.SaveChangesAsync();
    }

    private void ProceedClothText(Cloth photo)
    {
        var wh = @"([\d-*]+)";
        var match = Regex.Match(photo.Text, @$"^\d+\s[A-zА-я]\s(.+)\s(?:длинна|дл|д)\s?{wh}\s(?:ширина|шир|ш)\s?{wh}\s(.+)?\s(?:цена)\s?(\d+)", RegexOptions.IgnoreCase);

        photo.Title = match.Groups[1].Value;
        photo.Width = match.Groups[2].Value;
        photo.Height = match.Groups[3].Value;
        photo.Scrap = match.Groups[4].Value;

        int.TryParse(match.Groups[5].Value, out var price);
        photo.Price = price;
    }
}