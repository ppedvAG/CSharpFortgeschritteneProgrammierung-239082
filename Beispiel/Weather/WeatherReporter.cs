namespace Beispiel.Weather
{
    public class WeatherReporter
    {
        private readonly IWeatherService _weatherService;

        public WeatherReporter(IWeatherService weatherService)
        {
            _weatherService = weatherService;
            _weatherService.SetLocation(Location.Paris);
        }

        public string GetFormattedWeatherReport()
        {
            var forecast = _weatherService.GetWeatherForecast();
            return $"Das aktuelle Wetter: {forecast}";
        }
    }
}
