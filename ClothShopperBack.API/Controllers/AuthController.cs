using AutoMapper;
using ClothShopperBack.API.Models;
using ClothShopperBack.BLL.Models;
using ClothShopperBack.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClothShopperBack.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login(LoginDTO model)
    {
        var result = await _authService.Login(model);

        return Ok(result);
    }

    [HttpPost("logout")]
    public async Task<ActionResult> Logout(LoginDTO model)
    {
        try
        {
            var result = await _authService.Logout(model);

            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}