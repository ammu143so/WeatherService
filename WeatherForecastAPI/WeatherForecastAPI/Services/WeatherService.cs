using System.Text.Json;
using WeatherForecastAPI.DTOs;

namespace WeatherForecastAPI.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<WeatherService> _logger;

        public WeatherService(HttpClient httpClient, ILogger<WeatherService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<WeatherForecastResponse> GetWeatherAsync(double latitude, double longitude)
        {
            var url = $"https://api.open-meteo.com/v1/forecast?latitude={latitude}&longitude={longitude}&current_weather=true";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);

            var current = doc.RootElement.GetProperty("current_weather");

            return new WeatherForecastResponse
            {
                Latitude = latitude,
                Longitude = longitude,
                Temperature = current.GetProperty("temperature").GetDouble(),
                Date = current.GetProperty("time").GetDateTime()
            };
        }
    }
}
