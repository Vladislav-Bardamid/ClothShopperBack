using AutoMapper;
using ClothShopperBack.BLL.Models;
using ClothShopperBack.DAL;
using ClothShopperBack.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClothShopperBack.BLL.Services;

public interface IOrderService
{
    Task<OrderListDTO> GetOrderListAsync(int userId);
    Task<OrderListDTO> GetOrderListByIdAsync(int listId, int userId);
    Task<List<OrderListDTO>> GetCompletedOrderListsAsync(int userId);
    Task<int> GetUserSumPrice(int userId);
    Task ChangeOrdersAsync(OrderListCommand command, int userId);
    Task DeleteOrderAsync(int id);
    Task DeleteAllUserOrdersAsync(int userId);
    Task DeleteAllUserAlbumOrdersAsync(int userId, int albumId);
    DateTime GetOrderListCommitDate();
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

    public async Task<OrderListDTO> GetOrderListAsync(int userId)
    {
        var orderList = await _context.OrderLists
            .Include(x => x.Orders)
            .ThenInclude(x => x.Cloth)
            .SingleOrDefaultAsync(x => x.UserId == userId && x.CommitDate == null);

        var result = _mapper.Map<OrderListDTO>(orderList);

        result.PriceSum = orderList!.Orders.Sum(x => x.Cloth!.Price);
        result.CommitDate = GetOrderListCommitDate();

        return result;
    }

    public DateTime GetOrderListCommitDate()
    {
        var now = DateTime.Now;
        var dayOfWeek = DayOfWeek.Tuesday;
        var hours = 22;
        var diff =
            new TimeSpan((int)dayOfWeek, hours, 0, 0)
            - new TimeSpan((int)now.DayOfWeek, now.Hour, 0, 0);
        var dayDiff = ((int)dayOfWeek - diff.Days + 7) % 7;

        return now.Date.AddDays(dayDiff).AddHours(hours);
    }

    public async Task<OrderListDTO> GetOrderListByIdAsync(int listId, int userId)
    {
        var orderList = await _context.OrderLists
            .Include(x => x.Orders)
            .SingleOrDefaultAsync(x => x.Id == listId && x.UserId == userId);

        var result = _mapper.Map<OrderListDTO>(orderList);

        result.PriceSum = orderList!.Orders.Sum(x => x.Cloth!.Price);

        return result;
    }

    public async Task<List<OrderListDTO>> GetCompletedOrderListsAsync(int userId)
    {
        var orderLists = await _context.OrderLists
            .Where(x => x.UserId == userId && x.CommitDate != null)
            .ToListAsync();

        return _mapper.Map<List<OrderListDTO>>(orderLists);
    }

    public async Task<int> GetUserSumPrice(int userId)
    {
        var orderList = await _context.OrderLists
            .Include(x => x.Orders)
            .ThenInclude(x => x.Cloth)
            .SingleOrDefaultAsync(x => x.UserId == userId);

        return orderList?.Orders.Sum(x => x.Cloth!.Price) ?? 0;
    }

    public async Task ChangeOrdersAsync(OrderListCommand command, int userId)
    {
        await AddOrdersAsync(command.Add, userId);
        await DeleteOrdersAsync(command.Delete);

        await _context.SaveChangesAsync();
    }

    private async Task AddOrdersAsync(IEnumerable<int> list, int userId)
    {
        var orderList = _context.OrderLists.SingleOrDefault(x => x.UserId == userId);

        if (orderList == null)
        {
            orderList = new OrderList()
            {
                UserId = userId,
                Orders = list.Select(x => new Order() { ClothId = x }).ToList()
            };

            await _context.OrderLists.AddAsync(orderList);
        }
        else
        {
            var orders = list.Select(x => new Order() { ClothId = x, OrderListId = orderList.Id })
                .ToList();

            await _context.Orders.AddRangeAsync(orders);
        }
    }

    private async Task DeleteOrdersAsync(IEnumerable<int> list)
    {
        var models = _context.Orders.Where(x => list.Contains(x.ClothId));

        _context.Orders.RemoveRange(models);
    }

    public async Task DeleteOrderAsync(int id)
    {
        var order = await _context.Orders.SingleAsync(x => x.Id == id);

        _context.Orders.Remove(order);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAllUserOrdersAsync(int userId)
    {
        var ordersToRemove = _context.OrderLists
            .Where(x => x.UserId == userId && x.CommitDate == null)
            .SelectMany(x => x.Orders);

        _context.Orders.RemoveRange(ordersToRemove);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAllUserAlbumOrdersAsync(int userId, int albumId)
    {
        var ordersToRemove = _context.OrderLists
            .Include(x => x.Orders)
            .ThenInclude(x => x.Cloth)
            .Where(x => x.UserId == userId && x.CommitDate == null)
            .SelectMany(x => x.Orders)
            .Where(x => x.Cloth!.AlbumId == albumId);

        _context.Orders.RemoveRange(ordersToRemove);

        await _context.SaveChangesAsync();
    }
}
