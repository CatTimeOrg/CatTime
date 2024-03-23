using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using CatTime.Shared;
using CatTime.Frontend.Infrastructure.Service;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private const string LocalStorageKey = "currentUser";

    private readonly LocalStorageService _localStorageService;

    public CustomAuthenticationStateProvider(LocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var currentUser = await GetCurrentUserAsync();

        if (currentUser == null)
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        Claim[] claims = [
            new Claim(ClaimTypes.NameIdentifier, currentUser.Id!.ToString()!),
                new Claim(ClaimTypes.Name, currentUser.EmailAddress!.ToString()!),
                new Claim(ClaimTypes.Email, currentUser.EmailAddress!.ToString()!)
        ];

        var authenticationState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims, authenticationType: nameof(CustomAuthenticationStateProvider))));

        return authenticationState;
    }

    public async Task SetCurrentUserAsync(EmployeeDTO? currentUser)
    {
        await _localStorageService.SetItem(LocalStorageKey, currentUser);

        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public Task<EmployeeDTO?> GetCurrentUserAsync() => _localStorageService.GetItemAsync<EmployeeDTO>(LocalStorageKey);
}