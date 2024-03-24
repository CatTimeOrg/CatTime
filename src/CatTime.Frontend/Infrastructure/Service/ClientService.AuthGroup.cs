using CatTime.Shared.Routes.Auth;
using CatTime.Shared;
using System.Net.Http.Json;

namespace CatTime.Frontend.Infrastructure.Service
{
    public partial class ClientService
    {
        public async Task<LoginResponse> Login(string emailAddress, string password)
        {
            var request = new LoginRequest
            {
                EmailAddress = emailAddress,
                Password = password,
            };
            var response = await this._httpClient.PostAsJsonAsync("/auth/login", request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<LoginResponse>();
        }

        public async Task<LoginResponse> Refresh(string refreshToken)
        {
            var request = new RefreshRequest
            {
                RefreshToken = refreshToken
            };
            var response = await this._httpClient.PostAsJsonAsync("/auth/refresh", request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<LoginResponse>();
        }

        public async Task Register(string firstName, string lastName, string emailAddress, string password)
        {
            var request = new RegisterRequest
            {
                FirstName = firstName,
                LastName = lastName,
                EmailAddress = emailAddress,
                Password = password
            };
            var response = await this._httpClient.PostAsJsonAsync("/auth/register", request);
            response.EnsureSuccessStatusCode();
        }

        public async Task<EmployeeDTO> Me()
        {
            var response = await this._httpClient.GetAsync("/auth/me");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<EmployeeDTO>();
        }
    }
}
