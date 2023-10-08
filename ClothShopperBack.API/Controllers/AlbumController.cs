using AutoMapper;
using ClothShopperBack.BLL.Models;
using ClothShopperBack.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClothShopperBack.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AlbumController : ControllerBase
{
    private readonly IAlbumService _albumService;

    public AlbumController(IAlbumService photoService)
    {
        _albumService = photoService;
    }

    [HttpGet]
    public async Task<ActionResult> GetAlbumsAsync()
    {
        try
        {
            var albums = await _albumService.GetAlbumsAsync();

            return Ok(albums);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}