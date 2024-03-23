using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using CatTime.Frontend.Infrastructure.Service;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private const string LocalStorageKey = "authToken";

    private readonly LocalStorageService _localStorageService;
    private readonly ClientService _clientService;

    public CustomAuthenticationStateProvider(LocalStorageService localStorageService, ClientService clientService)
    {
        _localStorageService = localStorageService;
        _clientService = clientService;
    }

    public async Task<string> GetTokenAsync() => await _localStorageService.GetItemAsync<string>(LocalStorageKey);

    public async Task SetTokenAsync(string? token)
    {
        if (token == null)
        {
           await _localStorageService.RemoveItemAsync(LocalStorageKey);
        }
        else
        {
           await _localStorageService.SetItem<string>(LocalStorageKey,token);
        }

        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await GetTokenAsync();

        if (string.IsNullOrWhiteSpace(token))
            return new AuthenticationState(new ClaimsPrincipal());

        this._clientService.SetAccessToken(token);

        var me = await this._clientService.Me();

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, me.Id.ToString()),
            new Claim(ClaimTypes.Email, me.EmailAddress),
            new Claim(ClaimTypes.GivenName, me.FirstName),
            new Claim(ClaimTypes.Surname, me.LastName),
        };
        var identity = new ClaimsIdentity(claims, "CatTime.Backend");

        return new AuthenticationState(new ClaimsPrincipal(identity));
    }
}