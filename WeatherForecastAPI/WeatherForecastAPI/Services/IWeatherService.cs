using WeatherForecastAPI.DTOs;

namespace WeatherForecastAPI.Services
{
    public interface IWeatherService
    {
        Task<WeatherForecastResponse> GetWeatherAsync(double latitude, double longitude);
    }
}
