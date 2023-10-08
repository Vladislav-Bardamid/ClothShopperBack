using AutoMapper;
using ClothShopperBack.API.Models;
using ClothShopperBack.BLL.Models;
using ClothShopperBack.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClothShopperBack.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IUserService _userService;

    public UserController(IMapper mapper, IUserService userService)
    {
        _mapper = mapper;
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult> GetAllUsersAsync()
    {
        var result = await _userService.GetAllUsersAsync();

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeteteUserByIdAsync(int id)
    {
        await _userService.DeteteUserByIdAsync(id);

        return Ok();
    }
}