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
    public async Task<ActionResult<IEnumerable<Order>>> GetOrderListAsync()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        
        var orderList = await _orderService.GetOrderListAsync(userId);

        return Ok(orderList);
    }
    
    [HttpGet("commitDate")]
    public ActionResult GetOrderListCommitDate()
    {
        var result = _orderService.GetOrderListCommitDate();

        return Ok(result);
    }

    [Authorize]
    [HttpGet("sum")]
    public async Task<ActionResult<IEnumerable<Order>>> GetUserSumPrice()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        
        var result = await _orderService.GetUserSumPrice(userId);

        return Ok(result);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult> ChangeOrdersAsync(OrderListCommand command)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        await _orderService.ChangeOrdersAsync(command, userId);

        return Ok();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteOrderAsync(int id)
    {
        await _orderService.DeleteOrderAsync(id);

        return Ok();
    }

    [Authorize]
    [HttpDelete("album/{albumId}")]
    public async Task<ActionResult> DeleteAllUserAlbumOrdersAsync(int albumId)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        await _orderService.DeleteAllUserAlbumOrdersAsync(userId, albumId);

        return Ok();
    }

    [Authorize]
    [HttpDelete("all")]
    public async Task<ActionResult> DeleteAllUserOrdersAsync()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        await _orderService.DeleteAllUserOrdersAsync(userId);

        return Ok();
    }
}