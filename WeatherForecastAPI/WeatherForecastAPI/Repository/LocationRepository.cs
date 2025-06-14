using Microsoft.EntityFrameworkCore;
using WeatherForecastAPI.Data;
using WeatherForecastAPI.Models;

namespace WeatherForecastAPI.Repository
{
    public class LocationRepository : ILocationRepository
    {
        private readonly WeatherDbContext _context;

        public LocationRepository(WeatherDbContext context)
        {
            _context = context;
        }

        public async Task<List<Location>> GetAllLocationsAsync() =>
            await _context.Locations.ToListAsync();

        public async Task<Location?> GetByCoordinatesAsync(double lat, double lon) =>
            await _context.Locations.FirstOrDefaultAsync(x =>
                Math.Abs(x.Latitude - lat) < 0.0001 &&
                Math.Abs(x.Longitude - lon) < 0.0001);

        public async Task<Location> AddLocationAsync(Location location)
        {
            _context.Locations.Add(location);
            await _context.SaveChangesAsync();
            return location;
        }

        public async Task<bool> DeleteLocationAsync(int id)
        {
            var loc = await _context.Locations.FindAsync(id);
            if (loc == null) return false;
            _context.Locations.Remove(loc);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
