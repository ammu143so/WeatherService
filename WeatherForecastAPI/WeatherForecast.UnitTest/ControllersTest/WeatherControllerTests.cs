using Microsoft.AspNetCore.Mvc;
using Moq;
using WeatherForecastAPI.Controllers;
using WeatherForecastAPI.DTOs;
using WeatherForecastAPI.Models;
using WeatherForecastAPI.Repository;
using WeatherForecastAPI.Services;

namespace WeatherForecast.UnitTest.ControllersTest
{
    public class WeatherControllerTests
    {
        private readonly Mock<IWeatherService> _weatherServiceMock = new();
        private readonly Mock<ILocationRepository> _locationRepoMock = new();
        private readonly WeatherController _controller;

        public WeatherControllerTests()
        {
            _controller = new WeatherController(_weatherServiceMock.Object, _locationRepoMock.Object);
        }

        [Fact]
        public async Task AddLocation_ShouldReturnOk()
        {
            var request = new AddLocationRequest { Latitude = 30.0, Longitude = 70.0 };

            _locationRepoMock.Setup(r => r.GetByCoordinatesAsync(30.0, 70.0)).ReturnsAsync((Location?)null);
            _locationRepoMock.Setup(r => r.AddLocationAsync(It.IsAny<Location>()))
                .ReturnsAsync(new Location { Id = 1, Latitude = 30.0, Longitude = 70.0 });

            var result = await _controller.AddLocation(request);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var data = Assert.IsType<Location>(okResult.Value);
            Assert.Equal(30.0, data.Latitude);
        }

        [Fact]
        public async Task GetForecast_ReturnsValidForecast()
        {
            var response = new WeatherForecastResponse
            {
                Latitude = 10,
                Longitude = 20,
                Temperature = 25,
                Date = DateTime.UtcNow
            };

            _weatherServiceMock.Setup(s => s.GetWeatherAsync(10, 20)).ReturnsAsync(response);

            var result = await _controller.GetForecast(10, 20);

            var ok = Assert.IsType<OkObjectResult>(result);
            var data = Assert.IsType<WeatherForecastResponse>(ok.Value);
            Assert.Equal(25, data.Temperature);
        }

        [Fact]
        public async Task ListLocations_ReturnsAll()
        {
            var list = new List<Location>
        {
            new Location { Id = 1, Latitude = 10, Longitude = 20 },
            new Location { Id = 2, Latitude = 11, Longitude = 21 }
        };

            _locationRepoMock.Setup(r => r.GetAllLocationsAsync()).ReturnsAsync(list);

            var result = await _controller.ListLocations();

            var ok = Assert.IsType<OkObjectResult>(result);
            var data = Assert.IsType<List<Location>>(ok.Value);
            Assert.Equal(2, data.Count);
        }
    }
}
