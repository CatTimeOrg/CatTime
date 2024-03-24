using System.Net.Http.Json;
using CatTime.Shared.Routes.Attendances;

namespace CatTime.Frontend.Infrastructure.Service;

public partial class ClientService
{
    public async Task<List<AttendanceDTO>> GetAttendances()
    {
        var response = await this._httpClient.GetAsync("/attendance");
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<List<AttendanceDTO>>();
    }
}