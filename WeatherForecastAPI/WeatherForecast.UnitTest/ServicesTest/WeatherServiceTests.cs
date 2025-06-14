using Microsoft.Extensions.Logging;
using Moq;
using RichardSzalay.MockHttp;
using WeatherForecastAPI.Services;

namespace WeatherForecast.UnitTest.ServicesTest
{
    public class WeatherServiceTests
    {
        [Fact]
        public async Task GetWeatherAsync_ReturnsValidData()
        {
            var handler = new MockHttpMessageHandler();
            handler.When("*")
                .Respond("application/json", @"{
                ""current_weather"": {
                    ""temperature"": 25.5,
                    ""time"": ""2024-06-10T10:00:00Z""
                }
            }");

            var client = new HttpClient(handler)
            {
                BaseAddress = new Uri("https://api.open-meteo.com/")
            };

            var logger = new Mock<ILogger<WeatherService>>().Object;
            var service = new WeatherService(client, logger);

            var result = await service.GetWeatherAsync(28.6, 77.2);

            Assert.NotNull(result);
            Assert.Equal(25.5, result.Temperature);
            Assert.Equal(28.6, result.Latitude);
        }
    }
}
