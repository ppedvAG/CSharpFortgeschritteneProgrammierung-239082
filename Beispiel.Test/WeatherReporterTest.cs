using Beispiel.Weather;
using Moq;
using System.IO;

namespace Beispiel_To.Test
{
    [TestClass]
    public class WeatherReporterTest
    {
        private Mock<IWeatherService> _weatherService = new Mock<IWeatherService>();

        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void Init()
        {
            _weatherService.Setup(e => e.CurrentLocation)
                .Returns(Location.Honolulu);
        }

        [TestCleanup]
        public void Cleanup()
        {
            // Loesche temoporaere Dateien
        }

        [TestMethod]
        public void Test_GetFormattedWeatherReport()
        {
            // Arrange
            _weatherService.Setup(e => e.GetWeatherForecast())
                .Returns("Max Meier");

            var uut = new WeatherReporter(_weatherService.Object);            

            // Act
            var result = uut.GetFormattedWeatherReport();

            File.WriteAllText($"{TestContext.TestResultsDirectory}/{TestContext.TestName}.txt", result);

            // Assert
            Assert.AreEqual("Das aktuelle Wetter: Max Meier", result);
        }
    }
}