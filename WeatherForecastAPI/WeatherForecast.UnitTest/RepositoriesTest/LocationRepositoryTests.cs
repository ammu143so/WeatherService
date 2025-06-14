using Microsoft.EntityFrameworkCore;
using WeatherForecastAPI.Data;
using WeatherForecastAPI.Models;
using WeatherForecastAPI.Repository;

namespace WeatherForecast.UnitTest.RepositoriesTest
{
    public class LocationRepositoryTests
    {
        private readonly WeatherDbContext _context;
        private readonly LocationRepository _repository;

        public LocationRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<WeatherDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new WeatherDbContext(options);
            _repository = new LocationRepository(_context);
        }

        [Fact]
        public async Task AddLocationAsync_AddsSuccessfully()
        {
            var location = new Location { Latitude = 10.0, Longitude = 20.0 };

            var result = await _repository.AddLocationAsync(location);

            Assert.NotNull(result);
            Assert.Equal(10.0, result.Latitude);
            Assert.Single(await _repository.GetAllLocationsAsync());
        }

        [Fact]
        public async Task GetByCoordinatesAsync_ReturnsCorrectLocation()
        {
            await _repository.AddLocationAsync(new Location { Latitude = 12.3, Longitude = 45.6 });

            var loc = await _repository.GetByCoordinatesAsync(12.3, 45.6);

            Assert.NotNull(loc);
            Assert.Equal(12.3, loc?.Latitude);
        }

        [Fact]
        public async Task DeleteLocationAsync_RemovesLocation()
        {
            var loc = await _repository.AddLocationAsync(new Location { Latitude = 1.1, Longitude = 2.2 });

            var result = await _repository.DeleteLocationAsync(loc.Id);

            Assert.True(result);
            Assert.Empty(await _repository.GetAllLocationsAsync());
        }
    }

}
