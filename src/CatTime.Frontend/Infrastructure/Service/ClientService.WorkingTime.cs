using CatTime.Shared.Routes.WorkingTimes;
using CatTime.Shared;
using System.Net.Http;
using CatTime.Shared.Routes.Employees;
using System.Net.Http.Json;
using CatTime.Frontend.Helper;

namespace CatTime.Frontend.Infrastructure.Service;

public partial class ClientService
{
    public async Task<List<WorkingTimeDTO>> GetWorkingTimes(DateOnly? from=null, DateOnly? to=null, int? employeeId=null)
    {
        var urlQueryBuilder = new UrlQueryBuilder("/workingtime");            
        urlQueryBuilder.AddParameter("from", from);
        urlQueryBuilder.AddParameter("to", to);
        urlQueryBuilder.AddParameter("employeeId", employeeId);

        var response = await this._httpClient.GetAsync(urlQueryBuilder);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<List<WorkingTimeDTO>>();
    }

    public async Task<WorkingTimeDTO> CreateWorkingTime(CreateWorkingTimeRequest request)
    {
        var response = await this._httpClient.PostAsJsonAsync("/workingtime", request);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<WorkingTimeDTO>();
    }

    public async Task<WorkingTimeDTO> UpdateWorkingTime(int id, UpdateWorkingTimeRequest request)
    {
        var response = await this._httpClient.PutAsJsonAsync($"/workingtime/{id}", request);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<WorkingTimeDTO>();
    }

    public async Task<WorkingTimeDTO> GetWorkingTime(int id)
    {
        var response = await this._httpClient.GetAsync($"/workingtime/{id}");
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<WorkingTimeDTO>();
    }

    public async Task DeleteWorkingTime(int id)
    {
        var response = await this._httpClient.DeleteAsync($"/workingtime/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<CurrentTimeDTO> GetCurrentWorkingTime()
    {
        var response = await this._httpClient.GetAsync($"/workingtime/current");
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<CurrentTimeDTO>();
    }

    public async Task<WorkingTimeDTO> CheckIn(CheckinRequest request)
    {
        var response = await this._httpClient.PostAsJsonAsync($"/workingtime/actions/checkin", request);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<WorkingTimeDTO>();
    }

    public async Task<WorkingTimeDTO> CheckOut()
    {
        var response = await this._httpClient.PostAsync($"/workingtime/actions/checkout", null);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<WorkingTimeDTO>();
    }
}