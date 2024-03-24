using CatTime.Shared.Routes.Auth;
using CatTime.Shared;
using System.Net.Http.Json;
using CatTime.Shared.Routes.Employees;

namespace CatTime.Frontend.Infrastructure.Service;

public partial class ClientService
{
    public async Task<List<EmployeeDTO>> GetEmployees(bool showInactive=false)
    {
        var response = await this._httpClient.GetAsync($"/employees?showInactive={showInactive}");
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<List<EmployeeDTO>>();
    }

    public async Task<EmployeeDTO> CreateEmployee(CreateEmployeeRequest request)
    {
        var response = await this._httpClient.PostAsJsonAsync("/employees", request);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<EmployeeDTO>();
    }

    public async Task<EmployeeDTO> UpdateEmployee(int id, UpdateEmployeeRequest request)
    {
        var response = await this._httpClient.PutAsJsonAsync($"/employees/{id}", request);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<EmployeeDTO>();
    }

    public async Task<EmployeeDTO> GetEmployee(int id)
    {
        var response = await this._httpClient.GetAsync($"/employees/{id}");
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<EmployeeDTO>();
    }

    public async Task ChangeEmployeePassword(ChangePasswordRequest request)
    {
        var response = await this._httpClient.PostAsJsonAsync("/employees/actions/change-password", request);
        response.EnsureSuccessStatusCode();
    }
}