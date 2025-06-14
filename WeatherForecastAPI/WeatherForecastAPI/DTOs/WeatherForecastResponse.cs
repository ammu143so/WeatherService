namespace WeatherForecastAPI.DTOs
{
    public class WeatherForecastResponse
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime Date { get; set; }
        public double Temperature { get; set; }
    }
}
