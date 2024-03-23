using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using CatTime.Frontend.Infrastructure.Service;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private const string LocalStorageKey = "authToken";

    private readonly LocalStorageService _localStorageService;

    public CustomAuthenticationStateProvider(LocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
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
        var identity = string.IsNullOrEmpty(token)
            ? new ClaimsIdentity()
            : new ClaimsIdentity(token);

        return new AuthenticationState(new ClaimsPrincipal(identity));
    }
}