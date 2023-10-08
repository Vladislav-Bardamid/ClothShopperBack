using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using ClothShopperBack.API.Models;
using ClothShopperBack.BLL.Models;
using ClothShopperBack.DAL;
using ClothShopperBack.DAL.Common;
using ClothShopperBack.DAL.Common.VkApiModels;
using ClothShopperBack.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ClothShopperBack.BLL.Services;

public interface IUserService
{
    Task<List<UserDTO>> GetAllUsersAsync();
    Task DeteteUserByIdAsync(int id);
}

public class UserService : IUserService
{
    private UserManager<User> _userManager;
    private RoleManager<Role> _roleManager;
    private AppDbContext _context;
    private IVkAPI _vkAPI;
    private IMapper _mapper;
    private IConfiguration _configuration;

    public UserService(AppDbContext context, IMapper mapper, IVkAPI vkAPI, UserManager<User> userManager, RoleManager<Role> roleManager, IConfiguration configuration)
    {
        _context = context;
        _mapper = mapper;
        _vkAPI = vkAPI;
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
    }

    public async Task<List<UserDTO>> GetAllUsersAsync()
    {
        var users = await _userManager.Users.ToListAsync();

        return _mapper.Map<List<UserDTO>>(users);
    }

    public async Task DeteteUserByIdAsync(int id)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id);
        await _userManager.DeleteAsync(user);
    }
}