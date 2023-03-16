using AutoMapper;
using ClothShopperBack.API.Models;
using ClothShopperBack.BLL.Models;
using ClothShopperBack.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClothShopperBack.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class ClothesController : BaseController
{
    private readonly IPhotoService _photoService;
    private readonly IMapper _mapper;

    public ClothesController(IPhotoService photoService, IMapper mapper)
    {
        _photoService = photoService;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult> GetClothes(ClothesFilterModel? filter = null) => await MakeDefaultAction(async () =>
    {
        var photos = await _photoService.GetPhotos(filter);
        var photosViewModels = _mapper.Map<List<ClothViewModel>>(photos);

        return photosViewModels;
    });
}