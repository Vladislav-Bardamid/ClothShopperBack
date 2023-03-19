using System.Net.Http.Json;
using System.Text.Json;
using ClothShopperBack.DAL.Common.VkApiModels;
using ClothShopperBack.DAL.Entities;
using Microsoft.Extensions.Configuration;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ClothShopperBack.DAL.Common;

public interface IVkAPI
{
    Task<List<VkPhoto>?> GetPhotos(int ownerId, int albumId);
}

public class VkAPI : IVkAPI
{
    private string _baseUri = "https://api.vk.com/method";
    private HttpClient _client;
    private readonly IConfiguration _configuration;

    public VkAPI(IConfiguration configuration)
    {
        _client = new HttpClient();
        _configuration = configuration;
    }

    public async Task<List<VkPhoto>?> GetPhotos(int ownerId, int albumId)
    {
        var accessToken = _configuration["AccessToken"];
        var query = $"{_baseUri}/photos.get?access_token={accessToken}&owner_id=-{ownerId}&album_id={albumId}&v=5.131";

        var result = await SendVkApiResponse<GetVkListResult<VkPhoto>>(query);

        return result.Items;
    }

    private async Task<T> SendVkApiResponse<T>(string query)
    {
        var response = await _client.GetAsync(query);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<VkApiResult<T>>();

        if (result.Error != null) throw new Exception(result.Error.ErrorMessage);

        return result.Response;
    }
}