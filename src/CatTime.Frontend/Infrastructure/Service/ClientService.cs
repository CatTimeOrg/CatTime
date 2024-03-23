using System.Net.Http;
using System.Text.Json;
using System.Text;

namespace CatTime.Frontend.Infrastructure.Service
{
    public class ClientService
    {
        private readonly HttpClient _httpClient;

        public ClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Use _httpClient to make requests

        public async Task<string> Login(string username, string password)
        {
            var loginRequest = new { Username = username, Password = password };
            var content = new StringContent(JsonSerializer.Serialize(loginRequest), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/login", content);

            if (response.IsSuccessStatusCode)
            {
                var jwtToken = await response.Content.ReadAsStringAsync();
                return jwtToken;
            }
            else
            {
                throw new HttpRequestException($"Login failed with status code {response.StatusCode}");
            }
        }
    }
}