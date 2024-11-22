using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace INF4001N_1814748_NVSAAY001_2024.Services
{
    //Interface defining the contract for email validation services
    public interface IEmailValidationService
    {
        //Asynchronous method to validate an email address
        Task<bool> IsEmailValidAsync(string email);
    }

    //Implementation of the IEmailValidationService using Abstract Email Validation API
    public class AbstractEmailValidationService : IEmailValidationService
    {
        //HTTP client for making API requests
        private readonly HttpClient _httpClient;

        //Constructor to inject the HttpClient dependency
        public AbstractEmailValidationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        //Method to validate an email address by making a request to Abstract API
        public async Task<bool> IsEmailValidAsync(string email)
        {
            //API key for Abstract Email Validation API
            var apiKey = "b92b785a16f74231b3d39ad965055dea";

            //Construct the API URL with the provided email and API key
            var apiUrl = $"https://emailvalidation.abstractapi.com/v1/?api_key={apiKey}&email={email}";

            //Send a GET request to the API
            var response = await _httpClient.GetAsync(apiUrl);

            //Check if the response indicates success
            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            //Read the JSON response content as a string
            var jsonResponse = await response.Content.ReadAsStringAsync();

            //Deserialize the JSON response into an EmailValidationResult object
            var result = JsonSerializer.Deserialize<EmailValidationResult>(jsonResponse);

            //Check the deliverability status from the API response
            return result.Deliverability == "DELIVERABLE"; // Check deliverability status
        }

        //Private class to represent the response structure from the Abstract API
        private class EmailValidationResult
        {
            //Property to store the deliverability status ("DELIVERABLE", "UNDELIVERABLE", etc.)
            public string Deliverability { get; set; } 
        }
    }
}