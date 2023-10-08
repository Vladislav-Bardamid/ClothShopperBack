using System.Security.Claims;
using ClothShopperBack.BLL.Models;
using ClothShopperBack.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClothShopperBack.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClothController : ControllerBase
{
    private readonly IClothService _clothService;

    public ClothController(IClothService clothService)
    {
        _clothService = clothService;
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult> GetClothesAsync([FromQuery] ClothesFilterModelDTO filter)
    {
        try
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            filter.UserId = userId;
            
            var photos = await _clothService.GetCachedClothesAsync(filter);

            return Ok(photos);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}