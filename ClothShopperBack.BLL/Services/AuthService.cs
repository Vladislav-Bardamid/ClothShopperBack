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

public interface IAuthService
{
    Task<UserDTO> Login(LoginDTO model);
    Task<UserDTO> Logout(LoginDTO model);
}

public class AuthService : IAuthService
{
    private UserManager<User> _userManager;
    private IVkAPI _vkAPI;
    private IConfiguration _configuration;

    public AuthService(IVkAPI vkAPI, UserManager<User> userManager, IConfiguration configuration)
    {
        _vkAPI = vkAPI;
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<UserDTO> Login(LoginDTO model)
    {
        var vkToken = await _vkAPI.GetToken(model.Email, model.Password);
        var vkUserProfile = await _vkAPI.GetProfileInfoAsync(vkToken.AccessToken);

        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.VkUserId == vkToken.UserId);
        if (user == null)
        {
            user = new User
            {
                VkUserId = vkToken.UserId,
                VkAccessToken = vkToken.AccessToken,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Email,
                Email = model.Email
            };

            var identityResult = await _userManager.CreateAsync(user);
        }
        else
        {
            user.VkAccessToken = vkToken.AccessToken;
        }

        string token = CreateToken(user);

        var result = new UserDTO
        {
            UserName = user.UserName,
            AccessToken = token,
        };

        return result;
    }

    public Task<UserDTO> Logout(LoginDTO model)
    {
        throw new NotImplementedException();
    }

    private string CreateToken(User? user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
        };

        var jwt = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]!)),
                SecurityAlgorithms.HmacSha256
            )
        );

        var token = new JwtSecurityTokenHandler().WriteToken(jwt);
        return token;
    }
}