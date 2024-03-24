using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using CatTime.Shared;
using CatTime.Shared.Routes.Auth;

namespace CatTime.Frontend.Infrastructure.Service;

public partial class ClientService
{
    private readonly HttpClient _httpClient;

    public ClientService(HttpClient httpClient)
    {
        this._httpClient = httpClient;
    }

    public void SetAccessToken(string accessToken)
    {
        this._httpClient.DefaultRequestHeaders.Authorization = string.IsNullOrWhiteSpace(accessToken) is false
            ? new AuthenticationHeaderValue("Bearer", accessToken)
            : null;
    }
}