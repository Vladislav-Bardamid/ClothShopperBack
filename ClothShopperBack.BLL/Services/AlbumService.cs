using System.Text.RegularExpressions;
using AutoMapper;
using ClothShopperBack.BLL.Models;
using ClothShopperBack.DAL;
using ClothShopperBack.DAL.Common;
using ClothShopperBack.DAL.Common.VkApiModels;
using Microsoft.EntityFrameworkCore;

namespace ClothShopperBack.BLL.Services;

public interface IAlbumService
{
    Task<List<AlbumDTO>> GetAlbumsAsync();
}

public class AlbumService : IAlbumService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public AlbumService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<AlbumDTO>> GetAlbumsAsync()
    {
        var albums = await _context.Albums.ToListAsync();
        
        return _mapper.Map<List<AlbumDTO>>(albums);
    }
}