using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Io;
using System.Net.Http.Json;
using ClothShopperBack.DAL.Common.VkApiModels;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace ClothShopperBack.DAL.Common;

public interface IVkAPI
{
    Task<List<VkPhoto>?> GetPhotosAsync(int ownerId, int albumId);
    Task<List<VkAlbum>?> GetAlbumsAsync(int ownerId, IEnumerable<int> albumIds);
    Task<VkToken> GetToken(string username, string password);
    Task<VkProfileInfo?> GetProfileInfoAsync(string accessToken);
}

public class VkAPI : IVkAPI
{
    public readonly static string Version = "5.131";
    private readonly string _baseUri = "https://api.vk.com/method";
    private readonly HttpClient _client;
    private readonly IConfiguration _configuration;

    public VkAPI(IConfiguration configuration, HttpClient client)
    {
        _client = client;
        _configuration = configuration;
    }

    public async Task<List<VkPhoto>?> GetPhotosAsync(int ownerId, int albumId)
    {
        var accessToken = _configuration["VK:ServiceToken"];
        var query = $"photos.get?access_token={accessToken}&owner_id=-{ownerId}&album_id={albumId}&v={Version}";

        var result = await SendVkApiResponseAsync<GetVkListResult<VkPhoto>>(query);
        foreach (var photo in result.Items)
        {
            photo.OwnerId = ownerId;
            photo.AlbumId = albumId;
        }

        return result.Items;
    }

    public async Task<VkToken> GetToken(string username, string password)
    {
        var clientId = _configuration["Vk:ClientId"]!;
        var filtered = new List<Request>();
        var config = Configuration.Default.WithRequesters().WithMetaRefresh().WithDefaultCookies().WithDefaultLoader();
        var browsingContext = BrowsingContext.New(config);

        var query = $"https://oauth.vk.com/authorize?client_id={clientId}&display=mobile&redirect_uri=https://oauth.vk.com/blank.html&scope=photos,offline&response_type=token&v=5.131";

        try
        {
            await browsingContext.OpenAsync(query);
        }
        catch (Exception ex)
        {
            throw new Exception("Can't send request", ex);
        }

        var form = browsingContext.Active.QuerySelector<IHtmlFormElement>("form");
        var formInputs = form.Elements.OfType<IHtmlInputElement>();

        formInputs.Single(x => x.Name == "email").SetAttribute("value", username);
        formInputs.Single(x => x.Name == "pass").SetAttribute("value", password);

        var submitButton = formInputs.Single(x => x.Type == "submit");

        await form.SubmitAsync();

        var serviceMsg = browsingContext.Active.QuerySelector<IHtmlDivElement>(".service_msg");

        if (serviceMsg != null)
            throw new Exception(serviceMsg.InnerHtml);

        if (browsingContext.Active.Url.StartsWith("https://oauth.vk.com/blank.html"))
            return ConvertToken(browsingContext.Active.BaseUrl.Fragment);

        var accessForm = browsingContext.Active.QuerySelector<IHtmlFormElement>("form");

        await accessForm.SubmitAsync();

        return ConvertToken(browsingContext.Active.BaseUrl.Fragment);
    }

    public async Task<List<VkAlbum>?> GetAlbumsAsync(int ownerId, IEnumerable<int> albumIds)
    {
        var accessToken = _configuration["VK:ServiceToken"];
        var query = $"photos.getAlbums?access_token={accessToken}&owner_id=-{ownerId}&album_ids={string.Join(",", albumIds)}&v={Version}";

        var result = await SendVkApiResponseAsync<GetVkListResult<VkAlbum>>(query);

        return result.Items;
    }

    public async Task<VkProfileInfo?> GetProfileInfoAsync(string accessToken)
    {
        var query = $"account.getProfileInfo?access_token={accessToken}&v={Version}";

        var result = await SendVkApiResponseAsync<VkProfileInfo>(query);

        return result;
    }

    private async Task<T> SendVkApiResponseAsync<T>(string query)
    {
        var response = await _client.GetAsync($"{_baseUri}/{query}");
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<VkApiResult<T>>();

        if (result.Error != null) throw new Exception(result.Error.ErrorMessage);

        return result.Response;
    }

    private async Task<string> SendResponseAsync(string query)
    {
        var response = await _client.GetStringAsync(query);

        return response;
    }

    private VkToken ConvertToken(string source)
    {
        var array = source.Split('&');
        var token = new VkToken()
        {
            AccessToken = array[0].Split('=')[1],
            ExpiresIn = int.Parse(array[1].Split('=')[1]),
            UserId = int.Parse(array[2].Split('=')[1])
        };

        return token;
    }
}