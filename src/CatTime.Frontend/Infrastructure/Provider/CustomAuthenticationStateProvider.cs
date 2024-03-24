using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using CatTime.Frontend.Infrastructure.Service;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private const string LocalStorageKey = "loginResponse";

    private readonly LocalStorageService _localStorageService;
    private readonly ClientService _clientService;

    public CustomAuthenticationStateProvider(LocalStorageService localStorageService, ClientService clientService)
    {
        this._localStorageService = localStorageService;
        this._clientService = clientService;
    }

    public async Task<LoginResponse?> GetTokenAsync()
    {
        return await this._localStorageService.GetItemAsync<LoginResponse?>(LocalStorageKey);
    }
    public async Task SetTokenAsync(LoginResponse? token)
    {
        if (token == null)
        {
           await this._localStorageService.RemoveItemAsync(LocalStorageKey);
        }
        else
        {
           await this._localStorageService.SetItem<LoginResponse>(LocalStorageKey,token);
        }

        this.NotifyAuthenticationStateChanged(this.GetAuthenticationStateAsync());
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await this.GetTokenAsync();

        if (string.IsNullOrWhiteSpace(token?.AccessToken))
            return new AuthenticationState(new ClaimsPrincipal());

        this._clientService.SetAccessToken(token.AccessToken);

        try
        {
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
        catch (Exception e)
        {
            // Something went wrong, token is probably expired, try refreshing it with the refresh-token
            try
            {
                var newToken = await this._clientService.Refresh(token.RefreshToken);
                await this.SetTokenAsync(newToken);

                // We refreshed the token, try again
                return await this.GetAuthenticationStateAsync();
            }
            catch (Exception exception)
            {
                // Nope, still didn't work, we bail out, user has to login again
                await this.SetTokenAsync(null);
                return new AuthenticationState(new ClaimsPrincipal());
            }
        }
    }
}