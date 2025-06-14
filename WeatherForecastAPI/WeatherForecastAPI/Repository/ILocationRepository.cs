using WeatherForecastAPI.Models;

namespace WeatherForecastAPI.Repository
{
    public interface ILocationRepository
    {
        Task<List<Location>> GetAllLocationsAsync();
        Task<Location?> GetByCoordinatesAsync(double lat, double lon);
        Task<Location> AddLocationAsync(Location location);
        Task<bool> DeleteLocationAsync(int id);
    }
}
