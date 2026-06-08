

using EmployeeApi.Models;

namespace Employee_Dept_Loc_Proj.Services
{
    public class DepartmentApiClient
    {
        private readonly HttpClient _httpclient;

        public DepartmentApiClient(HttpClient httpclient)
        {
            _httpclient = httpclient;
        }

        public async Task<ApiResponse<Department>> GetDepartmentByIdAsync(int id,string JWTToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"Department/GetDepartmentById/{id}");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", JWTToken);

            var response = await _httpclient.SendAsync(request);
            response.EnsureSuccessStatusCode();
          
            var department = await response.Content.ReadFromJsonAsync<ApiResponse<Department>>();
            return department;
        }
    }
}