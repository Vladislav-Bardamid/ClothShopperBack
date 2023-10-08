using AutoMapper;
using ClothShopperBack.BLL.Models;
using ClothShopperBack.DAL;
using ClothShopperBack.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClothShopperBack.BLL.Services;

public interface IOrderService
{
    Task<List<OrderDTO>> GetUnlistedOrdersAsync(int userId);
    Task<List<OrderDTO>> GetOrdersByListIdAsync(int listId, int userId);
    Task<List<OrderListDTO>> GetCompletedOrderListsAsync(int userId);
    Task CreateOrdersAsync(IEnumerable<int> list, int userId);
    Task DeleteOrdersAsync(IEnumerable<int> list, int userId);
    Task DeleteOrdersAsync(int userId);
}

public class OrderService : IOrderService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public OrderService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<OrderDTO>> GetUnlistedOrdersAsync(int userId)
    {
        var orders = await _context.Orders.Where(x => x.UserId == userId && x.OrderListId == null).ToListAsync();

        return _mapper.Map<List<OrderDTO>>(orders);
    }

    public async Task<List<OrderDTO>> GetOrdersByListIdAsync(int listId, int userId)
    {
        var orders = await _context.Orders.Where(x => x.OrderListId == listId && x.UserId == userId).ToListAsync();

        return _mapper.Map<List<OrderDTO>>(orders);
    }

    public async Task<List<OrderListDTO>> GetCompletedOrderListsAsync(int userId)
    {
        var orderLists = await _context.OrderLists.Where(x => x.UserId == userId && x.CommitDate != null).ToListAsync();

        return _mapper.Map<List<OrderListDTO>>(orderLists);
    }

    public async Task CreateOrdersAsync(IEnumerable<int> orderIds, int userId)
    {
        var orders = orderIds.Select(x => new Order()
        {
            ClothId = x,
            UserId = userId
        }).ToList();

        var orderList = new OrderList()
        {
            UserId = userId,
            Orders = orders
        };

        await _context.AddAsync(orderList);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteOrdersAsync(IEnumerable<int> list, int userId)
    {
        var models = list.Select(x => new Order()
        {
            ClothId = x,
            UserId = userId
        });

        _context.Orders.RemoveRange(models);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteOrdersAsync(int userId)
    {
        _context.Orders.RemoveRange(_context.Orders.Where(x => x.UserId == userId));

        await _context.SaveChangesAsync();
    }
}