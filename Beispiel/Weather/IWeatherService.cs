namespace Beispiel.Weather
{
    public interface IWeatherService
    {
        Location CurrentLocation { get; }

        string GetWeatherForecast();

        void SetLocation(Location location);
    }
}
