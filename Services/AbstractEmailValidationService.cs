using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace INF4001N_1814748_NVSAAY001_2024.Services
{
    public interface IEmailValidationService
    {
        Task<bool> IsEmailValidAsync(string email);
    }

    public class AbstractEmailValidationService : IEmailValidationService
    {
        private readonly HttpClient _httpClient;

        public AbstractEmailValidationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> IsEmailValidAsync(string email)
        {
            var apiKey = "b92b785a16f74231b3d39ad965055dea"; 
            var apiUrl = $"https://emailvalidation.abstractapi.com/v1/?api_key={apiKey}&email={email}";

            var response = await _httpClient.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                return true; // Consider API failure as invalid email
            }
            else
            {
                return false;
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<EmailValidationResult>(jsonResponse);

            return result.Deliverability == "DELIVERABLE"; // Check deliverability status
        }

        private class EmailValidationResult
        {
            public string Deliverability { get; set; } // "DELIVERABLE", "UNDELIVERABLE", etc.
        }
    }
}
