//using System.Net.Http;

//namespace AnwendungWPF.Services
//{
//    public class ApiService
//    {
//        private readonly HttpClient _httpClient;

//        public ApiService(HttpClient httpClient)
//        {
//            _httpClient = httpClient;
//        }

//        public async Task<string> GetAddressesAsync()
//        {
//            var response = await _httpClient.GetAsync("api/addresses");

//            response.EnsureSuccessStatusCode();

//            return await response.Content.ReadAsStringAsync();
//        }
//    }
//}
