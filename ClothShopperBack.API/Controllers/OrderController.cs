using System.Security.Claims;
using AutoMapper;
using ClothShopperBack.API.Models;
using ClothShopperBack.BLL.Models;
using ClothShopperBack.BLL.Services;
using ClothShopperBack.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClothShopperBack.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Order>>> GetUnlistedOrdersAsync()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var orders = await _orderService.GetUnlistedOrdersAsync(userId);

        return Ok(orders);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult> CreateOrdersAsync(IEnumerable<int> list)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        try
        {
            await _orderService.CreateOrdersAsync(list, userId);

            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [Authorize]
    [HttpDelete]
    public async Task<ActionResult> RemoveOrdersAsync(IEnumerable<int> list)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        try
        {
            await _orderService.DeleteOrdersAsync(list, userId);

            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [Authorize]
    [HttpDelete("all")]
    public async Task<ActionResult> RemoveAllOrdersAsync()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        try
        {
            await _orderService.DeleteOrdersAsync(userId);

            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}