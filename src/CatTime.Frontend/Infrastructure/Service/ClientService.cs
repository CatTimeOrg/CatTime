using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using CatTime.Shared;
using CatTime.Shared.Routes.Auth;

namespace CatTime.Frontend.Infrastructure.Service
{
    public partial class ClientService
    {
        protected readonly HttpClient _httpClient;

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

        public async Task<CompanyDTO> GetCompany()
        {
            var response = await this._httpClient.GetAsync("/companies/me");
            response.EnsureSuccessStatusCode();
            
            return await response.Content.ReadFromJsonAsync<CompanyDTO>();
        }

        public async Task<CompanyDTO> UpdateCompany(CompanyDTO company)
        {
            var response = await this._httpClient.PutAsJsonAsync("/companies/me", company);
            response.EnsureSuccessStatusCode();
            
            return await response.Content.ReadFromJsonAsync<CompanyDTO>();
        }
    }
    
    public class LoginResponse
    {
        public string TokenType { get; set; }
        public string AccessToken { get; set; }
        public int ExpiresIn { get; set; }
        public string RefreshToken { get; set; }
    }
}