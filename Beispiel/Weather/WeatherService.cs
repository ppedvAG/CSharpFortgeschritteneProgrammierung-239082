namespace Beispiel.Weather
{
    public class WeatherService : IWeatherService
    {
        public Location CurrentLocation { get; private set; }

        public string GetWeatherForecast()
        {
            // Hier würden wir normalerweise eine API aufrufen oder eine andere Logik verwenden,
            // um die Wettervorhersage zu erhalten. Für dieses Beispiel geben wir einfach einen Dummy-Text zurück.
            switch (CurrentLocation)
            {
                case Location.Honolulu:
                    return "Sonnig, 25°C";
                case Location.Paris:
                    return "Bewölkt, 18°C";
                case Location.London:
                    return "Regen, 12°C";
                case Location.NewYork:
                    return "Stürmisch, 5°C";
                default:
                    return "Standortinformation nicht verfügbar";
            }
        }

        public void SetLocation(Location location)
        {
            CurrentLocation = location;
        }
    }
}
