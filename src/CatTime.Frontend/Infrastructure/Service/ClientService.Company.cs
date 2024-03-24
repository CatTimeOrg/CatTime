using System.Net.Http.Json;
using CatTime.Shared;

namespace CatTime.Frontend.Infrastructure.Service;

public partial class ClientService
{
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