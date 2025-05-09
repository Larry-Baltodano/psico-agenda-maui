using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace PsicoAgenda.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthService _authService;
        private const string BaseUrl = "https://bnhzk05r-5229.use2.devtunnels.ms/api/";

        public ApiService()
        {
            _httpClient = new HttpClient();
            _authService = new AuthService();
        }

        private async Task AddAuthorizationHeaderAsync()
        {
            var token = await _authService.GetTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<T> GetAsync<T>(string endpoint)
        {
            await AddAuthorizationHeaderAsync();
            var response = await _httpClient.GetAsync(BaseUrl + endpoint);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }

        public async Task<T> PostAsync<T>(String endpoint, object data)
        {
            await AddAuthorizationHeaderAsync();
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(BaseUrl + endpoint, content);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseContent);
        }

        public async Task<T> PutAsync<T>(string endpoint, object data)
        {
            await AddAuthorizationHeaderAsync();
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(BaseUrl + endpoint, content);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseContent);
        }

        public async Task DeleteAsync(string endpoint)
        {
            await AddAuthorizationHeaderAsync();
            var response = await _httpClient.DeleteAsync(BaseUrl + endpoint);
            response.EnsureSuccessStatusCode();
        }
    }
}
