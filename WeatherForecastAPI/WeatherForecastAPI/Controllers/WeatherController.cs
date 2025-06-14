using Microsoft.AspNetCore.Mvc;
using WeatherForecastAPI.DTOs;
using WeatherForecastAPI.Models;
using WeatherForecastAPI.Repository;
using WeatherForecastAPI.Services;

namespace WeatherForecastAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _weatherService;
        private readonly ILocationRepository _locationRepository;

        public WeatherController(IWeatherService weatherService, ILocationRepository locationRepository)
        {
            _weatherService = weatherService;
            _locationRepository = locationRepository;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddLocation(AddLocationRequest request)
        {
            var existing = await _locationRepository.GetByCoordinatesAsync(request.Latitude, request.Longitude);
            if (existing != null) return Ok(existing);

            var newLoc = await _locationRepository.AddLocationAsync(new Location
            {
                Latitude = request.Latitude,
                Longitude = request.Longitude
            });

            return Ok(newLoc);
        }

        [HttpGet("weather")]
        public async Task<IActionResult> GetForecast(double lat, double lon)
        {
            //It doesn't involve the repository, because it's not interacting with the database — just calling an external API.
            var forecast = await _weatherService.GetWeatherAsync(lat, lon);
            return Ok(forecast);
        }

        [HttpGet("list")]
        public async Task<IActionResult> ListLocations()
        {
            var list = await _locationRepository.GetAllLocationsAsync();
            return Ok(list);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            var success = await _locationRepository.DeleteLocationAsync(id);
            return success ? Ok() : NotFound();
        }
    }
}
